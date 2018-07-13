using System.Collections.Generic;
using UnityEngine;

public class IntersectionTile : Tile
{
	[SerializeField]
	private GameObject arrowPrefab;	
	private List<Tile> nextTiles = new List<Tile>();
	private Dictionary<Arrow, Tile> selectedTile = new Dictionary<Arrow, Tile>();
    private CharacterMovementController enteredCharacter;

	protected override void Start()
	{
		base.Start();
		for (int i = 1; i < neighbourTiles.Count; i++)
		{
			nextTiles.Add(neighbourTiles[i]);
			GameObject arrowGameObject = Instantiate(arrowPrefab, transform);
			Arrow arrow = arrowGameObject.GetComponent<Arrow>();
			selectedTile.Add(arrow, neighbourTiles[i]);

			Vector3 arrowPosition = new Vector3((neighbourTiles[i].transform.position.x + transform.position.x) / 2, 0, (neighbourTiles[i].transform.position.z + transform.position.z) / 2);
			arrow.transform.position = arrowPosition;

			Vector3 targetPosition = new Vector3(neighbourTiles[i].transform.position.x, transform.position.y, neighbourTiles[i].transform.position.z);
			arrow.LookatTarget(targetPosition);
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
            enteredCharacter.ShouldPlayMoveAnim = false;

            foreach (Arrow arrow in selectedTile.Keys)
            {
                arrow.MeshRenderer.enabled = true;
                if (PhotonNetwork.player.ID == GameManager.Instance.TurnManager.CurrentTurnPlayerID)
                {
                    if (!arrow.Collider.enabled)
                    {
                        arrow.Collider.enabled = true;
                    }
                }
                else
                {
                    if (arrow.Collider.enabled)
                    {
                        arrow.Collider.enabled = false;
                    }
                }
            }
        }
	}
    
    public void OnArrowPress(Arrow arrow)
    {
        NextTile = selectedTile[arrow];
        enteredCharacter.TileAfterMove = NextTile;
        
        if (enteredCharacter.MoveLeft > 0)
        {
            enteredCharacter.PhotonView.RPC("MoveCharacter", PhotonTargets.All, enteredCharacter.MoveLeft);
        }
        else
        {
            GameManager.Instance.TurnManager.TurnEnd();
        }
    }
}
