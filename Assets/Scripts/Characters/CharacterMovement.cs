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
		Tile.TileEntered += OnTileEnter;
	}

	private void OnDisable()
	{
		Dice.DiceRollEvent -= OnDiceRolled;
		Tile.TileEntered -= OnTileEnter;
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
        if (TilesToMove > 0)
        {
            //MoveForward
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

            TilesToMove--;
                        
            CurrentTile.GetComponent<Tile>().OnCharacterEnter(TilesToMove, photonView.viewID);
        }
        else if (TilesToMove < 0)
        {
            //MoveBackward
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

            TilesToMove++;

            CurrentTile.GetComponent<Tile>().OnCharacterEnter(TilesToMove, photonView.viewID);
        }
        else
        {
            Debug.LogError("Somethig's wrong. Trying to move, but TilesToMove = 0");
        }
    }
    
    private void OnTileEnter(int tilesToMove, int currentTurnCharacterViewID)
    {
        if (photonView.viewID != currentTurnCharacterViewID)
            return;

        if (tilesToMove > 0)
        {
            CurrentTile = TileManager.Instance.Tiles[NextTile.GetComponent<Tile>().index];
            if (NextTile.GetComponent<Tile>().index < TileManager.Instance.Tiles.Count - 1)
            {
                NextTile = TileManager.Instance.Tiles[NextTile.GetComponent<Tile>().index + 1];
            }
            else
            {
                DebugUtility.Log("NextTile is the last tile, it's better be the goal!");
            }

            StartCoroutine(Move());
        }
        else if (tilesToMove < 0)
        {
            CurrentTile = TileManager.Instance.Tiles[NextTile.GetComponent<Tile>().index];
            if (NextTile.GetComponent<Tile>().index > 0)
            {
                NextTile = TileManager.Instance.Tiles[NextTile.GetComponent<Tile>().index - 1];
            }
            else
            {
                DebugUtility.Log("Next tile is start tile no more going back!");
            }

            StartCoroutine(Move());
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
                if (PhotonNetwork.isMasterClient)
                {
					GameManager.Instance.TurnManager.TurnEnd();                    
                }
            }
        }
    }
}