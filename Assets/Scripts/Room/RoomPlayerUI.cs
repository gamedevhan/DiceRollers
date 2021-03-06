﻿using UnityEngine;

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
		RoomPlayerSpawnManager.NewPlayerJoin += OnNewPlayerJoined;
		RoomButtonManager.ReadyPressed += OnReadyButtonPressed;
	}

	private void OnDisable()
	{
		RoomPlayerSpawnManager.NewPlayerJoin -= OnNewPlayerJoined;
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

	private void OnNewPlayerJoined()
	{
		// Send RPC to newplayer to sync local player's name UI text
		PhotonView.RPC("SyncReadyIcon", PhotonTargets.Others, roomPlayer.IsReady);
		PhotonView.RPC("DisplayPlayerName", PhotonTargets.Others, PlayerName);
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
