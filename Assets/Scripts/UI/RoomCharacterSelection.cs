using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCharacterSelection : MonoBehaviour
{
	public GameObject[] characters;
	private int currentCharacter = 0;

	private void Start()
	{
		FaceCamera();
	}

	public void OnLeftButtonClick()
	{
		if (currentCharacter != 0) { currentCharacter--; }
		else currentCharacter = characters.Length - 1;
		
		DisplayCurrnetCharacter();
		FaceCamera();
	}

	public void OnRightButtonClick()
	{
		if (currentCharacter != characters.Length - 1) { currentCharacter++; }
		else currentCharacter = 0;
		
		DisplayCurrnetCharacter();
		FaceCamera();
	}

	private void DisplayCurrnetCharacter()
	{
		foreach (Transform character in transform)
		{
			character.gameObject.SetActive(false);
		}

		characters[currentCharacter].SetActive(true);
	}

	private void FaceCamera()
	{		
		Camera camera = Camera.main;
		Vector3 targetPosition = new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z);
		transform.LookAt(targetPosition);
	}
}
