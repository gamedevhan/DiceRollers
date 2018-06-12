using UnityEngine;

public class RoomPlayerUI : MonoBehaviour
{	
	[SerializeField]
	private GameObject uiButtons;

	[SerializeField]
	private UILabel nameLabel;
	
	public UISprite ReadyIcon;

	public string PlayerName
	{
		get
		{
			return nameLabel.text;
		}
		set
		{
			nameLabel.text = value;
		}
	}

	public PhotonView RoomPlayerView { get; private set; }
	
	#region Unity CallBacks

	private void Awake()
	{			
		RoomPlayerView = PhotonView.Get(this);		
	}

	private void Start()
	{	
		// Display only local player's character selection buttons
		if (RoomPlayerView.isMine) { uiButtons.SetActive(true); }
		else { uiButtons.SetActive(false); }
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
