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
		PhotonNetwork.OnEventCall += OnStartCountDown;
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnStartCountDown;
	}

	public void OnReadyPressed()
	{	
		// Publish local Event
		ReadyPressed();

		// Publish PhotonEvent
		PhotonNetwork.RaiseEvent((byte)EventCodes.ReadyPress, null, true, null);	
	}
		
	public void OnLeavePressed()
	{			
		LevelTransitionManager.Instance.DestroyGameObject();
		PhotonNetwork.LeaveRoom(false);
		PhotonNetwork.JoinLobby();
		SceneManager.LoadScene("01 Lobby");
	}

	private void OnStartCountDown(byte eventcode, object content, int senderid)
	{
		if (eventcode != (byte)EventCodes.CountDownStart)
			return;

		roomUIButtons.SetActive(false);
	}
}