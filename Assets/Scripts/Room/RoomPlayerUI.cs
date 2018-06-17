using UnityEngine;

public class RoomPlayerUI : MonoBehaviour
{
	public UISprite ReadyIcon;

	[SerializeField]
	private GameObject uiButtons;

	[SerializeField]
	private UILabel nameLabel;

	private RoomPlayer roomPlayer;	

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

	public PhotonView PhotonView { get; private set; }

	#region Unity Methods

	private void Awake()
	{
		PhotonView = PhotonView.Get(this);
		roomPlayer = GetComponent<RoomPlayer>();		
	}

	private void Start()
	{
		// Display only local player's character selection buttons
		if (PhotonView.isMine) { uiButtons.SetActive(true); }
		else { uiButtons.SetActive(false); }
	}

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnNewPlayerJoined;
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnNewPlayerJoined;
	}

	#endregion

	#region UIButtons

	public void OnPreviousPressed()
	{
		PhotonView.RPC("DisplayPreviousCharacter", PhotonTargets.All);
	}

	public void OnNextPressed()
	{
		PhotonView.RPC("DisplayNextCharacter", PhotonTargets.All);
	}

	#endregion

	private void OnNewPlayerJoined(byte eventcode, object content, int senderid)
	{
		// Send RPC to newplayer to sync local player's name UI text
		PhotonView.RPC("SyncReadyIcon", PhotonPlayer.Find(senderid), roomPlayer.IsReady);
		PhotonView.RPC("DisplayPlayerName", PhotonPlayer.Find(senderid), PlayerName);
	}

	private void OnReadyButtonPressed()
	{
		PhotonView.RPC("SyncReadyIcon", PhotonTargets.Others, roomPlayer.IsReady);
	}

	[PunRPC]
	public void DisplayPlayerName(string playerName)
	{
		PlayerName = playerName;
	}

	[PunRPC]
	private void SyncReadyIcon(bool isReady)
	{
		ReadyIcon.alpha = isReady ? 255 : 0;
	}
}
