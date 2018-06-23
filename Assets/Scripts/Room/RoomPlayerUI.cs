using UnityEngine;

public class RoomPlayerUI : MonoBehaviour
{
	public UISprite ReadyIcon;

	[SerializeField]
	private GameObject characterScrollButtons;

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
		if (PhotonView.isMine) { characterScrollButtons.SetActive(true); }
		else { characterScrollButtons.SetActive(false); }
	}

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnNewPlayerJoined;
		RoomButtonManager.ReadyPressed += OnReadyButtonPressed;
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnNewPlayerJoined;
		RoomButtonManager.ReadyPressed -= OnReadyButtonPressed;
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
		if (eventcode != PhotonEventList.NewPlayerJoin)
			return;

		// Send RPC to newplayer to sync local player's name UI text
		PhotonView.RPC("SyncReadyIcon", PhotonPlayer.Find(senderid), roomPlayer.IsReady);
		PhotonView.RPC("DisplayPlayerName", PhotonPlayer.Find(senderid), PlayerName);
	}
		
	private void OnReadyButtonPressed()
	{
		if (!PhotonView.isMine)	{ return; }

		roomPlayer.IsReady = !roomPlayer.IsReady;

		if (roomPlayer.IsReady)
		{
			characterScrollButtons.SetActive(false);
		}
		else
		{
			characterScrollButtons.SetActive(true);
		}
		
		PhotonView.RPC("SyncReadyStatus", PhotonTargets.Others, roomPlayer.IsReady);
		PhotonView.RPC("SyncReadyIcon", PhotonTargets.All, roomPlayer.IsReady);

		// master client has to be the last one pressing ready. Publish PhotonEvent that only masterclient listens.
		PhotonNetwork.RaiseEvent(PhotonEventList.ReadyPress, null, true, null);		
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
