using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : Photon.PunBehaviour
{
	public byte maxPlayersPerRoom = 4;
	public string Name { get; private set; }

	private bool isConnecting;
	private string gameVersion = "0.1.0";
	
	public static NetworkManager Instance = null;

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

	#region Public Methods

	public void ConnectToMaster()
	{
		isConnecting = true;

		if (PhotonNetwork.connected) { PhotonNetwork.JoinLobby(TypedLobby.Default); }
		else { PhotonNetwork.ConnectUsingSettings(gameVersion); }
	}
	
	#endregion

	#region Photon CallBacks

	public override void OnConnectedToMaster()
	{
		Debug.Log("Region: " + PhotonNetwork.networkingPeer.CloudRegion);

		PhotonNetwork.playerName = PlayerPrefs.GetString(Launcher.playerNamePrefKey);

		if (isConnecting)
		{
			Debug.Log("Joining Lobby");
			PhotonNetwork.JoinLobby();
		}
	}

	public override void OnDisconnectedFromPhoton()
	{
		Debug.Log("Disconnected from Photon, Loading 00 Menu Scene");
		isConnecting = false;		
		SceneManager.LoadScene("00 Menu");
	}

	public override void OnJoinedLobby()
	{		
		Debug.Log("Joined Lobby, Loading 01 Lobby Scene");
		SceneManager.LoadScene("01 Lobby");
	}

	#endregion
}