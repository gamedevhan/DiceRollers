using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private const float lerpThreshold = 0.05f;

	public bool isMoving = false;
	public float speed = 1.0F;	
	
	public Transform currentTile;
	public Transform nextTile;
		
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
	
	// This method listens to DiceRolled event
	public void OnDiceRolled()
	{		
		startTime = Time.time;
		journeyLength = Vector3.Distance(currentTile.position, nextTile.position);
		StartCoroutine(MoveOneTile());
	}

	// This method listens to TileEntered event
	public void OnTileEntered()
	{		
		currentTile = nextTile;
		nextTile = currentTile.GetComponent<Tile>().nextTile;
		isMoving = false;

		Dice.diceResult--;
		if (Dice.diceResult > 0)
		{
			startTime = Time.time;
			journeyLength = Vector3.Distance(currentTile.position, nextTile.position);
			StartCoroutine(MoveOneTile());
		}
	}

	public IEnumerator MoveOneTile()
	{
		Debug.Log(name + " Start lerping");
		isMoving = true;
		
		while (Vector3.Distance(transform.position, nextTile.position) > lerpThreshold)
		{
			float distCovered = (Time.time - startTime) * speed;
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
		transform.position = currentTile.position;
	}
}