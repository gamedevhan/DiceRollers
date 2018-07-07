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

	private IEnumerator Move()
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
        if (TilesToMove > 0) // Going Forward
        {
			// Are we on the last tile?
			if (NextTile.GetComponent<Tile>().index == TileManager.Instance.Tiles.Count - 1)
			{
				DebugUtility.Log("On the Last Tile!");
			}
			else
			{
				CurrentTile = TileManager.Instance.Tiles[CurrentTile.GetComponent<Tile>().index + 1];
				NextTile = TileManager.Instance.Tiles[CurrentTile.GetComponent<Tile>().index + 1];
				Move();
			}
        }
        else if (TilesToMove < 0) // Going Backward
        {
			// Are we on the first tile?
			if (CurrentTile.GetComponent<Tile>().index == 1)
			{
				DebugUtility.Log("On the first Tile!");
			}
			else
			{
				CurrentTile = TileManager.Instance.Tiles[CurrentTile.GetComponent<Tile>().index - 1];
				NextTile = TileManager.Instance.Tiles[CurrentTile.GetComponent<Tile>().index - 1];
			}			
        }
        else // TilesToMove == 0
        {
			// TODO: Need to check if tile we just stopped is first or last tile
			// And update currentTile and NextTile

            ShouldPlayMoveAnim = false;

            // No move left. Check if this tile is special tile. If not, end turn.
            ISpecialTile specialTile = CurrentTile.GetComponent<ISpecialTile>();
            if (specialTile != null)
            {
                specialTile.SpecialTileEffect();
            }
            else
            {
				GameManager.Instance.TurnManager.TurnEnd();
            }
        }
    }
}