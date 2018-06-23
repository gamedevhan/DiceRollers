using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomUIManager : MonoBehaviour
{
	[SerializeField]
	private GameObject roomUIButtons;

	public delegate void ReadyPressedEvent();
	public static event ReadyPressedEvent ReadyPressed;
	
	private void OnEnable()
	{
		LevelTransitionManager.GameStart += OnGameStart;
	}

	private void OnDisable()
	{
		LevelTransitionManager.GameStart -= OnGameStart;
	}

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

	private void OnGameStart()
	{
		roomUIButtons.SetActive(false);
	}
}