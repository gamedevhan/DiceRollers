using UnityEngine;

public class IntersectionTile : Tile
{
	[SerializeField]	
	private GameObject[] arrows;
	private Tile[] neighbourTiles;			

	public override void OnCharacterEnter(CharacterMovementController character)
	{
		
	}
}
