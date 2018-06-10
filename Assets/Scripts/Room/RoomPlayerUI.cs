using UnityEngine;

public class RoomPlayerUI : MonoBehaviour
{		
	public UILabel PlayerName;
	public UISprite ReadyIcon;
	
	[SerializeField]
	private GameObject uiButtons;

	public PhotonView RoomPlayerView { get; private set; }
	
	#region Unity CallBacks

	private void Awake()
	{			
		RoomPlayerView = PhotonView.Get(this);
		if (RoomPlayerView.isMine) { uiButtons.SetActive(true); }
	}

	#endregion

	#region UIButtons

	public void OnPreviousPressed()
	{
		RoomPlayerView.RPC("DisplayPreviousCharacter", PhotonTargets.All);
	}

	public void OnNextPressed()
	{
		RoomPlayerView.RPC("DisplayNextCharacter", PhotonTargets.All);
	}

	#endregion
}
