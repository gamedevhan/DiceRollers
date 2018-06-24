using System;
using System.Collections.Generic;
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
	public List<RoomPlayer> roomPlayers = new List<RoomPlayer>(); // This is causing duplicates and many issues
	
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
