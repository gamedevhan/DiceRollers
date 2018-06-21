using System.Collections.Generic;
using UnityEngine;

public class RoomPlayerCharacter : MonoBehaviour
{	
	[SerializeField]
	private Transform charactersParent;

	private int currentCharacterIndex = 0;
	private List<GameObject> characters = new List<GameObject>();	
	private PhotonView photonView;	

	private void Awake()
	{	
		photonView = PhotonView.Get(this);		

		foreach (Transform character in charactersParent)
		{			
			characters.Add(character.gameObject);		
		}
	}

	private void OnEnable()
	{
		RoomUIManager.ReadyPressed += OnReadyButtonPressed;
	}

	private void OnDestroy()
	{
		RoomUIManager.ReadyPressed += OnReadyButtonPressed;
	}

	private void Start()
	{
		FaceCamera();
		// Send RPC to request other players' current character		
		photonView.RPC("SendCurrentCharacterIndex", PhotonTargets.Others, PhotonNetwork.player.ID);
	}

	private void OnReadyButtonPressed()
	{
		switch (characters[currentCharacterIndex].name)
		{
			case "Ai":
				LevelTransitionManager.Instance.SelectedCharacter = Character.Ai;
				break;
			case "UnityChan":
				LevelTransitionManager.Instance.SelectedCharacter = Character.UnityChan;
				break;
			case "Riko":
				LevelTransitionManager.Instance.SelectedCharacter = Character.Riko;
				break;
			case "Yuji":
				LevelTransitionManager.Instance.SelectedCharacter = Character.Yuji;
				break;
			default:
				break;
		}		
	}

	[PunRPC]
	public void DisplayPreviousCharacter()
	{
		if (currentCharacterIndex == 0)
		{
			currentCharacterIndex = characters.Count - 1;
		}
		else
		{
			currentCharacterIndex--;
		}

		DisplaySelectedCharacter(currentCharacterIndex);		
	}

	[PunRPC]
	public void DisplayNextCharacter()
	{
		if (currentCharacterIndex == characters.Count - 1)
		{
			currentCharacterIndex = 0;
		}
		else
		{
			currentCharacterIndex++;
		}

		DisplaySelectedCharacter(currentCharacterIndex);		
	}	
	
	private void DisplaySelectedCharacter(int characterIndex)
	{
		foreach (Transform characterTransform in charactersParent)
		{
			characterTransform.gameObject.SetActive(false);
		}
		characters[characterIndex].SetActive(true);
	}

	[PunRPC]
	private void SendCurrentCharacterIndex(int senderID)
	{		
		photonView.RPC("SyncOtherPlayerCharacter", PhotonPlayer.Find(senderID), currentCharacterIndex);
	}

	[PunRPC]
	private void SyncOtherPlayerCharacter(int selectedCharacterIndex)
	{
		currentCharacterIndex = selectedCharacterIndex;
		DisplaySelectedCharacter(currentCharacterIndex);
	}

	private void FaceCamera()
	{
		Camera camera = Camera.main;
		Vector3 targetPosition = new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z);
		charactersParent.LookAt(targetPosition);
	}
}
