﻿using System.Collections;
using UnityEngine;

public class MoveTile : Tile, ISpecialTile
{
	private float fxDelay = 1f;
    
    [SerializeField] private GameObject moveFX;
    [SerializeField] private int moveAmount;

    protected override void Start()
    {
        base.Start();
        fxDelay = moveFX.GetComponent<ParticleSystem>().main.duration;        
    }

    public override void OnCharacterEnter(CharacterMovementController character)
    {
        if (character.MoveLeft > 0) // character is moving forward
        {
            // Are we on the last tile?
            if (isEndTile)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                character.CurrentTile = TileManager.Instance.Tiles[index];
                character.NextTile = TileManager.Instance.Tiles[index + 1];
                StartCoroutine(character.Move());
            }
        }
        else if (character.MoveLeft < 0) // character is moving forward
        {
            // Are we on the first tile?
            if (isStartTile)
            {   
                character.MoveLeft = 0;
                GameManager.Instance.TurnManager.TurnEnd(); // Moved backward and reached start tile, end turn
            }
            else
            {
                character.CurrentTile = TileManager.Instance.Tiles[index];
                character.NextTile = TileManager.Instance.Tiles[index - 1];
                StartCoroutine(character.Move());
            }
        }
        else // if (character.MoveLeft == 0)
        {
            character.CurrentTile = TileManager.Instance.Tiles[index];
            character.NextTile = TileManager.Instance.Tiles[index + 1];
            character.ShouldPlayMoveAnim = false;
            SpecialTileBehaviour(character);
        }
    }

    public IEnumerator SpecialTileBehaviour(CharacterMovementController character)
	{
        Vector3 fxPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Instantiate(moveFX, fxPosition, moveFX.transform.rotation, null);
        yield return new WaitForSeconds(fxDelay);
     			
		character.MoveLeft += moveAmount;
		Debug.Log("Entered MoveTile, Tiles to Move: " + character.MoveLeft);

        Move(character);
	}
    
	private void Move(CharacterMovementController character)
	{
		if (character.MoveLeft > 0)
			MoveForward(character);
		else if (character.MoveLeft < 0)		
			MoveBackWard(character);
		else
			Debug.Log("Check the inspector, amount is probably set to 0");
	}

	private void MoveForward(CharacterMovementController character)
    {        
        StartCoroutine(character.Move());
	}

	private void MoveBackWard(CharacterMovementController character)
	{	
		character.NextTile = TileManager.Instance.Tiles[character.CurrentTile.GetComponent<Tile>().index - 1];
        StartCoroutine(character.Move());
    }
}
