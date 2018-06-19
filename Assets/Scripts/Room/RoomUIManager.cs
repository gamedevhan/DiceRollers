using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomUIManager : MonoBehaviour
{
	public delegate void ReadyPressedEvent();
	public static event ReadyPressedEvent ReadyPressed;

	public void OnReadyPressed()
	{	
		ReadyPressed();
	}
		
	public void OnLeavePressed()
	{	
		LevelTransitionManager.Instance.DestroyGameObject();
		PhotonNetwork.LeaveRoom(false);
		PhotonNetwork.JoinLobby();
		SceneManager.LoadScene("01 Lobby");
	}
}