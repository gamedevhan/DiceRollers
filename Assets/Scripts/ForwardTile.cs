using System.Collections;
using UnityEngine;

public class ForwardTile : MonoBehaviour, ISpecialTile
{
	private const float FxDelay = 1f;

	[SerializeField] private int amount;

	public IEnumerator SpecialTileEffect()
	{
		// TODO: Play FX

		yield return new WaitForSeconds(FxDelay);
				
		Dice.DiceResult += amount;
		Debug.Log("Tiles to Move: " + Dice.DiceResult);
		
		CharacterMovement character = FindObjectOfType<CharacterMovement>();
		StartCoroutine(character.MoveOneTile());
	}
}
