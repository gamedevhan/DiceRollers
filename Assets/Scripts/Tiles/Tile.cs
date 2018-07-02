using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public int index;

	// This event needs to be raised when character finish lerping	
	public static event Action<int, int> TileEntered = delegate { };

	public void OnCharacterEnter(int tilesToMove, int characterViewID)
	{
		if (TileEntered != null)
			TileEntered(tilesToMove, characterViewID);
	}
}