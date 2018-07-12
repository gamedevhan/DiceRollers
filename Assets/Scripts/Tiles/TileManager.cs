using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	public static TileManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		InitializeTileIndex();
	}

	private void InitializeTileIndex()
	{
		int i = 0;		
		foreach (Transform tileTransform in transform)
		{			
			Tile tile = tileTransform.GetComponent<Tile>();
			tile.Index = i;			
			i++;
		}
	}
}
