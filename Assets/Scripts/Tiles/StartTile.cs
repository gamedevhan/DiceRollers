public class StartTile : Tile
{
	protected override void Start()
	{
		base.Start();
	}

	public override void OnCharacterEnter(CharacterMovementController character)
	{
		character.MoveLeft = 0;
		character.ShouldPlayMoveAnim = false;
		character.TileBeforeMove = this;
		character.TileAfterMove = nextTile;
		GameManager.Instance.TurnManager.TurnEnd();
	}
}