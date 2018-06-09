using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomPlayerSpawnManager : Photon.PunBehaviour
{	
	[SerializeField]
	private GameObject roomPlayerPrefab;

	[SerializeField]
	private Vector3[] spawnPoints;
	
	[SerializeField]
	private bool[] IsEmptySpawnPoints = { true, true, true, true };

	#region Unity CallBacks

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	#endregion
	
	#region Photon CallBacks

	// On newPlayer Joined, masterClient instantiate prefab on emptySpawnPoint and transfer ownership
	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{

	}

	#endregion
	
	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom(false);
		PhotonNetwork.JoinLobby();
		SceneManager.LoadScene("01 Lobby");
		// TODO: set isempty true
	}
	
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// If masterClient, instantiate on spawnPoints[0], set IsEmptySpawnPoints[0] false
		if (PhotonNetwork.isMasterClient)
		{
			IsEmptySpawnPoints[0] = false;
			PhotonNetwork.Instantiate(roomPlayerPrefab.name, spawnPoints[0], Quaternion.identity, 0);		
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
		int indexToSpawn = new int();
		for (int i = 0; i < SpawnPointsCount; i++)
		{
			if (IsEmptySpawnPoints[i])
			{
				IsEmptySpawnPoints[i] = false;
				indexToSpawn = i;
				break;
			}
		}

		bool[] syncedEmptySpawnPoints = new bool[SpawnPointsCount];
		for (int i = 0; i < SpawnPointsCount; i++)
		{			
			syncedEmptySpawnPoints[i] = IsEmptySpawnPoints[i];
		}

		photonView.RPC("OnEmptySpawnPointsInfoReceived", PhotonPlayer.Find(id), indexToSpawn, syncedEmptySpawnPoints);
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
		PhotonNetwork.Instantiate(roomPlayerPrefab.name, spawnPoints[indexToSpawn], Quaternion.identity, 0);
	}
}