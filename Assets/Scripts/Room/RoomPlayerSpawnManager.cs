using UnityEngine;

public class RoomPlayerSpawnManager : Photon.PunBehaviour
{	
	[System.Serializable]
	public class SpawnPoint
	{	
		public Vector3 Position;
		public bool IsOccupied = false;
		public int PlayerID = -1;
	}
	
	public SpawnPoint[] spawnPoints;

	[SerializeField]
	private GameObject roomPlayerPrefab;
	
	public static RoomPlayerSpawnManager Instance { get; private set; }	
	
	#region Unity CallBacks

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		SpawnRoomPlayer();
	}

	#endregion

	#region Photon CallBacks

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			if (spawnPoints[i].PlayerID == otherPlayer.ID)
			{
				spawnPoints[i].IsOccupied = false;
				spawnPoints[i].PlayerID = -1;
			}
		}
	}
	
	#endregion

	private void SpawnRoomPlayer()
	{
		// If masterClient, Spawn on spawnPoints[0]
		if (PhotonNetwork.isMasterClient)
		{			
			// Instantiate on first spawn point
			GameObject roomPlayerGameObject = PhotonNetwork.Instantiate(roomPlayerPrefab.name, spawnPoints[0].Position, Quaternion.identity, 0);
			spawnPoints[0].IsOccupied = true;			
			spawnPoints[0].PlayerID = PhotonNetwork.player.ID;
			roomPlayerGameObject.GetComponent<RoomPlayer>().ApplyPhotonPlayer(PhotonNetwork.player);
		}
		else // If not masterclient, send RPC to MasterClient to sync spawnPoints then Instantiate
		{
			photonView.RPC("SendSpawnPointInfo", PhotonTargets.MasterClient, PhotonNetwork.player.ID);
		}
	}

	[PunRPC]
	private void SendSpawnPointInfo(int playerID)
	{
		int spawnPointsCount = spawnPoints.Length;
				
		// Find first empty spawnPoint
		int indexToSpawn = new int();
		int[] syncedViewIDs = new int[spawnPointsCount];
		int[] syncedPlayerIDs = new int[spawnPointsCount];
		bool[] syncedOccupiedInfo = new bool[spawnPointsCount];

		for (int i = 0; i < spawnPointsCount; i++)
		{
			if (!spawnPoints[i].IsOccupied)
			{			
				indexToSpawn = i;
				spawnPoints[i].IsOccupied = true;
				break;
			}
		}

		// Sync parameters to send RPC
		for (int i = 0; i < spawnPointsCount; i++)
		{	
			syncedOccupiedInfo[i] = spawnPoints[i].IsOccupied;			
			syncedPlayerIDs[i] = spawnPoints[i].PlayerID;
		}

		// Send RPC back to the client to sync
		photonView.RPC("OnSpawnPointsInfoReceived", PhotonPlayer.Find(playerID), indexToSpawn, syncedOccupiedInfo, syncedViewIDs, syncedPlayerIDs);
	}

	[PunRPC]
	private void OnSpawnPointsInfoReceived(int indexToSpawn, bool[] syncedEmptyInfo, int[] syncedViewIDs, int[] syncedPlayerIDs)
	{
		// Update local spawnPoints
		for (int i = 0; i < spawnPoints.Length; i++)
		{	
			spawnPoints[i].IsOccupied = syncedEmptyInfo[i];			
		}
		
		GameObject roomPlayerGameObject = PhotonNetwork.Instantiate(roomPlayerPrefab.name, spawnPoints[indexToSpawn].Position, Quaternion.identity, 0);
		int viewID = PhotonView.Get(roomPlayerGameObject).viewID;
		int playerID = PhotonNetwork.player.ID;
				
		roomPlayerGameObject.GetComponent<RoomPlayer>().ApplyPhotonPlayer(PhotonNetwork.player);
		
		// Raise Event so other players already in the room can send RPC to new player so their current character selected, player name, ready status synced
		PhotonNetwork.RaiseEvent(PhotonEventList.NewPlayerJoin, null, true, null);

		// Send RPC to other players to let spawnPoint[indexToSpawn] is occupied
		photonView.RPC("SyncSpawnPointInfo", PhotonTargets.All, indexToSpawn, viewID, playerID);
	}

	[PunRPC]
	private void SyncSpawnPointInfo(int spawnedPointIndex, int viewID, int playerID)
	{
		spawnPoints[spawnedPointIndex].IsOccupied = true;		
		spawnPoints[spawnedPointIndex].PlayerID = playerID;
	}
}