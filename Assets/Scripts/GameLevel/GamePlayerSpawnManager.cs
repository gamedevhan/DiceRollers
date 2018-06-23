using UnityEngine;

public class GamePlayerSpawnManager : MonoBehaviour
{
	[SerializeField]
	private Transform startTile;

	// Use this for initialization
	void Start()
	{
		SpawnGamePlayer();
		// SendRPC to other players to sync number of player finished loading
		// If number of player finished loading == photonplayers in the room loading has finished, start game!
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void SpawnGamePlayer()
	{		
		string characterName = LevelTransitionManager.Instance.SelectedCharacter.ToString();
		Debug.Log(characterName);
		PhotonNetwork.Instantiate(characterName, startTile.position, Quaternion.identity, 0);
	}
}
