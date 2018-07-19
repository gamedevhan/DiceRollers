using System.Collections.Generic;
using UnityEngine;

public class IntersectionTile : Tile
{
	[SerializeField]
	private GameObject arrowPrefab;
    private List<Tile> nextTiles = new List<Tile>();
    private List<int> arrowViewIDs = new List<int>();
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

            if (PhotonNetwork.isMasterClient)
            {
                GameObject arrowGameObject = PhotonNetwork.InstantiateSceneObject(arrowPrefab.name, transform.position, Quaternion.identity, 0, null);
                Arrow arrow = arrowGameObject.GetComponent<Arrow>();
                int arrowViewID = arrow.PhotonView.viewID;

                arrowViewIDs.Add(arrowViewID);                
                arrow.PointingTile = nextTiles[i - 1];
                
                photonView.RPC("InitializeArrow", PhotonTargets.All, arrowViewID, photonView.viewID, i);
            }
		}

        if (PhotonNetwork.isMasterClient)
        {
            int[] viewIDs = new int[arrowViewIDs.Count];
            for (int i = 0; i < arrowViewIDs.Count; i++)
            {
                viewIDs[i] = arrowViewIDs[i];
            }

            photonView.RPC("SyncArrowList", PhotonTargets.All, viewIDs);
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

            foreach (int viewID in arrowViewIDs)
            {
                Arrow arrow = PhotonView.Find(viewID).GetComponent<Arrow>();
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
        NextTile = arrow.PointingTile;
        enteredCharacter.TileAfterMove = NextTile;
        
        if (enteredCharacter.MoveLeft > 0)
        {
            //enteredCharacter.PhotonView.RPC("MoveCharacter", PhotonTargets.All, enteredCharacter.MoveLeft);
            StartCoroutine(enteredCharacter.Move());
        }
        else
        {            
            GameManager.Instance.TurnManager.TurnEnd();
        }

        photonView.RPC("HideAllArrow", PhotonTargets.All);
    }

	[PunRPC]
	private void InitializeArrow(int arrowViewID, int intersectionTileViewID, int indexOfNeighbourTile)
	{
		Arrow arrow = PhotonView.Find(arrowViewID).GetComponent<Arrow>();
        IntersectionTile intersectionTile = PhotonView.Find(intersectionTileViewID).GetComponent<IntersectionTile>();

        arrow.IntersectionTile = intersectionTile;

        Vector3 arrowPosition = new Vector3((neighbourTiles[indexOfNeighbourTile].transform.position.x + transform.position.x) / 2, 0, (neighbourTiles[indexOfNeighbourTile].transform.position.z + transform.position.z) / 2);
		arrow.transform.position = arrowPosition;

		Vector3 targetPosition = new Vector3(neighbourTiles[indexOfNeighbourTile].transform.position.x, transform.position.y, neighbourTiles[indexOfNeighbourTile].transform.position.z);
		arrow.transform.LookAt(targetPosition);
	}

    [PunRPC]
    private void SyncArrowList(int[] viewIDs)
    {
        foreach (int arrowViewID in viewIDs)
        {
            arrowViewIDs.Add(arrowViewID);
        }
    }

    [PunRPC]
    private void HideAllArrow()
    {
        foreach (int viewID in arrowViewIDs)
        {
            Arrow eachArrow = PhotonView.Find(viewID).GetComponent<Arrow>();
            eachArrow.SetActivte(false);
        }
    }
}
