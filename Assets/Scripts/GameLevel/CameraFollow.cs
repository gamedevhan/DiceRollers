using UnityEngine;

public class CameraFollow : MonoBehaviour
{	
	private Transform currentCharacter;	
	private Vector3 offset;

	private void Start()
	{
		offset = transform.position;
	}

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnTurnBegin;
	}

	private void OnDestroy()
	{
		PhotonNetwork.OnEventCall -= OnTurnBegin;
	}

	private void LateUpdate()
	{
		if (currentCharacter != null)
		{
			transform.position = currentCharacter.position + offset;
		}
	}

	private void OnTurnBegin(byte eventcode, object currentTurnPlayerID, int senderid)
	{
		if (eventcode != PhotonEventCode.TurnBegin)
			return;

		int currentTurnCharacterViewID = PlayerCharacterManager.Instance.CharacterPhotonViewID[(int)currentTurnPlayerID];
		Debug.Log(currentTurnCharacterViewID + "'s turn");
		currentCharacter = PhotonView.Find(currentTurnCharacterViewID).transform;
	}
}
