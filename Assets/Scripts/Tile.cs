using UnityEngine;

public class Tile : MonoBehaviour
{
	public Transform nextTile;

	// This event needs to be raised when character finish lerping
	public delegate void EnterTileEvent();
	public static event EnterTileEvent TileEntered;	

	public void OnCharacterEnter()
	{
		if (TileEntered != null)
			TileEntered();
	}	
}