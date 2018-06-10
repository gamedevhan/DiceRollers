using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : Photon.PunBehaviour
{
	public Transform RoomListGrid;
		
	public GameObject lobbyRoomPrefab;

	private List<RoomListing> localLobbyRoomList = new List<RoomListing>();
	
	public static LobbyManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		Refresh();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F3))
		{
			GameObject lobbyRoomGO = Instantiate(lobbyRoomPrefab);
			Debug.Log("Instantiated prefab");
			lobbyRoomGO.transform.SetParent(RoomListGrid.transform, false);
			Debug.Log("Instantiated GO's parent is: " + lobbyRoomGO.transform.parent.name);
		}
	}

	#region UI Buttons

	public void OnQuickGamePressed()
	{
		// Join random room. If fails, NetworkManager will create room. OnPhotonRandomJoinFailed()
		PhotonNetwork.JoinRandomRoom();
	}

	public void OnCreatePressed()
	{	
		// TODO: Open a Panel that lets users to type room name
		// Create a room.
		RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = NetworkManager.Instance.maxPlayersPerRoom = 4 };

		PhotonNetwork.CreateRoom(PhotonNetwork.playerName + "'s Game", roomOptions, TypedLobby.Default);
	}
	
	public void OnRefreshPressed()
	{	
		Refresh();
	}

	public void OnSettingsPressed()
	{
		// TODO: Open Settings Panel
	}

	public void OnQuitPressed()
	{	
		Application.Quit();
	}
	
	public void JoinRoom(string roomName)
	{
		PhotonNetwork.JoinRoom(roomName);
	}

	#endregion

	#region Photon CallBacks

	public override void OnCreatedRoom()
	{
		Debug.Log("Room created succesfully");		
	}

	public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
	{
		Debug.Log("Create room failed due to " + codeAndMsg[1]);
	}

	public override void OnJoinedRoom()
	{
		if (!PhotonNetwork.isMasterClient)
		{
			Debug.Log("Trying to load a level but we are not masterclient");
		}
		PhotonNetwork.LoadLevel("02 Room");
	}

	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		Debug.Log("No room available, create new one");
		
		// Create a room
		PhotonNetwork.CreateRoom(PhotonNetwork.playerName + "'s Game", new RoomOptions { MaxPlayers = NetworkManager.Instance.maxPlayersPerRoom }, TypedLobby.Default);
	}

	public override void OnReceivedRoomListUpdate()
	{
		Debug.Log("RoomList Updated");

		Refresh();
	}

	#endregion

	private void Refresh()
	{
		// Get Photon Room List and check if room locally exist, if not create new one		
		RoomInfo[] rooms = PhotonNetwork.GetRoomList();
	
		foreach (RoomInfo rooom in rooms)
		{
			RoomReceived(rooom);
			
		}

		RemoveOldRooms();
		RoomListGrid.GetComponent<UIGrid>().Reposition();
	}

	private void RoomReceived(RoomInfo room)
	{		
		// Check if localLobbyRoomList has matching room in Photon RoomList
		int index = localLobbyRoomList.FindIndex(x => x.RoomName == room.Name);

		// index == -1, localLobbyRoomList doesn't have same name of room existing in Photon RoomList.
		// Instantiate lobbyRoom gameobject from prefab, and add to localLobbyRommList.
		if (index == -1)
		{	
			// Instantiate gameobject if room is visible and not full
			if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
			{
				GameObject lobbyRoomGO = Instantiate(lobbyRoomPrefab);
				lobbyRoomGO.transform.SetParent(RoomListGrid, false);				
				
				// Add lobbyRoom to local roomlist
				RoomListing lobbyRoom = lobbyRoomGO.GetComponent<RoomListing>();
				localLobbyRoomList.Add(lobbyRoom);

				index = (localLobbyRoomList.Count - 1);
			}
		}

		// index != -1, localLobbyRoomList has a room that matches the name of room in Photon RoomList.
		if (index != -1)		
		{
			RoomListing lobbyRoom = localLobbyRoomList[index];
			lobbyRoom.SetRoomNameText(room.Name);
			lobbyRoom.Updated = true;
		}
	}

	private void RemoveOldRooms() 
	{
		List<RoomListing> roomsToRemove = new List<RoomListing>();

		foreach (RoomListing lobbyRoom in localLobbyRoomList)
		{
			if (!lobbyRoom.Updated) { roomsToRemove.Add(lobbyRoom);	}
			else { lobbyRoom.Updated = false; }
		}

		foreach (RoomListing lobbyRoom in roomsToRemove)
		{
			GameObject lobbyRoomGO = lobbyRoom.gameObject;
			localLobbyRoomList.Remove(lobbyRoom);
			Destroy(lobbyRoomGO);
		}
	}
}