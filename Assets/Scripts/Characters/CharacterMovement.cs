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
		Move();
	}

    public void Move()
    {
        if (TilesToMove > 0)
        {
			StartCoroutine(MoveToNextTile());
            TilesToMove--;              
			UpdateTiles();            
        }
        else if (TilesToMove < 0)
        {            
            StartCoroutine(MoveToNextTile());
            TilesToMove++;
            UpdateTiles();
        }
        else
        {
            Debug.LogError("Somethig's wrong. Trying to move, but TilesToMove = 0");
        }
    }
    
    private void UpdateTiles()
    {        
        if (TilesToMove > 0) // Going Forward
        {
            CurrentTile = TileManager.Instance.Tiles[NextTile.GetComponent<Tile>().index];
			if (!(CurrentTile.GetComponent<Tile>().index == TileManager.Instance.Tiles.Count - 1))
			{
				Move();	
			}
			else
			{
				DebugUtility.Log("On the Last Tile!");
			}
        }
        else if (TilesToMove < 0) // Going Backward
        {
            CurrentTile = TileManager.Instance.Tiles[NextTile.GetComponent<Tile>().index];
			if (!(CurrentTile.GetComponent<Tile>().index == 0))
			{
				Move();
			}
			else
			{
				DebugUtility.Log("On the First Tile!");
			}
			
        }
        else
        {
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

	private IEnumerator MoveToNextTile()
	{
		transform.LookAt(NextTile);

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
	}
}