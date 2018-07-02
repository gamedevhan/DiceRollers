using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterAnimation : MonoBehaviour
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
		animator.SetBool("isMoving", character.ShouldPlayMoveAnim);
	}
}
