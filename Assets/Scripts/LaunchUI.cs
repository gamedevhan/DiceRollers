using UnityEngine;


public class LaunchUI : MonoBehaviour
{
	public UILabel NameInputField;
	public LaunchManager LaunchManager;

	public GameObject ConnectPanel;
	public GameObject ConnectingPanel;
	public GameObject DisConnectPanel;

	private static string playerNamePrefKey = "PlayerName";

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
		LaunchManager.Connect();
	}

	public void ReConnectButton()
	{
		ConnectingPanel.SetActive(true);
		DisConnectPanel.SetActive(false);
		LaunchManager.Connect();
	}

	public void Disconnected()
	{
		ConnectingPanel.SetActive(false);
		ConnectingPanel.SetActive(false);
		DisConnectPanel.SetActive(true);		
	}

	public void SetPlayerName()
	{	
		PhotonNetwork.playerName = NameInputField.text;
		PlayerPrefs.SetString(playerNamePrefKey, NameInputField.text);
	}
}
