using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private const float lerpThreshold = 0.05f;

	private int tilesToMove = 0;
	public bool IsMoving = false;
	public bool IsMovingForward = true;

	public float Speed = 1.0F;
	
	private int tileIndex = 0;
	public Transform CurrentTile;
	public Transform NextTile;
	
	private float startTime;
	private float journeyLength;

	private void OnEnable()
	{
		Dice.DiceRolled += OnDiceRolled;
		Tile.TileEntered += OnTileEntered;
	}

	private void OnDisable()
	{
		Dice.DiceRolled -= OnDiceRolled;
		Tile.TileEntered -= OnTileEntered;
	}

	private void Start()
	{		
		CurrentTile = TileManager.Instance.Tiles[tileIndex];
		NextTile = TileManager.Instance.Tiles[tileIndex + 1];		
	}

	// This method listens to DiceRolled event
	private void OnDiceRolled()
	{	
		StartCoroutine(Move());
		IsMovingForward = true;
		tilesToMove = Dice.DiceResult;
	}

	// This method listens to TileEntered event
	private void OnTileEntered()
	{	
		// Update tile index
		if (IsMovingForward)		
			tileIndex++;
		else
		{
			tileIndex--;
		}

		if(tilesToMove == 0)
			IsMoving = false;

		CurrentTile = TileManager.Instance.Tiles[tileIndex];

		if (IsMovingForward)		
			NextTile = TileManager.Instance.Tiles[tileIndex + 1];
		else
			NextTile = TileManager.Instance.Tiles[tileIndex - 1];

		Debug.Log("<color=yellow> Current tile Index is: </color>" + "<b>" + CurrentTile.GetComponent<Tile>().index + "</b>" + "<color=yellow>, Next tile Index is: </color>" + "<b>" + NextTile.GetComponent<Tile>().index + "</b>");

		if (tilesToMove > 0) // Still moving
		{			
			StartCoroutine(Move());
		}
		if (tilesToMove < 0) // Going Backward
		{
			StartCoroutine(Move());
		}
		if (tilesToMove == 0) // Finished moving
		{	
			IsMoving = false;
			transform.LookAt(NextTile);

			ISpecialTile specialTile = CurrentTile.GetComponent<ISpecialTile>();
			if (specialTile != null)
				StartCoroutine(specialTile.SpecialTileEffect());
		}
	}

	public IEnumerator Move()
	{	
		transform.LookAt(NextTile);
		Debug.Log(name + " Start lerping");

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

		//Snap to the destination when distance between transform.position <= lerpThreshold
		transform.position = NextTile.position;
		Debug.Log(name + " Reached next tile.");
		
		tilesToMove--;
		NextTile.GetComponent<Tile>().OnCharacterEnter();
	}

	// For testing
	public void ResetPosition()
	{
		transform.position = new Vector3(0, 0, 0);
		tileIndex = 0;
		CurrentTile = TileManager.Instance.Tiles[tileIndex];
		NextTile = TileManager.Instance.Tiles[tileIndex + 1];
		transform.LookAt(NextTile);
	}
}