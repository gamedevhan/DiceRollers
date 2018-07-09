using UnityEngine;

[RequireComponent(typeof(CharacterMovementController))]
public class CharacterAnimationController : MonoBehaviour
{
	private CharacterMovementController character;
	private Animator animator;

	private void Start()
	{
		character = GetComponent<CharacterMovementController>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		animator.SetBool("isMoving", character.ShouldPlayMoveAnim);
	}
}
