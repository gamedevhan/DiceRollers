using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public static TurnManager Instance = null;
	private List<int> playerIDs = new List<int>();
	private PhotonView photonView;
	
	public int CurrentPlayerTurnID { get; private set; }

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

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnPlayerLoad;
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnPlayerLoad;
	}

	private void OnPlayerLoad(byte eventcode, object content, int senderid)
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
		
	private void StartMatch()
	{
		// ShufflePlayerIDs for random turn order
		ShufflePlayerIDs();

		int[] shuffledPlayerIDs = new int[playerIDs.Count];

		for (int i = 0; i < playerIDs.Count; i++)
		{
			shuffledPlayerIDs[i] = playerIDs[i];
		}

		// Send RPC to clients to sync shuffled playerIDs
		photonView.RPC("SyncPlayerIDs", PhotonTargets.Others, shuffledPlayerIDs);
		RaiseEventOptions eventOptions = new RaiseEventOptions { CachingOption = EventCaching.DoNotCache, Receivers = ReceiverGroup.All };
		PhotonNetwork.RaiseEvent(PhotonEventCode.TurnBegin, null, true, eventOptions);		
	}
		
	private void ShufflePlayerIDs() // Shuffle using Fisher-Yates algorithm
	{
		for (int i = playerIDs.Count; i > 0; i--)
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
