using UnityEngine;

public class LobbyUI : MonoBehaviour
{
	[SerializeField] private UIGrid roomListGrid;
	[SerializeField] private GameObject roomPrefab;

	private void Start()
	{
		RefreshRoomList();
	}

	public void JoinButton()
	{
		// TODO: Get room info and join
	}

	public void QuickGameButton()
	{
		// Join random room. If fails, NetworkManager will create room. OnPhotonRandomJoinFailed()
		PhotonNetwork.JoinRandomRoom();
	}

	public void CreateButton()
	{	
		// Create a room. 
		PhotonNetwork.CreateRoom(PhotonNetwork.playerName + "'s Game", new RoomOptions { MaxPlayers = NetworkManager.Instance.maxPlayersPerRoom }, TypedLobby.Default);
		
		// TODO: Send message to other players in the lobby to refresh room list
	}
	
	public void RefreshRoomList()
	{
		// Get room list from Photon and display
		RoomInfo[] roomInfo = PhotonNetwork.GetRoomList();
		foreach (RoomInfo room in roomInfo)
		{
			Instantiate(roomPrefab, roomListGrid.transform);
		}
	}

	public void SettingsButton()
	{
		// TODO: Open Settings Panel
	}

	public void QuitButton()
	{	
		Application.Quit();
	}
}
