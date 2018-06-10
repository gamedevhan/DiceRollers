using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : Photon.PunBehaviour
{	
	public static RoomManager Instance { get; private set; }

	[SerializeField]
	private GameObject roomPlayerPrefab;

	[SerializeField]
	private Vector3[] spawnPoints;
	
	[SerializeField]
	private bool[] IsEmptySpawnPoints = { true, true, true, true };

	private bool isReady;
	private int mySpawnPointIndex = 0;
	public GameObject MyGameObject { get; private set; }
	
	#region Unity CallBacks

	private void Awake()
	{
		Instance = this;
	}

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
	
	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		if (PhotonNetwork.player != otherPlayer)
		{
			// TODO: Set leaver's isEmptySpawnPosition = true
			
		}
	}

	#endregion
		
	public void OnReadyPressed()
	{
		RoomPlayerUI roomPlayerUI = MyGameObject.GetComponent<RoomPlayerUI>();
		isReady = !isReady;
		roomPlayerUI.ReadyIcon.alpha = isReady ? 255 : 0;
	}
		
	public void OnLeavePressed()
	{		
		IsEmptySpawnPoints[mySpawnPointIndex] = true;
		PhotonNetwork.LeaveRoom(false);
		PhotonNetwork.JoinLobby();
		SceneManager.LoadScene("01 Lobby");
	}
	
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
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
				mySpawnPointIndex = i;
				break;
			}
		}

		bool[] syncedEmptySpawnPoints = new bool[SpawnPointsCount];
		for (int i = 0; i < SpawnPointsCount; i++)
		{			
			syncedEmptySpawnPoints[i] = IsEmptySpawnPoints[i];
		}

		photonView.RPC("OnEmptySpawnPointsInfoReceived", PhotonPlayer.Find(id), mySpawnPointIndex, syncedEmptySpawnPoints);
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