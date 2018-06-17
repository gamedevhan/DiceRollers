using UnityEngine;

public class RoomPlayerSpawnManager : Photon.PunBehaviour
{	
	[System.Serializable]
	public class SpawnPoint
	{	
		public Vector3 Position;
		public bool IsOccupied = false;
		public int viewID = 0;
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
		if (PhotonNetwork.player != otherPlayer)
		{
			// TODO: Set leaver's isEmptySpawnPosition = true
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
			spawnPoints[0].viewID = PhotonView.Get(roomPlayerGameObject).viewID;
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

		// 
		for (int i = 0; i < spawnPointsCount; i++)
		{	
			syncedOccupiedInfo[i] = spawnPoints[i].IsOccupied;
			syncedViewIDs[i] = spawnPoints[i].viewID;
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
			spawnPoints[i].viewID = syncedViewIDs[i];
		}
		
		GameObject roomPlayerGameObject = PhotonNetwork.Instantiate(roomPlayerPrefab.name, spawnPoints[indexToSpawn].Position, Quaternion.identity, 0);
		int viewID = PhotonView.Get(roomPlayerGameObject).viewID;

		photonView.RPC("OnInstantiatedRoomPlayer", PhotonTargets.All, indexToSpawn, viewID);		
	}

	[PunRPC]
	private void OnInstantiatedRoomPlayer(int spawnedPointIndex, int viewID)
	{
		spawnPoints[spawnedPointIndex].IsOccupied = true;
		spawnPoints[spawnedPointIndex].viewID = viewID;
	}
}