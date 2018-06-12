using UnityEngine;

public class RoomManager : Photon.PunBehaviour
{	
	[System.Serializable]
	public class SpawnPoint
	{		
		public bool IsOccupied;
		public Vector3 Position;
	}
	
	public SpawnPoint[] spawnPoints;

	[SerializeField]
	private GameObject roomPlayerPrefab;	
	
	public static RoomManager Instance { get; private set; }	
	
	#region Unity CallBacks

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		SpawnRoomPlayer();
		// SyncCurrentCharacter - what are other players' current character?
		// SyncReadyStatus - what are other players' ready status?
		// SyncName - what are other players' name?		
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
		}
		else // If not masterclient, send RPC to MasterClient to sync spawnPoints then Instantiate
		{
			photonView.RPC("SyncSpawnPointsThenSpawn", PhotonTargets.MasterClient, photonView.ownerId);
		}
	}

	[PunRPC]
	private void SyncSpawnPointsThenSpawn(int id)
	{
		int spawnPointsCount = spawnPoints.Length;

		// Find first empty spawnPoint, this will be where the client spawn
		int indexToSpawn = new int();
		
		for (int i = 0; i < spawnPointsCount; i++)
		{
			if (!spawnPoints[i].IsOccupied)
			{			
				indexToSpawn = i;
				spawnPoints[i].IsOccupied = true;
				break;
			}
		}

		//// Sync spawnPoints. True if occupied, false if empty
		bool[] isOccupieds = new bool[spawnPointsCount];
		
		for (int i = 0; i < spawnPointsCount; i++)
		{			
			if (spawnPoints[i].IsOccupied)
			{
				isOccupieds[i] = true;
			}

			if (!spawnPoints[i].IsOccupied)
			{
				isOccupieds[i] = false;
			}
		}

		// Send RPC back to the client to sync
		photonView.RPC("OnSpawnPointsInfoReceived", PhotonPlayer.Find(id), indexToSpawn, isOccupieds);
	}

	[PunRPC]
	private void OnSpawnPointsInfoReceived(int indexToSpawn, bool[] isOccupieds)
	{
		PhotonNetwork.Instantiate(roomPlayerPrefab.name, spawnPoints[indexToSpawn].Position, Quaternion.identity, 0);
		for (int i = 0; i < spawnPoints.Length; i++)
		{	
			spawnPoints[i].IsOccupied = isOccupieds[i];
		}
	}
}