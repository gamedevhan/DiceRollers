using UnityEngine;

public class Launcher : MonoBehaviour
{	
	public UILabel NameInputField;
	
	public delegate void ButtonEvent();

	[SerializeField] private GameObject ConnectPanel;
	[SerializeField] private GameObject ConnectingPanel;
	[SerializeField] private GameObject DisconnectPanel;

	public static string playerNamePrefKey = "PlayerName";
	
	private void Start()
	{
		string playerName = "";
		if (NameInputField != null)
		{
			if (PlayerPrefs.HasKey(playerNamePrefKey))
			{
				playerName = PlayerPrefs.GetString(playerNamePrefKey);
				NameInputField.text = playerName;
			}
		}
	}
		
	public void OnConnectPressed()
	{
		SetPlayerName();
		ConnectPanel.SetActive(false);
		ConnectingPanel.SetActive(true);
		NetworkManager.Instance.ConnectToMaster();
	}

	public void OnReconnectPressed()
	{
		ConnectingPanel.SetActive(true);
		DisconnectPanel.SetActive(false);		
	}

	public void Disconnected()
	{
		ConnectingPanel.SetActive(false);
		ConnectingPanel.SetActive(false);
		DisconnectPanel.SetActive(true);
	}

	private void SetPlayerName()
	{			
		PlayerPrefs.SetString(playerNamePrefKey, NameInputField.text);
	}
}
