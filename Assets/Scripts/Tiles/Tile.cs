using UnityEngine;

public abstract class Tile : MonoBehaviour
{
	public int index;

    protected bool isStartTile;
    protected bool isEndTile;

    protected virtual void Start()
    {
        if (index == 0)
        {
            isStartTile = true;
        }
        else if (index == TileManager.Instance.Tiles.Count - 1)
        {
            isEndTile = true;            
        }
    }

    public abstract void OnCharacterEnter(CharacterMovementController character);
}