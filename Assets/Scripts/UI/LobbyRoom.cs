using UnityEngine;

public class LobbyRoom : MonoBehaviour
{	
	private UILabel RoomNameLabel;
	public string RoomName;

	public bool Updated;

	private void Awake()
	{
		RoomNameLabel = GetComponentInChildren<UILabel>();
	}

	public void OnClicked()
	{	
		LobbyManager.Instance.JoinRoom(RoomNameLabel.text);
	}

	public void SetRoomNameText(string text)
	{	
		RoomName = text;
		RoomNameLabel.text = RoomName;
	}
}
