using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour
{
	// Key = playerID, Value = characterPhotonViewID controlled by local player	
	public Dictionary<int, int> CharacterPhotonViewID = new Dictionary<int, int>();	
	
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

		int keyViewID = (int)senderCharacterPhotonViewID;

		if (CharacterPhotonViewID.ContainsKey(keyViewID))
			return;

		CharacterPhotonViewID.Add(senderid, keyViewID);
	}
}
