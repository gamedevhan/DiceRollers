using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Tile))]
public class MoveTile : MonoBehaviour, ISpecialTile
{
	private const float FxDelay = 1f;
    
	[SerializeField] private int amount;
    
	public IEnumerator OnSpecialTileEnter(CharacterMovement character)
	{
		// TODO: Play FX

		yield return new WaitForSeconds(FxDelay);
				
		character.TilesToMove += amount;
		Debug.Log("Entered MoveTile, Tiles to Move: " + character.TilesToMove);

        Move(character);
	}

	private void Move(CharacterMovement character)
	{
		if (amount > 0)
			MoveForward(character);
		else if (amount < 0)		
			MoveBackWard(character);
		else
			Debug.Log("Check the inspector, amount is probably set to 0");
	}

	private void MoveForward(CharacterMovement character)
    { 
		StartCoroutine(character.Move());
	}

	private void MoveBackWard(CharacterMovement character)
	{	
		character.NextTile = TileManager.Instance.Tiles[character.CurrentTile.GetComponent<Tile>().index - 1];
        StartCoroutine(character.Move());
    }
}
