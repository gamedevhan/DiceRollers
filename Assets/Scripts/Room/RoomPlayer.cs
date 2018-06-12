using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayer : MonoBehaviour
{		
	public bool IsReady;

	public PhotonPlayer Player { get; private set; }
	public string PlayerName { get; private set; }

	private void Start()
	{	
		IsReady = false;
	}
	
	private void DisplayName()
	{		
		//PlayerName = PhotonNetwork.playerName;
		//RoomPlayerUI roomPlayerUI = GetComponent<RoomPlayerUI>();
		//roomPlayerUI.PlayerName = PlayerName;
	}

	[PunRPC]
	private void ToggleReady()
	{
		IsReady = !IsReady;
		RoomPlayerUI roomPlayerUI = GetComponent<RoomPlayerUI>();
		roomPlayerUI.ReadyIcon.alpha = IsReady ? 255 : 0;
	}
}
