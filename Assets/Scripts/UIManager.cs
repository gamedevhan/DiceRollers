using UnityEngine;


public class UIManager : MonoBehaviour
{
	public UILabel NameInputField;
	
	public GameObject ConnectPanel;
	public GameObject ConnectingPanel;
	public GameObject DisConnectPanel;

	public void ConnectButton()
	{
		ConnectPanel.SetActive(false);
		ConnectingPanel.SetActive(true);
	}

	public void ReConnectButton()
	{
		ConnectingPanel.SetActive(true);
		DisConnectPanel.SetActive(false);

	}
}
