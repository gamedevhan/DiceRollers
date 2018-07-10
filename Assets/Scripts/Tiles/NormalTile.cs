public class NormalTile : Tile
{
    protected override void Start()
    {
        base.Start();
    }

    public override void OnCharacterEnter(CharacterMovementController character)
    {
        if (character.MoveLeft > 0) // character is moving forward
        {
            // Are we on the last tile?
            if (isEndTile)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                character.CurrentTile = TileManager.Instance.Tiles[index];
                character.NextTile = TileManager.Instance.Tiles[index + 1];
                StartCoroutine(character.Move());
            }
        }
        else if (character.MoveLeft < 0) // character is moving forward
        {
            // Are we on the first tile?
            if (isStartTile)
            {
                character.MoveLeft = 0;
                GameManager.Instance.TurnManager.TurnEnd(); // Moved backward and reached start tile, end turn
            }
            else
            {
                character.CurrentTile = TileManager.Instance.Tiles[index];
                character.NextTile = TileManager.Instance.Tiles[index - 1];
                StartCoroutine(character.Move());
            }
        }
        else // if (character.MoveLeft == 0)
        {
            character.CurrentTile = TileManager.Instance.Tiles[index];
            character.NextTile = TileManager.Instance.Tiles[index + 1];
            character.ShouldPlayMoveAnim = false;            
        }
    }
}
