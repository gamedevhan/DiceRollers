using System.Collections;
using UnityEngine;

public class CharacterMovement : Photon.PunBehaviour
{
	private const float lerpThreshold = 0.05f;

	public int tilesToMove = 0;
	public bool IsMoving = false;
	public bool IsMovingForward = true;
	
	public float Speed = 1.0F;
		
	public Transform CurrentTile;
	public Transform NextTile;
	
	private float startTime;
	private float journeyLength;

	private void OnEnable()
	{
		Dice.DiceRollEvent += OnDiceRolled;
		Tile.TileEntered += OnTileEntered;
	}

	private void OnDisable()
	{
		Dice.DiceRollEvent -= OnDiceRolled;
		Tile.TileEntered -= OnTileEntered;
	}

	private void Start()
	{		
		CurrentTile = TileManager.Instance.Tiles[0];
		NextTile = TileManager.Instance.Tiles[1];	
	}

	// This method listens to DiceRolled event
	private void OnDiceRolled(int diceResult)
	{
		if (photonView.isMine)
		{
			int movingCharacterViewID = photonView.viewID;
			photonView.RPC("MoveCharacter", PhotonTargets.All, diceResult, movingCharacterViewID);
		}
	}

	// This method listens to TileEntered event
	private void OnTileEntered()
	{	
		if (tilesToMove != 0) // Call Move() again if there is still tiles to move
			StartCoroutine(Move());
		else if (tilesToMove == 0) // Otherwise, stop moving
		{				
			IsMoving = false;
			IsMovingForward = true;
			
			// SpecialTileEffect
			ISpecialTile specialTile = CurrentTile.GetComponent<ISpecialTile>();
			if (specialTile != null)
				StartCoroutine(specialTile.SpecialTileEffect());
			else // TurnEnd. Update currentTurnPlayer, then begin turn
			{
				RaiseEventOptions eventOptions = new RaiseEventOptions() { CachingOption = EventCaching.DoNotCache, Receivers = ReceiverGroup.MasterClient };
				PhotonNetwork.RaiseEvent(PhotonEventCode.TurnEnd, null, true, eventOptions);
			}	
		}
	}

	public IEnumerator Move()
	{	
		transform.LookAt(NextTile);

		// Lerp		
		startTime = Time.time;
		journeyLength = Vector3.Distance(CurrentTile.position, NextTile.position);
		IsMoving = true;
		
		while (Vector3.Distance(transform.position, NextTile.position) > lerpThreshold)
		{
			float distCovered = (Time.time - startTime) * Speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(CurrentTile.position, NextTile.position, fracJourney);
			yield return null;
		}

		// Snap to the destination when distance between transform.position <= lerpThreshold
		transform.position = NextTile.position;		

		// Update tilesToMove
		if (IsMovingForward)
			tilesToMove--;
		else
			tilesToMove++;
				
		// Update CurrentTile and NextTile
		CurrentTile = TileManager.Instance.Tiles[NextTile.GetComponent<Tile>().index];
			
		if (tilesToMove >= 0)
			NextTile = TileManager.Instance.Tiles[NextTile.GetComponent<Tile>().index + 1];
		else if (tilesToMove < 0)
			NextTile = TileManager.Instance.Tiles[NextTile.GetComponent<Tile>().index - 1];		
		else
			Debug.LogError("There is a logic error with updating NextTile");
		
		NextTile.GetComponent<Tile>().OnCharacterEnter();
	}

	[PunRPC]
	private void MoveCharacter(int diceResult, int movingCharacterViewID)
	{
		if (photonView.viewID != movingCharacterViewID)
			return;
		
		tilesToMove = diceResult;
		IsMovingForward = true;
		StartCoroutine(Move());
	}
}