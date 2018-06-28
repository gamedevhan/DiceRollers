using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour
{
	// Key = playerID, Value = characterPhotonViewID controlled by local player
	public Dictionary<int, int> CharacterPhotonViewID = new Dictionary<int, int>();

	public static PlayerCharacterManager Instance = null;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnPlayerLoaded;
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnPlayerLoaded;
	}

	private void OnPlayerLoaded(byte eventcode, object senderCharacterPhotonViewID, int senderid)
	{
		if (eventcode != PhotonEventCode.PlayerLoaded)
			return;

		CharacterPhotonViewID.Add(senderid, (int)senderCharacterPhotonViewID);
	}
}
