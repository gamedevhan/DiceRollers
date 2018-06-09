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
		if (photonView.isMine) { uiButtons.SetActive(true); }
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
}
