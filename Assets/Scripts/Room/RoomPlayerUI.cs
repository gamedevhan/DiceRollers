using UnityEngine;

public class RoomPlayerUI : MonoBehaviour
{		
	public UILabel PlayerName;
	public UISprite ReadySprite;
	public bool IsReady = false;
	
	[SerializeField]
	private GameObject uiButtons;

	private PhotonView photonView;
		
	private void Awake()
	{	
		photonView = PhotonView.Get(this);
		
		HideNotMine();
	}

	private void HideNotMine()
	{
		if (!photonView.isMine || photonView.isSceneView)
		{ uiButtons.SetActive(false); }

		if (photonView.isSceneView)
		{
			PlayerName.alpha = 0;
		}
	}

	public void OnPreviousPressed()
	{
		photonView.RPC("DisplayPreviousCharacter", PhotonTargets.All);
	}

	public void OnNextPressed()
	{
		photonView.RPC("DisplayNextCharacter", PhotonTargets.All);
	}
}
