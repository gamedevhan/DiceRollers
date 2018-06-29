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

	private void Start()
	{
		RollButton.SetActive(false);
	}

	public void OnRollButtonPressed()
	{		
		RollButton.SetActive(false);
		RollButtonPress();
	}

	private void OnTurnBegin(byte eventcode, object currentTurnPlayerID, int senderid)
	{
		if (eventcode != PhotonEventCode.TurnBegin)
			return;

		Debug.Log("Turn began! CurrentTurn player's ID is: " + currentTurnPlayerID);

		if (PhotonNetwork.player.ID != (int)currentTurnPlayerID) // Not local player's turn
		{
			RollButton.SetActive(false);
		}
		else if (!RollButton.activeInHierarchy) // local player's turn
		{
			RollButton.SetActive(true);
		}
	}
}
