using UnityEngine;

public class Tile : MonoBehaviour
{
	public int index;

	// This event needs to be raised when character finish lerping
	public delegate void EnterTileEvent();
	public static event EnterTileEvent TileEntered;
	
	public void OnCharacterEnter()
	{
		if (TileEntered != null)
			TileEntered();
	}	
}