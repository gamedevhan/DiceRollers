using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public static TurnManager Instance = null;
	private List<int> playerIDs = new List<int>();
	private PhotonView photonView;
	
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

		photonView = PhotonView.Get(this);
	}

	private void Start()
	{
		playerIDs.Add(PhotonNetwork.player.ID); // Register localPlayer
		photonView.RPC("SendLoadedPlayerID", PhotonTargets.Others, PhotonNetwork.player.ID); // Request loadedplayerlist to others
	}

	[PunRPC]
	private void SendLoadedPlayerID(int senderID)
	{	
		int[] playerIDInfo = new int[playerIDs.Count];

		for (int i = 0; i < playerIDInfo.Length; i++)
		{
			playerIDInfo[i] = playerIDs[i];
		}

		photonView.RPC("SyncLoadedPlayerIDs", PhotonPlayer.Find(senderID), playerIDInfo);

		for (int i = 0; i < playerIDs.Count; i++)
		{
			if (playerIDs[i] == senderID)
			{
				break;
			}
			else
			{
				playerIDs.Add(senderID);
			}			
		}

		CheckIfEveryoneHasLoaded();
	}

	[PunRPC]
	private void SyncLoadedPlayerIDs(int[] playerIDInfo)
	{
		for (int i = 0; i < playerIDInfo.Length; i++)
		{
			if (!HasRegistered(playerIDInfo[i]))
			{
				playerIDs.Add(playerIDInfo[i]);
			}
		}

		CheckIfEveryoneHasLoaded();
	}

	private bool HasRegistered(int playerID)
	{
		for (int i = 0; i < playerIDs.Count; i++)
		{
			if (playerIDs[i] == playerID)
			{				
				return true;
			}
		}

		return false;
	}

	private void CheckIfEveryoneHasLoaded()
	{
		if (!PhotonNetwork.isMasterClient)
			return;		
		
		if (playerIDs.Count == PhotonNetwork.playerList.Length)
		{
			Debug.Log("Everyone is loaded!");
			ShufflePlayerIDs();
			// Send RPC to clients to sync
			// Start game
		}
	}

	private void ShufflePlayerIDs()
	{
		
	}
}
