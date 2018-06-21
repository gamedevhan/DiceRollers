using System.Collections.Generic;
using UnityEngine;

public enum Character
{
	Ai,
	UnityChan,
	Riko,
	Yuji,	
	None
};

public class LevelTransitionManager : MonoBehaviour
{	
	public Character SelectedCharacter = Character.None; // For local player
	public List<RoomPlayer> roomPlayers = new List<RoomPlayer>();

	public static LevelTransitionManager Instance = null;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;			
		}
		else
		{
			DestroyGameObject();
		}
	}

	public void LoadGameLevel()
	{	
		if (roomPlayers.Count > 1 && IsEveryoneReady())
		{
			Debug.Log("Masterclient will load level");
		}
	}

	public void DestroyGameObject()
	{		
		Destroy(gameObject);
	}

	private bool IsEveryoneReady()
	{
		// Check if everyone is ready
		foreach (var player in roomPlayers)
		{
			if (!player.IsReady)
			{
				Debug.Log("Someone is not ready");
				return false;	
			}
		}
		
		Debug.Log("Everyone is ready!");
		return true;
	}
}
