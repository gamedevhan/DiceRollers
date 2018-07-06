using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public TurnManager TurnManager { get; private set; }

	[SerializeField]
	private UIButton rollButton;

	private PlayerCharacterManager playerCharacterManager;
	private CameraController cameraController;

	[SerializeField]
	private Dice dice;

	public static GameManager Instance = null;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}

		playerCharacterManager = GetComponent<PlayerCharacterManager>();
		cameraController = GetComponent<CameraController>();		
		TurnManager = GetComponent<TurnManager>();
	}

	private void Start()
	{
		rollButton.gameObject.SetActive(false);
	}

	public void OnRollButtonPress()
	{
		rollButton.gameObject.SetActive(false);
		dice.MeshRednderer.enabled = true;
		StartCoroutine(dice.Roll());
	}

	public void OnTurnBegin()
	{
		int currentTurnPlayerID = TurnManager.CurrentTurnPlayerID;
		int currentTurnCharacterViewID = playerCharacterManager.CharacterPhotonViewID[currentTurnPlayerID];
        Transform currentTurnCharacter = PhotonView.Find(currentTurnCharacterViewID).transform;

        DebugUtility.Log(currentTurnPlayerID + "'s turn");

        cameraController.FollowTarget = currentTurnCharacter;

        dice.Reset(currentTurnCharacter);
        
		if (PhotonNetwork.player.ID == currentTurnPlayerID)
		{
			rollButton.gameObject.SetActive(true);
		}
	}
}
