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

	private void Awake()
	{
		if (PhotonNetwork.isMasterClient)
		{
			GameObject MasterClientGO = InstantiateRoomPlayer(0);			
			MasterClientGO.GetComponent<RoomPlayerUI>().DisplayButtons();
		}
	}

	#endregion

	#region Photon CallBacks

	// On newPlayer Joined, masterClient instantiate prefab on emptySpawnPoint and transfer ownership
	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		if (!PhotonNetwork.isMasterClient) { return; }

		// Find emptySpawnPoint
		int i;
		for (i = 0; i < IsEmptySpawnPoints.Length; i++)
		{
			if (IsEmptySpawnPoints[i]) { break; }			
		}
		
		// Instantiate and transfer ownership
		GameObject newRoomPlayerGO = InstantiateRoomPlayer(i);
		PhotonView newView = PhotonView.Get(newRoomPlayerGO);
		newView.TransferOwnership(newPlayer);
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
	
	private GameObject InstantiateRoomPlayer(int spawnPointIndex)
	{
		IsEmptySpawnPoints[spawnPointIndex] = false;
		return PhotonNetwork.Instantiate(roomPlayerPrefab.name, spawnPoints[spawnPointIndex], Quaternion.identity, 0);		
	}
}