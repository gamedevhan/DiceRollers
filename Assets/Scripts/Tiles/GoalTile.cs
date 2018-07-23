using System.Collections;
using UnityEngine;

public class GoalTile : Tile
{
    #region FireWorksTest
    public bool Test;
    public bool instantiated = false;
    private void Update()
    {
        if (Test && instantiated)
        {
            instantiated = false;
            LaunchFireworks();
            StartCoroutine(LaunchFireworks());
        }
    }
    #endregion

    [SerializeField]
    private GameObject[] fireWorks;

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

    [PunRPC]
    private void RpcInstantiateFireworks()
    {
        StartCoroutine(LaunchFireworks());
    }

    private IEnumerator LaunchFireworks()
    {
        for (int i = 0; i < fireWorks.Length; i++)
        {
            Vector3 position = new Vector3(0, transform.position.y + 3, 0);
            Instantiate(fireWorks[i], position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }   
}