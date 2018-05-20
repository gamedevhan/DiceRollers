using System.Collections;
using UnityEngine;

public class MoveTile : MonoBehaviour, ISpecialTile
{
	private const float FxDelay = 1f;

	private CharacterMovement character;

	[SerializeField] private int amount;

	private void Start()
	{
		character = FindObjectOfType<CharacterMovement>();	
	}

	public IEnumerator SpecialTileEffect()
	{
		// TODO: Play FX

		yield return new WaitForSeconds(FxDelay);
				
		character.tilesToMove += amount;
		Debug.Log("Entered MoveTile, Tiles to Move: " + character.tilesToMove);
		
		Move();
	}

	private void Move()
	{
		if (amount > 0)
			StartCoroutine(character.Move());
		else if (amount < 0)
			MoveBackWard();
		else
			Debug.Log("Check the inspector, amount is probably set to 0");
	}

	private void MoveBackWard()
	{	
		character.IsMovingForward = false;
		character.NextTile = TileManager.Instance.Tiles[character.CurrentTile.GetComponent<Tile>().index - 1];		
		StartCoroutine(character.Move());
	}
}
