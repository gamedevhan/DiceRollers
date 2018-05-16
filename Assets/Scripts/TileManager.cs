using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	public static TileManager Instance { get; private set; }

	public List<Transform> Tiles = new List<Transform>();

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
		foreach (Transform tile  in transform)
		{
			Tiles.Add(tile);
		}
	}
}
