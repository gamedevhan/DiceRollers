using UnityEngine;

public class RoomPlayerSpawnManager : Photon.PunBehaviour
{	
	[System.Serializable]
	public class SpawnPoint
	{	
		public Vector3 Position;
		public bool IsOccupied = false;
		public int ViewID = 0;
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
				spawnPoints[i].ViewID = 0;
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
			spawnPoints[0].ViewID = PhotonView.Get(roomPlayerGameObject).viewID;
			spawnPoints[0].PlayerID = PhotonNetwork.player.ID;
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
			syncedViewIDs[i] = spawnPoints[i].ViewID;
		}

		// Send RPC back to the client to sync
		photonView.RPC("OnSpawnPointsInfoReceived", PhotonPlayer.Find(playerID), indexToSpawn, syncedOccupiedInfo, syncedViewIDs);
	}

	[PunRPC]
	private void OnSpawnPointsInfoReceived(int indexToSpawn, bool[] syncedEmptyInfo, int[] syncedViewIDs)
	{
		// Update local spawnPoints
		for (int i = 0; i < spawnPoints.Length; i++)
		{	
			spawnPoints[i].IsOccupied = syncedEmptyInfo[i];
			spawnPoints[i].ViewID = syncedViewIDs[i];
		}
		
		GameObject roomPlayerGameObject = PhotonNetwork.Instantiate(roomPlayerPrefab.name, spawnPoints[indexToSpawn].Position, Quaternion.identity, 0);
		int viewID = PhotonView.Get(roomPlayerGameObject).viewID;
		int playerID = PhotonNetwork.player.ID;

		photonView.RPC("OnInstantiatedRoomPlayer", PhotonTargets.All, indexToSpawn, viewID, playerID);
	}

	[PunRPC]
	private void OnInstantiatedRoomPlayer(int spawnedPointIndex, int viewID, int playerID)
	{
		spawnPoints[spawnedPointIndex].IsOccupied = true;
		spawnPoints[spawnedPointIndex].ViewID = viewID;
		spawnPoints[spawnedPointIndex].PlayerID = playerID;
	}
}