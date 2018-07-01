﻿using System;
using UnityEngine;

public class GamePlayerSpawnManager : MonoBehaviour
{
	[SerializeField]
	private Transform startTile;
	
	// Use this for initialization
	void Start()
	{
		SpawnGamePlayer();
	}

	private void SpawnGamePlayer()
	{
		string characterName = LevelTransitionManager.Instance.SelectedCharacter.ToString();
		GameObject playerCharacter = PhotonNetwork.Instantiate(characterName, startTile.position, Quaternion.identity, 0);
		PhotonView characterPhotonView = PhotonView.Get(playerCharacter);

		RaiseEventOptions eventOptions = new RaiseEventOptions { CachingOption = EventCaching.AddToRoomCache, Receivers = ReceiverGroup.All };		
		PhotonNetwork.RaiseEvent(PhotonEventCode.PlayerLoaded, characterPhotonView.viewID, true, eventOptions);
	}
}
