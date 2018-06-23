using UnityEngine;

public class RoomPlayer : MonoBehaviour
{		
	public bool IsReady { get; set; }
	public string PlayerName { get; private set; }
		
	public PhotonPlayer PhotonPlayer { get; private set; }
	public PhotonView PhotonView { get; private set; }
		
	private RoomPlayerUI roomPlayerUI;

	#region Unity Methods

	private void Awake()
	{
		PhotonView = PhotonView.Get(this);
		roomPlayerUI = GetComponent<RoomPlayerUI>();
	}

	private void Start()
	{		
		if (PhotonView.isMine)
		{
			IsReady = false;
		}
	}

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnNewPlayerJoined;		
		LevelTransitionManager.Instance.roomPlayerIDs.Add(PhotonPlayer.ID);
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnNewPlayerJoined;		
		LevelTransitionManager.Instance.roomPlayerIDs.Remove(PhotonPlayer.ID);
	}
	
	#endregion

	public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
	{
		PhotonPlayer = photonPlayer;
		PlayerName = PhotonPlayer.NickName;
		roomPlayerUI.PlayerName = PlayerName;

		// RPC method in RoomPlayer gets called
		PhotonView.RPC("DisplayPlayerName", PhotonTargets.All, PlayerName);
	}
	
	private void OnNewPlayerJoined(byte eventcode, object content, int senderid)
	{
		if (eventcode != PhotonEventList.NewPlayerJoin)
			return;
		
		// Send RPC to newplayer to sync local player's ready status
		PhotonView.RPC("SyncReadyStatus", PhotonPlayer.Find(senderid), IsReady);
	}

	[PunRPC]
	private void SyncReadyStatus(bool isReady)
	{
		IsReady = isReady;
	}
}
