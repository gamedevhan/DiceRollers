using System;
using UnityEngine;

public class GamePlayerSpawnManager : MonoBehaviour
{
    public Tile startTile { get; private set; }
    public static GamePlayerSpawnManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	// Use this for initialization
	void Start()
	{
        startTile = FindObjectOfType<StartTile>() as Tile;
        SpawnGamePlayer();
	}

	private void SpawnGamePlayer()
	{
		string characterName = LevelTransitionManager.Instance.SelectedCharacter.ToString();
        GameObject playerCharacter = PhotonNetwork.Instantiate(characterName, startTile.transform.position, Quaternion.Euler(0, 180, 0), 0);
		PhotonView characterPhotonView = PhotonView.Get(playerCharacter);
				
		RaiseEventOptions eventOptions = new RaiseEventOptions { CachingOption = EventCaching.AddToRoomCache, Receivers = ReceiverGroup.All };		
		PhotonNetwork.RaiseEvent(PhotonEventCode.PlayerLoaded, characterPhotonView.viewID, true, eventOptions);
	}
}
