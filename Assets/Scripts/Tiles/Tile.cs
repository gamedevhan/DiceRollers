using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public abstract class Tile : MonoBehaviour
{
	public int index;

	protected Tile previousTile;
	protected Tile nextTile;
	protected List<Tile> neighbourTiles = new List<Tile>();

	protected const float neighbourDistanceThreshold = 3.4f;

	protected virtual void Start()
	{
		Tile[] tiles = FindObjectsOfType<Tile>();
		float distanceFromThisTile = new float();		
		foreach (var tile in tiles)
		{
			distanceFromThisTile = Vector3.Distance(transform.position, tile.transform.position);
			if (distanceFromThisTile <= neighbourDistanceThreshold && tile != this)
			{
				neighbourTiles.Add(tile);
			}
		}

		neighbourTiles = neighbourTiles.OrderBy(x => x.index).ToList();
		previousTile = neighbourTiles[0];
		nextTile = neighbourTiles[neighbourTiles.Count - 1];
	}

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