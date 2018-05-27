using UnityEngine;

public class LobbyRoom : MonoBehaviour
{	
	private UILabel RoomNameLabel;
	public string RoomName;

	public bool Updated;
	
	public void OnClicked()
	{		
		RoomNameLabel = GetComponentInChildren<UILabel>();
		LobbyManager.Instance.JoinRoom(RoomNameLabel.text);		
	}

	public void SetRoomNameText(string text)
	{	
		RoomName = text;
		RoomNameLabel.text = RoomName;
	}
}
