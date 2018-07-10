public class NormalTile : Tile
{
    protected override void Start()
    {
        base.Start();
    }

    public override void OnCharacterEnter(CharacterMovementController character)
    {
        base.OnCharacterEnter(character);

        if (character.MoveLeft > 0) // character is moving forward
        {
            // Are we on the last tile?
            if (!isEndTile)            
            {
                character.CurrentTile = TileManager.Instance.Tiles[index];
                character.NextTile = TileManager.Instance.Tiles[index + 1];
                StartCoroutine(character.Move());
            }
        }
        else if (character.MoveLeft < 0) // character is moving forward
        {
            // Are we on the first tile?
            if (index == 0)
            {
                DebugUtility.Log("On the first Tile!");
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
        else // if (character.Movleft == 0)
        {
            character.ShouldPlayMoveAnim = false;

            if (index == 0)
            {
                DebugUtility.Log("On the first Tile!");                
                GameManager.Instance.TurnManager.TurnEnd(); // Moved backward and reached start tile, end turn
            }
            else
            {
                character.CurrentTile = TileManager.Instance.Tiles[index];
                character.NextTile = TileManager.Instance.Tiles[index + 1];
                GameManager.Instance.TurnManager.TurnEnd();
            }
        }
    }
}
