using UnityEngine;

public class RoomPlayerUI : MonoBehaviour
{		
	public UILabel PlayerName;
	public UISprite ReadySprite;
	public bool IsReady = false;
	
	[SerializeField]
	private GameObject uiButtons;

	private PhotonView photonView;

	#region Unity CallBacks

	private void Awake()
	{	
		photonView = PhotonView.Get(this);		
	}

	#endregion

	#region Photon CallBacks

	public void OnOwnershipTransfered(object[] viewAndPlayers)
	{
		DisplayButtons();
	}

	#endregion

	#region UIButtons
	public void OnPreviousPressed()
	{
		photonView.RPC("DisplayPreviousCharacter", PhotonTargets.All);
	}

	public void OnNextPressed()
	{
		photonView.RPC("DisplayNextCharacter", PhotonTargets.All);
	}

	#endregion
	
	public void DisplayButtons()
	{
		if (photonView.isMine) { uiButtons.SetActive(true); }
	}
}
