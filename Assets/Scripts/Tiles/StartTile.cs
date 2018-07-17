public class StartTile : Tile
{
	protected override void Start()
	{
		base.Start();
	}

	public override void OnCharacterEnter(CharacterMovementController character)
	{
		character.MoveLeft = 0;
        character.GetComponent<CharacterAnimationController>().PlayWalkAnimation(false);
        character.TileBeforeMove = this;
		character.TileAfterMove = NextTile;
		GameManager.Instance.TurnManager.TurnEnd();
	}
}