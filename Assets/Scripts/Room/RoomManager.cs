using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : Photon.PunBehaviour
{		
	[SerializeField]
	private GameObject roomPlayerPrefab;
	
	[SerializeField]
	private Transform roomPlayersParent;
	private List<Transform> roomPlayers = new List<Transform>();

	#region Unity CallBacks

	private void Awake()
	{
		foreach (Transform roomPlayer in roomPlayersParent)
		{
			roomPlayers.Add(roomPlayer);
		}
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
		// Find roomPlayerGO owned by this player
		// Transfer ownership to scene
		// Hide scene owned
	}

	#endregion

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom(false);
		PhotonNetwork.JoinLobby();
		SceneManager.LoadScene("01 Lobby");
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		// Find first owned by scene
		GameObject sceneOwnedGO = null;

		for (int i = 0; i < roomPlayers.Count; i++)
		{
			if (roomPlayers[i].GetComponent<PhotonView>().isSceneView)
			{	
				sceneOwnedGO = roomPlayers[i].gameObject;
				break;
			}
		}
		
		Debug.Log("<b><color=green>Found scene owned GO:</color>" + sceneOwnedGO.name + "</b>");
		// transfer it's ownership to local player
		


		// Hide scene owned
		// Display character if not owned by scene
    }
}