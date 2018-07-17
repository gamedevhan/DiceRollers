using UnityEngine;

[RequireComponent(typeof(CharacterMovementController))]
public class CharacterAnimationController : MonoBehaviour
{
    public bool IsWalking { get; set; }

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("isWalking", IsWalking);
    }
        
    public void PlayWalkAnimation(bool isWalking)
    {
        PhotonView.Get(this).RPC("RpcSetWalkParameter", PhotonTargets.All, isWalking);
    }

    [PunRPC]
    private void RpcSetWalkParameter(bool walkAnimationParmeter)
    {
        IsWalking = walkAnimationParmeter;
    }
}
