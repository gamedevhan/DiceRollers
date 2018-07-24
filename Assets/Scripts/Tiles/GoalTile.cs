using System.Collections;
using UnityEngine;

public class GoalTile : Tile
{
    protected override void Start()
    {
        base.Start();
    }

    public override void OnCharacterEnter(CharacterMovementController character)
    {
        character.MoveLeft = 0;
        character.GetComponent<CharacterAnimationController>().PlayWalkAnimation(false);

        GetComponent<PhotonView>().RPC("RpcInstantiateFireworks", PhotonTargets.All);

        string ownerNickName = character.PhotonView.owner.NickName;
        GameManager.Instance.GameOver(ownerNickName);
    }
}