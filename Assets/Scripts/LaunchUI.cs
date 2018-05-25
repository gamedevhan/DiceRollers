using UnityEngine;


public class LaunchUI : MonoBehaviour
{
	public UILabel NameInputField;
	public LaunchManager LaunchManager;

	public GameObject ConnectPanel;
	public GameObject ConnectingPanel;
	public GameObject DisConnectPanel;

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
}
