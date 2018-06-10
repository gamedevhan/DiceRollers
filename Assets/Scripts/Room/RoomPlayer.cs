using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayer : MonoBehaviour
{
	public string Name;
	public bool IsReady;

	private void Start()
	{
		IsReady = false;
	}

	[PunRPC]
	private void ToggleReady()
	{
		IsReady = !IsReady;
		RoomPlayerUI roomPlayerUI = GetComponent<RoomPlayerUI>();
		roomPlayerUI.ReadyIcon.alpha = IsReady ? 255 : 0;
	}
}
