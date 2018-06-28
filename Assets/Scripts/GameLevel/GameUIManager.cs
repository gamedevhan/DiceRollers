using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
	[SerializeField]
	private GameObject RollButton;

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnTurnBegin;
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnTurnBegin;
	}

	private void OnTurnBegin(byte eventcode, object content, int senderid)
	{
		if (eventcode != PhotonEventCode.TurnBegin)
			return;

		if ((int)content != PhotonNetwork.player.ID)
		{
			RollButton.SetActive(false);
		}
		else if (!RollButton.activeInHierarchy)
		{
			RollButton.SetActive(true);
		}
	}
}
