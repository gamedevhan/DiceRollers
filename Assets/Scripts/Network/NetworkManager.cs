using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : Photon.PunBehaviour
{
	private bool isConnecting;
	private string gameVersion = "0.1.0";

	public static NetworkManager Instance = null;
	public string Name { get; private set; }

	private void Awake()
	{
		if (Instance == null) {	Instance = this; }
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.automaticallySyncScene = true;
	}
		
	public void OnConnectButtonPressed()
	{
		isConnecting = true;

		if (PhotonNetwork.connected) { PhotonNetwork.JoinLobby(); }
		else { PhotonNetwork.ConnectUsingSettings(gameVersion); }
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Region: " + PhotonNetwork.networkingPeer.CloudRegion);
		PhotonNetwork.playerName = PlayerPrefs.GetString(LaunchUI.playerNamePrefKey);
		if (isConnecting)
		{
			Debug.Log("Joining Lobby");
			PhotonNetwork.JoinLobby();
		}
	}

	public override void OnDisconnectedFromPhoton()
	{
		isConnecting = false;
		SceneManager.LoadScene("00 Menu");
	}

	public override void OnJoinedLobby()
	{		
		PhotonNetwork.LoadLevel("01 Lobby");
	}
}