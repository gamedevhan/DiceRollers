using UnityEngine;

public class Tile : MonoBehaviour
{
	public int index;

	public void OnCharacterEnter(CharacterMovementController character)
	{
		character.OnTileEnter(index);
	}
}