using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private const float lerpThreshold = 0.05f;

	public bool IsMoving = false;
	public float Speed = 1.0F;
	
	private int tileIndex = 0;
	private Transform currentTile;
	private Transform nextTile;
		
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
		currentTile = TileManager.Instance.Tiles[tileIndex];
		nextTile = TileManager.Instance.Tiles[tileIndex + 1];		
	}

	// This method listens to DiceRolled event
	private void OnDiceRolled()
	{	
		StartCoroutine(MoveOneTile());
	}

	// This method listens to TileEntered event
	private void OnTileEntered()
	{	
		tileIndex++;
		currentTile = TileManager.Instance.Tiles[tileIndex];
		nextTile = TileManager.Instance.Tiles[tileIndex + 1];		

		Dice.DiceResult--;
		if (Dice.DiceResult > 0) // Still moving
		{			
			StartCoroutine(MoveOneTile());
		}
		else // Finished moving
		{			
			IsMoving = false;
		}
	}

	public IEnumerator MoveOneTile()
	{		
		transform.LookAt(nextTile);
		Debug.Log(name + " Start lerping");

		startTime = Time.time;
		journeyLength = Vector3.Distance(currentTile.position, nextTile.position);
		IsMoving = true;
		
		while (Vector3.Distance(transform.position, nextTile.position) > lerpThreshold)
		{
			float distCovered = (Time.time - startTime) * Speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(currentTile.position, nextTile.position, fracJourney);
			yield return null;
		}

		//Snap to the destination when distance between transform.position <= lerpThreshold
		transform.position = nextTile.position;
		Debug.Log(name + " Reached next tile.");

		nextTile.GetComponent<Tile>().OnCharacterEnter();
	}

	// For testing
	public void ResetPosition()
	{
		transform.position = new Vector3(0, 0, 0);
	}
}