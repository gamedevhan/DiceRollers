using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class AnimationController : MonoBehaviour
{
	private CharacterMovement character;
	private Animator animator;

	private void Start()
	{
		character = GetComponent<CharacterMovement>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		animator.SetBool("isMoving", character.IsMoving);
	}
}
