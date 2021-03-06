﻿using System;
using UnityEngine;

public class RoomPlayer : MonoBehaviour
{		
	public bool IsReady { get; set; }
	public string PlayerName { get; private set; }
		
	public PhotonPlayer PhotonPlayer { get; private set; }
	public PhotonView PhotonView { get; private set; }
		
	public static event Action PlayerIsReady = delegate { };

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
		RoomPlayerSpawnManager.NewPlayerJoin += OnNewPlayerJoined;
		LevelTransitionManager.Instance.roomPlayers.Add(this);
	}

	private void OnDisable()
	{
		RoomPlayerSpawnManager.NewPlayerJoin -= OnNewPlayerJoined;
		LevelTransitionManager.Instance.roomPlayers.Remove(this);
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
	
	private void OnNewPlayerJoined()
	{		
		// Send RPC to newplayer to sync local player's ready status
		PhotonView.RPC("SyncReadyStatus", PhotonTargets.Others, IsReady);
	}

	[PunRPC]
	private void SyncReadyStatus(bool isReady)
	{
		IsReady = isReady;
		PlayerIsReady();
	}
}
