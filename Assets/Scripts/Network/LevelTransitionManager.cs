﻿using System.Collections.Generic;
using UnityEngine;

public enum Character
{
	None,
	Ai,
	UnityChan,
	Riko,
	Yuji
};

public class LevelTransitionManager : MonoBehaviour
{	
	public Character SelectedCharacter; // For local player
	public List<RoomPlayer> roomPlayers = new List<RoomPlayer>();

	public static LevelTransitionManager Instance = null;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;			
		}
		else
		{
			DestroyGameObject();
		}
	}

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnReadyPressed;
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnReadyPressed;
	}

	public void OnReadyPressed(byte eventcode, object content, int senderid)
	{
		if (eventcode != PhotonEventList.ReadyPress)
			return;

		if (roomPlayers.Count > 1 && CheckIfAllReady())
		{
			Debug.Log("Masterclient will load level");
			if (PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.LoadLevel("03 TestLevel");
			}
		}
	}

	public void DestroyGameObject()
	{		
		Destroy(gameObject);
	}

	private bool CheckIfAllReady()
	{
		// Check if everyone is ready
		foreach (var player in roomPlayers)
		{
			if (!player.IsReady)
			{
				Debug.Log("Someone is not ready");
				return false;	
			}
		}
		
		Debug.Log("Everyone is ready!");
		return true;
	}
}