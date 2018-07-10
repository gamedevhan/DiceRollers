﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Tile))]
public class MoveTile : MonoBehaviour, ISpecialTile
{
	private float fxDelay = 1f;
    
    [SerializeField] private GameObject moveFX;

    [SerializeField] private int moveAmount;
    
	public IEnumerator OnSpecialTileEnter(CharacterMovementController character)
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
		if (moveAmount > 0)
			MoveForward(character);
		else if (moveAmount < 0)		
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
