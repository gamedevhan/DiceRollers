using System.Collections;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
	public int MoveLeft;
	public bool ShouldPlayMoveAnim = false;		
	public float Speed = 1.0F;		
	public Transform CurrentTile;
	public Transform NextTile;
	
	private const float lerpThreshold = 0.05f;
	private float startTime;
	private float journeyLength;
	private PhotonView photonView;

	private void Awake()
	{
		photonView = GetComponent<PhotonView>();
	}

	private void OnEnable()
	{
		Dice.DiceRollEvent += OnDiceRolled;
	}

	private void OnDisable()
	{
		Dice.DiceRollEvent -= OnDiceRolled;		
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
			photonView.RPC("MoveCharacter", PhotonTargets.All, diceResult);
		}
	}

	[PunRPC]
	private void MoveCharacter(int moveLeft)
	{		
		MoveLeft = moveLeft;
		DebugUtility.Log("Moving! tilestoMove = " + MoveLeft);
		StartCoroutine(Move());
	}

	public IEnumerator Move()
	{
		transform.LookAt(NextTile);

		#region Lerp
		startTime = Time.time;
        journeyLength = Vector3.Distance(CurrentTile.position, NextTile.position);
        ShouldPlayMoveAnim = true;

        while (Vector3.Distance(transform.position, NextTile.position) > lerpThreshold)
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(CurrentTile.position, NextTile.position, fracJourney);
            yield return null;
        }

        transform.position = NextTile.position;

		#endregion

		if (MoveLeft > 0)
		{
			MoveLeft--;
		}
		else if (MoveLeft < 0)
		{
			MoveLeft++;
		}

		NextTile.GetComponent<Tile>().OnCharacterEnter(this);
	}
}