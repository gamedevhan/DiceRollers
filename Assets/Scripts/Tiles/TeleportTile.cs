using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Tile))]
public class TeleportTile : MonoBehaviour, ISpecialTile
{
	private const float FxDelay = 1f;
	[SerializeField] private Transform destination;    

	public IEnumerator OnSpecialTileEnter(CharacterMovement character)
	{
		// TODO: Play FX

		yield return new WaitForSeconds(FxDelay);

		character.transform.position = destination.position;

		// Update CurrentTile and NextTile
		character.CurrentTile = TileManager.Instance.Tiles[destination.GetComponent<Tile>().index];
		character.NextTile = TileManager.Instance.Tiles[destination.GetComponent<Tile>().index + 1];
	}
}
