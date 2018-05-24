using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchManager : Photon.PunBehaviour
{	
	public UIManager UIManager;

	private bool isConnecting;
	private string gameVersion = "0.1.0";

	private void Awake()
	{
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.automaticallySyncScene = true;
	}

	public void Connect()
	{
		isConnecting = true;

		if (PhotonNetwork.connected) { PhotonNetwork.JoinLobby(); }
		else { PhotonNetwork.ConnectUsingSettings(gameVersion); }
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Region: " + PhotonNetwork.networkingPeer.CloudRegion);
		if (isConnecting)
		{
			Debug.Log("Joining Lobby");
			PhotonNetwork.JoinLobby();
		}
	}
	
	public override void OnDisconnectedFromPhoton()
	{
		isConnecting = false;
		UIManager.Disconnected();
	}

	public override void OnJoinedLobby()
	{		
		PhotonNetwork.LoadLevel("01 Lobby");
	}
}
