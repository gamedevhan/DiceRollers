using System;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
	Ai,
	UnityChan,
	Riko,
	Yuji
};

public class LevelTransitionManager : MonoBehaviour
{	
	public Character SelectedCharacter; // For local player, Issue: if other user ready first and masterclient ready second to start, masterclient's selectedchar set to others
	public List<RoomPlayer> roomPlayers = new List<RoomPlayer>();
	
	[SerializeField]
	private Transform roomManager;

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
		RoomPlayer.PlayerIsReady += OnPlayerReady;
		CountDownTimer.CountDownFinish += OnCountDownFinish;
	}

	private void OnDisable()
	{	
		RoomPlayer.PlayerIsReady -= OnPlayerReady;
		CountDownTimer.CountDownFinish -= OnCountDownFinish;
	}

	private void OnPlayerReady()
	{
		if (roomPlayers.Count > 1 && CheckIfAllReady())
		{
			Debug.Log("Masterclient will load level");
			
			PhotonView roomManagerView = PhotonView.Get(roomManager);

			// Start CountDown, when count down finish master client load level
			roomManagerView.RPC("StartCountDown", PhotonTargets.All);

			// Disable buttons
			roomManagerView.RPC("SetActive", PhotonTargets.All, false);

			if (PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.room.IsOpen = false;
				PhotonNetwork.room.IsVisible = false;
			}
		}
	}

	private void OnCountDownFinish()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.LoadLevel("03 TestLevel");
		}
	}

	public void DestroyGameObject()
	{		
		Destroy(gameObject);
	}

	private bool CheckIfAllReady()
	{		
		foreach (var player in roomPlayers)
		{
			if (!player.IsReady)
			{			
				return false;	
			}
		}

		return true;
	}
}
