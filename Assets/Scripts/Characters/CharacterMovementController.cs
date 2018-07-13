using System.Collections;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
	public int MoveLeft;
	public bool ShouldPlayMoveAnim = false;		
	public float Speed = 1.0F;
	
	public Tile TileBeforeMove;
	public Tile TileAfterMove;

	private const float lerpThreshold = 0.05f;
	private float startTime;
	private float journeyLength;
	
	public PhotonView PhotonView { get; private set; }

	private void Awake()
	{
		PhotonView = GetComponent<PhotonView>();
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
		TileBeforeMove = GamePlayerSpawnManager.Instance.startTile;
		TileAfterMove = TileBeforeMove.NextTile;
	}

	// This method listens to DiceRolled event
	private void OnDiceRolled(int diceResult)
	{
		if (PhotonView.isMine)
		{			
			PhotonView.RPC("MoveCharacter", PhotonTargets.All, diceResult);
		}
	}

	[PunRPC]
	private void MoveCharacter(int moveLeft)
	{		
		MoveLeft = moveLeft;
		StartCoroutine(Move());
	}

	public IEnumerator Move()
	{
		transform.LookAt(TileAfterMove.transform);

		#region Lerp
		startTime = Time.time;
        journeyLength = Vector3.Distance(TileBeforeMove.transform.position, TileAfterMove.transform.position);
        ShouldPlayMoveAnim = true;

        while (Vector3.Distance(transform.position, TileAfterMove.transform.position) > lerpThreshold)
        {
            float distCovered = (Time.time - startTime) * Speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(TileBeforeMove.transform.position, TileAfterMove.transform.position, fracJourney);
            yield return null;
        }

        transform.position = TileAfterMove.transform.position;

		#endregion

		if (MoveLeft > 0)
		{
			MoveLeft--;
		}
		else if (MoveLeft < 0)
		{
			MoveLeft++;
		}

		TileAfterMove.OnCharacterEnter(this);
	}
}