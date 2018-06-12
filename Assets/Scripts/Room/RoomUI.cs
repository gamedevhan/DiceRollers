using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomUI : MonoBehaviour
{
	public void OnReadyPressed()
	{	
		// Get reference of roomPlayer game object owned by local player
		//GameObject roomPlayerGameObject = RoomManager.Instance.MyGameObject;

		// Send RPC to notify local player is ready
		//PhotonView roomPlayerView = PhotonView.Get(roomPlayerGameObject);
		//roomPlayerView.RPC("ToggleReady", PhotonTargets.All);		
	}
		
	public void OnLeavePressed()
	{	
		//RoomManager.Instance.IsOccupied[RoomManager.Instance.MySpawnPointIndex] = true;
		PhotonNetwork.LeaveRoom(false);
		PhotonNetwork.JoinLobby();
		SceneManager.LoadScene("01 Lobby");
	}
}