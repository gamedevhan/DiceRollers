using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{	
	private List<int> playerIDs = new List<int>();
	private PhotonView photonView;
	
	public int CurrentTurnPlayerID { get; private set; }
	public static Action TurnBegan = delegate { };

	private void Awake()
	{
		photonView = GetComponent<PhotonView>();
	}

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnPlayerLoad;		
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnPlayerLoad;		
	}

	private void OnPlayerLoad(byte eventcode, object senderCharacterPhotonViewID, int senderid)
	{
		if (eventcode != PhotonEventCode.PlayerLoaded)
			return;

		playerIDs.Add(senderid);

		if (PhotonNetwork.isMasterClient)
		{
			// Check if all players are loaded
			if (playerIDs.Count == PhotonNetwork.playerList.Length)
			{				
				StartCoroutine(StartMatch());
			}
		}
	}

	public void TurnBegin()
	{		
		DebugUtility.Log(CurrentTurnPlayerID + "'s turn");
		photonView.RPC("RpcTurnBegin", PhotonTargets.All, CurrentTurnPlayerID);
	}

	public void TurnEnd()
	{
		photonView.RPC("RpcTurnEnd", PhotonTargets.All);
	}

	[PunRPC]
	private void RpcTurnBegin(int currentTurnPlayerID)
	{
		CurrentTurnPlayerID = currentTurnPlayerID;
		TurnBegan();
	}

	[PunRPC]
	private void RpcTurnEnd()
	{	
		int currentTurnPlayerIndex = playerIDs.IndexOf(CurrentTurnPlayerID);
		CurrentTurnPlayerID = ( currentTurnPlayerIndex == playerIDs.Count - 1 ) ? playerIDs[0] : playerIDs[currentTurnPlayerIndex + 1];

		if (PhotonNetwork.isMasterClient)
		{
			TurnBegin();
		}
	}

	private IEnumerator StartMatch()
	{
		// ShufflePlayerIDs for random turn order
		RandomizeTurnOrder();

		int[] shuffledPlayerIDs = new int[playerIDs.Count];

		for (int i = 0; i < playerIDs.Count; i++)
		{
			shuffledPlayerIDs[i] = playerIDs[i];
		}

		CurrentTurnPlayerID = playerIDs[0];

		// Send RPC to clients to sync shuffled playerIDs
		photonView.RPC("SyncPlayerIDs", PhotonTargets.Others, shuffledPlayerIDs);
				
		// TODO: Text -> Game Start!
		yield return new WaitForSeconds(1f);
		// TODO: Hide Text

		TurnBegin();
	}
		
	// Shuffle playerIDs using Fisher-Yates algorithm. Problem: When there are only 2 players, masterclient will always be the second.
	private void RandomizeTurnOrder()
	{
		for (int i = playerIDs.Count - 1; i > 0; i--)
		{
			int random = UnityEngine.Random.Range(0, i);
			int temp = playerIDs[i];
			playerIDs[i] = playerIDs[random];
			playerIDs[random] = temp;
		}
	}

	[PunRPC]
	private void SyncPlayerIDs(int[] shuffledIDs)
	{
		for (int i = 0; i < shuffledIDs.Length; i++)
		{
			playerIDs[i] = shuffledIDs[i];
		}
	}
}
