using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	protected List<Tile> neighbourTiles = new List<Tile>();
	protected const float neighbourDistanceThreshold = 3.4f;

	public int Index { get; set; }
	public Tile PreviousTile { get; private set; }
	public Tile NextTile { get; private set; }

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
				
		neighbourTiles = neighbourTiles.OrderBy(x => x.Index).ToList();
		PreviousTile = neighbourTiles[0];
		NextTile = neighbourTiles[neighbourTiles.Count - 1];
	}

    public virtual void OnCharacterEnter(CharacterMovementController character)
	{
		if (character.MoveLeft > 0) // character is moving forward
        {         
            {
                character.TileBeforeMove = this;
                character.TileAfterMove = NextTile;
                StartCoroutine(character.Move());
            }
        }
        else if (character.MoveLeft < 0) // character is moving backward
        {                  
            character.TileBeforeMove = this;
            character.TileAfterMove = PreviousTile;
            StartCoroutine(character.Move());         
        }
        else // if (character.MoveLeft == 0)
        {
            character.TileBeforeMove = this;
            character.TileAfterMove = NextTile;
            character.GetComponent<CharacterAnimationController>().PlayWalkAnimation(false);

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