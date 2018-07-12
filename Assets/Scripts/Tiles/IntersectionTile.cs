using System.Collections.Generic;
using UnityEngine;

public class IntersectionTile : Tile
{
	[SerializeField]
	private GameObject arrowPrefab;	
	private List<Tile> nextTiles = new List<Tile>();
	private Dictionary<Arrow, Tile> direction = new Dictionary<Arrow, Tile>();

	protected override void Start()
	{
		base.Start();
		for (int i = 1; i < neighbourTiles.Count; i++)
		{
			nextTiles.Add(neighbourTiles[i]);
			GameObject arrowGameObject = Instantiate(arrowPrefab, transform);
			Arrow arrow = arrowGameObject.GetComponent<Arrow>();
			direction.Add(arrow, neighbourTiles[i]);

			Vector3 arrowPosition = new Vector3((neighbourTiles[i].transform.position.x + transform.position.x) / 2, 0, (neighbourTiles[i].transform.position.z + transform.position.z) / 2);
			arrow.transform.position = arrowPosition;

			Vector3 targetPosition = new Vector3(neighbourTiles[i].transform.position.x, transform.position.y, neighbourTiles[i].transform.position.z);
			arrow.LookatTarget(targetPosition);
		}
	}

	public override void OnCharacterEnter(CharacterMovementController character)
	{
		foreach (Arrow arrow in direction.Keys)
		{
			arrow.MeshRenderer.enabled = true;
		}
	}
}
