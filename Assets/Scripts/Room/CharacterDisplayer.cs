using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplayer : MonoBehaviour
{	
	[SerializeField]
	private Transform charactersParent;

	private List<GameObject> characters = new List<GameObject>();
	private int currentCharacterIndex = 0;
	
	private void Awake()
	{
		foreach (Transform character in charactersParent)
		{			
			characters.Add(character.gameObject);		
		}
	}

	private void Start()
	{
		FaceCamera();
	}
	
	[PunRPC]
	public void DisplayPreviousCharacter()
	{
		// Get character index
		currentCharacterIndex = currentCharacterIndex != 0 ? currentCharacterIndex - 1 : characters.Count - 1;
		
		DisplayCurrentCharacter();
	}

	[PunRPC]
	public void DisplayNextCharacter()
	{
		// Get character index
		currentCharacterIndex = currentCharacterIndex != characters.Count - 1 ? currentCharacterIndex + 1 : 0;

		DisplayCurrentCharacter();
	}
		
	private void DisplayCurrentCharacter()
	{
		foreach (Transform characterTransform in charactersParent)
		{
			characterTransform.gameObject.SetActive(false);
		}		
		characters[currentCharacterIndex].SetActive(true);
	}

	private void FaceCamera()
	{
		Camera camera = Camera.main;
		Vector3 targetPosition = new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z);
		charactersParent.LookAt(targetPosition);
	}
}
