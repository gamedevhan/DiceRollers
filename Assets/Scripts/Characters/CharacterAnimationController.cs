using UnityEngine;

[RequireComponent(typeof(CharacterMovementController))]
public class CharacterAnimationController : MonoBehaviour
{
    public bool IsWalking = false;	
	private Animator animator;
    
	private void Start()
	{		
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		animator.SetBool("isWalking", IsWalking);
	}
}
