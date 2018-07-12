using UnityEngine;

public abstract class Tile : MonoBehaviour
{
	public int index;

	protected Tile previousTile;
	protected Tile nextTile;

    public virtual void OnCharacterEnter(CharacterMovementController character)
	{
		if (character.MoveLeft > 0) // character is moving forward
        {         
            {
                character.TileBeforeMove = TileManager.Instance.Tiles[index];
                character.TileAfterMove = TileManager.Instance.Tiles[index + 1];
                StartCoroutine(character.Move());
            }
        }
        else if (character.MoveLeft < 0) // character is moving backward
        {                  
            character.TileBeforeMove = TileManager.Instance.Tiles[index];
            character.TileAfterMove = TileManager.Instance.Tiles[index - 1];
            StartCoroutine(character.Move());         
        }
        else // if (character.MoveLeft == 0)
        {
            character.TileBeforeMove = TileManager.Instance.Tiles[index];
            character.TileAfterMove = TileManager.Instance.Tiles[index + 1];
            character.ShouldPlayMoveAnim = false;

			ISpecialTile specialTile = GetComponent<ISpecialTile>();
			if (specialTile != null)
			{
				StartCoroutine(specialTile.SpecialTileBehaviour(character));
			}
			else
			{
				GameManager.Instance.TurnManager.TurnEnd();
			}
        }
	}
}