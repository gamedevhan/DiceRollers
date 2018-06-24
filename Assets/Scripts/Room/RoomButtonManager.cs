using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomButtonManager : MonoBehaviour
{
	[SerializeField]
	private GameObject roomUIButtons;

	public delegate void ReadyPressedEvent();
	public static event ReadyPressedEvent ReadyPressed;
	
	private void OnEnable()
	{
		LevelTransitionManager.StartCountdown += OnStartCountDown;
	}

	private void OnDisable()
	{
		LevelTransitionManager.StartCountdown -= OnStartCountDown;
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

	private void OnStartCountDown()
	{
		roomUIButtons.SetActive(false);
	}
}