using System;
using UnityEngine;

public class GamePlayerSpawnManager : MonoBehaviour
{
	[SerializeField]
	private Transform startTile;
	
	// Use this for initialization
	void Start()
	{
		SpawnGamePlayer();
	}

	private void SpawnGamePlayer()
	{
		string characterName = LevelTransitionManager.Instance.SelectedCharacter.ToString();
		Debug.Log(characterName);
		PhotonNetwork.Instantiate(characterName, startTile.position, Quaternion.identity, 0);
	}
}
