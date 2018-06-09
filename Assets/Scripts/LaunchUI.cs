using UnityEngine;

public class LaunchUI : MonoBehaviour
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
		
	public void ConnectButton()
	{
		ConnectPanel.SetActive(false);
		ConnectingPanel.SetActive(true);
		NetworkManager.Instance.ConnectToMaster();
	}

	public void ReConnectButton()
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

	public void SetPlayerName()
	{			
		PlayerPrefs.SetString(playerNamePrefKey, NameInputField.text);
	}
}
