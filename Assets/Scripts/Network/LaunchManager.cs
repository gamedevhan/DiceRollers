using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchManager : Photon.PunBehaviour
{
	public byte MaxPlayersPerRoom = 4;
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

		if (PhotonNetwork.connected) { PhotonNetwork.JoinRandomRoom(); }
		else { PhotonNetwork.ConnectUsingSettings(gameVersion); }
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Region: " + PhotonNetwork.networkingPeer.CloudRegion);
		if (isConnecting)
		{
			Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
			PhotonNetwork.JoinRandomRoom();
		}
	}

	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

		PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
	}

	public override void OnDisconnectedFromPhoton()
	{
		isConnecting = false;
		UIManager.Disconnected();
	}

	public override void OnJoinedRoom()
	{
		PhotonNetwork.LoadLevel("01 Prep");
	}
}
