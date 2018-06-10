using UnityEngine;

public class RoomManager : Photon.PunBehaviour
{	
	public bool[] IsEmptySpawnPoints = { true, true, true, true };

	[SerializeField]
	private GameObject roomPlayerPrefab;

	[SerializeField]
	private Vector3[] spawnPoints;
	
	public int MySpawnPointIndex { get; private set; }	
	public GameObject MyGameObject { get; private set; }
	public static RoomManager Instance { get; private set; }	
	
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
		// If masterClient, instantiate on spawnPoints[0], set IsEmptySpawnPoints[0] false
		if (PhotonNetwork.isMasterClient)
		{
			IsEmptySpawnPoints[0] = false;
			MyGameObject = PhotonNetwork.Instantiate(roomPlayerPrefab.name, spawnPoints[0], Quaternion.identity, 0);		
		}
		else // If not masterclient, send RPC to MasterClient to receive index to spawn and sync isEmptySpawnPoints
		{
			photonView.RPC("RequestEmptySpawnPointsInfo", PhotonTargets.MasterClient, photonView.ownerId);
		}
	}

	[PunRPC]
	private void RequestEmptySpawnPointsInfo(int id)
	{
		int SpawnPointsCount = IsEmptySpawnPoints.Length;

		// Find first isEmptyPoints = true and set it false		
		for (int i = 0; i < SpawnPointsCount; i++)
		{
			if (IsEmptySpawnPoints[i])
			{
				IsEmptySpawnPoints[i] = false;
				MySpawnPointIndex = i;
				break;
			}
		}

		bool[] syncedEmptySpawnPoints = new bool[SpawnPointsCount];
		for (int i = 0; i < SpawnPointsCount; i++)
		{			
			syncedEmptySpawnPoints[i] = IsEmptySpawnPoints[i];
		}

		photonView.RPC("OnEmptySpawnPointsInfoReceived", PhotonPlayer.Find(id), MySpawnPointIndex, syncedEmptySpawnPoints);
	}

	[PunRPC]
	private void OnEmptySpawnPointsInfoReceived(int indexToSpawn, bool[] syncedEmptySpawnPointsInfo)
	{
		// Sync emptySpawnpoints
		for (int i = 0; i < IsEmptySpawnPoints.Length; i++)
		{
			IsEmptySpawnPoints[i] = syncedEmptySpawnPointsInfo[i];
		}
		
		// Instantiate gameobject
		MyGameObject = PhotonNetwork.Instantiate(roomPlayerPrefab.name, spawnPoints[indexToSpawn], Quaternion.identity, 0);
	}
}