using System.Collections;
using UnityEngine;

public class CharacterMovement : Photon.PunBehaviour
{
	public int TilesToMove;
	public bool ShouldPlayMoveAnim = false;		
	public float Speed = 1.0F;		
	public Transform CurrentTile;
	public Transform NextTile;
	
	private const float lerpThreshold = 0.05f;
	private float startTime;
	private float journeyLength;

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
		TilesToMove = moveLeft;
		DebugUtility.Log("Moving! tilestoMove = " + TilesToMove);
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

		if (TilesToMove > 0)
		{
			TilesToMove--;
		}
		else if (TilesToMove < 0)
		{
			TilesToMove++;
		}

		OnNextTileEnter();
	}

    private void OnNextTileEnter()
    {
        int enteredTileIndex = NextTile.GetComponent<Tile>().index;
        
        if (TilesToMove > 0) // Going Forward
        {
            // Are we on the last tile?
            if (enteredTileIndex == TileManager.Instance.Tiles.Count - 1)
			{
				DebugUtility.Log("On the Last Tile, You win!");
			}
			else
			{
				CurrentTile = TileManager.Instance.Tiles[enteredTileIndex];
				NextTile = TileManager.Instance.Tiles[enteredTileIndex + 1];
                StartCoroutine(Move());
			}
        }
        else if (TilesToMove < 0) // Going Backward
        {            
			// Are we on the first tile?
			if (enteredTileIndex == 0)
			{
				DebugUtility.Log("On the first Tile!");
                TilesToMove = 0;
                GameManager.Instance.TurnManager.TurnEnd(); // Moved backward and reached start tile, end turn
            }
			else
			{
				CurrentTile = TileManager.Instance.Tiles[enteredTileIndex];
				NextTile = TileManager.Instance.Tiles[enteredTileIndex - 1];
                StartCoroutine(Move());
            }			
        }
        else // TilesToMove == 0, No more moves left
        {            
            if (enteredTileIndex == TileManager.Instance.Tiles.Count - 1)
            {
                DebugUtility.Log("On the Last Tile, You win!");
            }            
            else if (enteredTileIndex == 0)
            {
                DebugUtility.Log("On the first Tile!");
                GameManager.Instance.TurnManager.TurnEnd(); // Moved backward and reached start tile, end turn
            }
            else
            {
                CurrentTile = TileManager.Instance.Tiles[enteredTileIndex];
                NextTile = TileManager.Instance.Tiles[enteredTileIndex + 1];

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