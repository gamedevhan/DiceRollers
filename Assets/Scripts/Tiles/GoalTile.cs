public class GoalTile : Tile
{
	protected override void Start()
	{
		base.Start();
	}

	public override void OnCharacterEnter(CharacterMovementController character)
	{
		character.MoveLeft = 0;
		character.GetComponent<CharacterAnimationController>().IsWalking = false;
		
		string ownerNickName = character.PhotonView.owner.NickName;
		GameManager.Instance.GameOver(ownerNickName);		
	}
}