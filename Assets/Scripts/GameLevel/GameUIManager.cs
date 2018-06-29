using System;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
	[SerializeField]
	private GameObject RollButton;

	public static event Action RollButtonPress = delegate{ };
	public static GameUIManager Instance = null;	

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;			
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnTurnBegin;
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnTurnBegin;
	}

	public void Roll()
	{		
		RollButton.SetActive(false);
		RollButtonPress();
	}

	private void OnTurnBegin(byte eventcode, object content, int senderid)
	{
		if (eventcode != PhotonEventCode.TurnBegin)
			return;

		if ((int)content != PhotonNetwork.player.ID && RollButton.activeInHierarchy)
		{
			RollButton.SetActive(false);
		}
		else if (!RollButton.activeInHierarchy)
		{
			RollButton.SetActive(true);
		}
	}
}
