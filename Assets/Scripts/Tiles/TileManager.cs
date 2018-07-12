using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	public static TileManager Instance { get; private set; }

	public List<Tile> Tiles = new List<Tile>();

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		PopulateTileList();
	}

	private void PopulateTileList()
	{
		int i = 0;
		foreach (Transform tileTransform in transform)
		{
			Tile tile = tileTransform.GetComponent<Tile>();
			tile.index = i;
			Tiles.Add(tile);
			i++;
		}
	}
}
