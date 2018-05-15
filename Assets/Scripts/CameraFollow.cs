using UnityEngine;

public class CameraFollow : MonoBehaviour
{	
	public Transform currentCharacter;
	
	private Vector3 offset;

	private void Start()
	{
		offset = transform.position - currentCharacter.position;
	}

	private void LateUpdate()
	{
		transform.position = currentCharacter.position + offset;
	}
}
