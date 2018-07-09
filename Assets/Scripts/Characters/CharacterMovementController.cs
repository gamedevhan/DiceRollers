﻿using System.Collections;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
	public int MoveLeft;
	public bool ShouldPlayMoveAnim = false;		
	public float Speed = 1.0F;		
	public Transform CurrentTile;
	public Transform NextTile;
	
	private const float lerpThreshold = 0.05f;
	private float startTime;
	private float journeyLength;
	private PhotonView photonView;

	private void Awake()
	{
		photonView = GetComponent<PhotonView>();
	}

	private void OnEnable()
	{
		Dice.DiceRollEvent += OnDiceRolled;
	}

	private void OnDisable()
	{
		Dice.DiceRollEvent -= OnDiceRolled;		
	}

	private void Start()
	{		
		CurrentTile = TileManager.Instance.Tiles[0];
		NextTile = TileManager.Instance.Tiles[1];	
	}

	// This method listens to DiceRolled event
	private void OnDiceRolled(int diceResult)
	{
		if (photonView.isMine)
		{			
			photonView.RPC("MoveCharacter", PhotonTargets.All, diceResult);
		}
	}

	[PunRPC]
	private void MoveCharacter(int moveLeft)
	{		
		MoveLeft = moveLeft;
		DebugUtility.Log("Moving! tilestoMove = " + MoveLeft);
		StartCoroutine(Move());
	}

	public IEnumerator Move()
	{
		transform.LookAt(NextTile);

		#region Lerp
		startTime = Time.time;
        journeyLength = Vector3.Distance(CurrentTile.position, NextTile.position);
        ShouldPlayMoveAnim = true;

        while (Vector3.Distance(transform.position, NextTile.position) > lerpThreshold)
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(CurrentTile.position, NextTile.position, fracJourney);
            yield return null;
        }

        transform.position = NextTile.position;

		#endregion

		if (MoveLeft > 0)
		{
			MoveLeft--;
		}
		else if (MoveLeft < 0)
		{
			MoveLeft++;
		}

		NextTile.GetComponent<Tile>().OnCharacterEnter(this);
	}

    public void OnTileEnter(int tileIndex)
    {   
        if (MoveLeft > 0) // Going Forward
        {
            // Are we on the last tile?
            if (tileIndex == TileManager.Instance.Tiles.Count - 1)
			{
				DebugUtility.Log("On the Last Tile, You win!");
			}
			else
			{
				CurrentTile = TileManager.Instance.Tiles[tileIndex];
				NextTile = TileManager.Instance.Tiles[tileIndex + 1];
                StartCoroutine(Move());
			}
        }
        else if (MoveLeft < 0) // Going Backward
        {            
			// Are we on the first tile?
			if (tileIndex == 0)
			{
				DebugUtility.Log("On the first Tile!");
                MoveLeft = 0;
                GameManager.Instance.TurnManager.TurnEnd(); // Moved backward and reached start tile, end turn
            }
			else
			{
				CurrentTile = TileManager.Instance.Tiles[tileIndex];
				NextTile = TileManager.Instance.Tiles[tileIndex - 1];
                StartCoroutine(Move());
            }			
        }
        else // TilesToMove == 0, No more moves left
        {            
            if (tileIndex == TileManager.Instance.Tiles.Count - 1)
            {
                DebugUtility.Log("On the Last Tile, You win!");
            }            
            else if (tileIndex == 0)
            {
                DebugUtility.Log("On the first Tile!");
                GameManager.Instance.TurnManager.TurnEnd(); // Moved backward and reached start tile, end turn
            }
            else
            {
                CurrentTile = TileManager.Instance.Tiles[tileIndex];
                NextTile = TileManager.Instance.Tiles[tileIndex + 1];

                ShouldPlayMoveAnim = false;

                // No move left. Check if this tile is special tile. If not, end turn.
                ISpecialTile specialTile = CurrentTile.GetComponent<ISpecialTile>();
                if (specialTile != null)
                {
                    StartCoroutine(specialTile.OnSpecialTileEnter(this));
                }
                else
                {
                    GameManager.Instance.TurnManager.TurnEnd();
                }
            }
        }
    }
}