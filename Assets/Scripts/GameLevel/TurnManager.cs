using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public static TurnManager Instance = null;
	private List<int> playerIDs = new List<int>();
	private PhotonView photonView;
	
	public int CurrentTurnPlayerID { get; private set; }
	private RaiseEventOptions turnBeginEventOptions = new RaiseEventOptions { CachingOption = EventCaching.DoNotCache, Receivers = ReceiverGroup.All };

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

		photonView = PhotonView.Get(this);
	}

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnPlayerLoad;
		PhotonNetwork.OnEventCall += OnTurnEnd;
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnPlayerLoad;
		PhotonNetwork.OnEventCall -= OnTurnEnd;
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
				StartMatch();
			}
		}
	}
	
	private void OnTurnEnd(byte eventcode, object content, int senderid)
	{
		if (eventcode != PhotonEventCode.TurnEnd)
			return;
		
		int currentTurnPlayerIndex = playerIDs.IndexOf(CurrentTurnPlayerID);		
		CurrentTurnPlayerID = ( currentTurnPlayerIndex == playerIDs.Count - 1 ) ? playerIDs[0] : playerIDs[currentTurnPlayerIndex + 1];

		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.RaiseEvent(PhotonEventCode.TurnBegin, CurrentTurnPlayerID, true, turnBeginEventOptions);
		}
	}

	private void StartMatch()
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
		PhotonNetwork.RaiseEvent(PhotonEventCode.TurnBegin, CurrentTurnPlayerID, true, turnBeginEventOptions);
	}
		
	// Shuffle playerIDs using Fisher-Yates algorithm. Problem: When there are only 2 players, masterclient will always be the second.
	private void RandomizeTurnOrder()
	{
		for (int i = playerIDs.Count - 1; i > 0; i--)
		{
			int random = Random.Range(0, i);
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
