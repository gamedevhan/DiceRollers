public class GoalTile : Tile
{
	public override void OnCharacterEnter(CharacterMovementController character)
	{
		character.MoveLeft = 0;
		character.ShouldPlayMoveAnim = false;
		string ownerNickName = character.PhotonView.owner.NickName;
		DebugUtility.Log(ownerNickName + " Win!");
	}
}