using System.Collections.Generic;
using UnityEngine;

public class IntersectionTile : Tile
{
	[SerializeField]
	private GameObject arrowPrefab;	
	private List<Tile> nextTiles = new List<Tile>();
	private Dictionary<Arrow, Tile> arrowToTile = new Dictionary<Arrow, Tile>();
    private CharacterMovementController enteredCharacter;
	private PhotonView photonView;

	private void Awake()
	{
		photonView = GetComponent<PhotonView>();
	}

	protected override void Start()
	{
		base.Start();
		for (int i = 1; i < neighbourTiles.Count; i++)
		{
			nextTiles.Add(neighbourTiles[i]);

            if (!PhotonNetwork.isMasterClient)
            {
                GameObject arrowGameObject = PhotonNetwork.InstantiateSceneObject(arrowPrefab.name, transform.position, Quaternion.identity, 0, null);
                Arrow arrow = arrowGameObject.GetComponent<Arrow>();
                arrow.IntersectionTile = this;
                int arrowViewID = arrow.PhotonView.viewID;
                photonView.RPC("InitializeArrow", PhotonTargets.All, arrowViewID, i);

                // TODO: All players need to have populated dictionary, currently only masterClient has ditionary populated
                arrowToTile.Add(arrow, neighbourTiles[i]);
            }
		}
	}
	
	public override void OnCharacterEnter(CharacterMovementController character)
	{		
        enteredCharacter = character;
        enteredCharacter.TileBeforeMove = this;

        if (enteredCharacter.MoveLeft < 0)
        {
            enteredCharacter.TileAfterMove = PreviousTile;
            StartCoroutine(enteredCharacter.Move());
        }
        else
        {
            enteredCharacter.GetComponent<CharacterAnimationController>().PlayWalkAnimation(false);

            foreach (Arrow arrow in arrowToTile.Keys)
            {
                arrow.MeshRenderer.enabled = true;
                if (PhotonNetwork.player.ID == GameManager.Instance.TurnManager.CurrentTurnPlayerID)
                {
                    if (!arrow.IsActive)
                    {
                        arrow.SetActivte(true);
                    }
                }
                else
                {
                    if (arrow.IsActive)
                    {
                        arrow.SetActivte(false);
                    }
                }
            }
        }
	}
    
    public void OnArrowPress(Arrow arrow)
    {
        NextTile = arrowToTile[arrow];
        enteredCharacter.TileAfterMove = NextTile;
        
        if (enteredCharacter.MoveLeft > 0)
        {
            enteredCharacter.PhotonView.RPC("MoveCharacter", PhotonTargets.All, enteredCharacter.MoveLeft);
        }
        else
        {            
            GameManager.Instance.TurnManager.TurnEnd();
        }

        foreach (Arrow item in arrowToTile.Keys)
        {
            item.SetActivte(false);
        }
    }

	[PunRPC]
	private void InitializeArrow(int arrowViewID, int indexOfNeighbourTile)
	{
		Arrow arrow = PhotonView.Find(arrowViewID).GetComponent<Arrow>();			
		arrowToTile.Add(arrow, neighbourTiles[indexOfNeighbourTile]);

		Vector3 arrowPosition = new Vector3((neighbourTiles[indexOfNeighbourTile].transform.position.x + transform.position.x) / 2, 0, (neighbourTiles[indexOfNeighbourTile].transform.position.z + transform.position.z) / 2);
		arrow.transform.position = arrowPosition;

		Vector3 targetPosition = new Vector3(neighbourTiles[indexOfNeighbourTile].transform.position.x, transform.position.y, neighbourTiles[indexOfNeighbourTile].transform.position.z);
		arrow.transform.LookAt(targetPosition);
	}
}
