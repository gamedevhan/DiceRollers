﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public TurnManager TurnManager { get; private set; }        
    public Dice Dice;

    private PlayerCharacterManager playerCharacterManager;
    private GameLevelUIManager gameLevelUIManager;
    private CameraController cameraController;
    private PhotonView photonView;
        
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
        gameLevelUIManager = GetComponent<GameLevelUIManager>();
		cameraController = GetComponent<CameraController>();		
		TurnManager = GetComponent<TurnManager>();
        photonView = GetComponent<PhotonView>();
	}
    
	public void OnTurnBegin(int currentTurnPlayerID)
	{
		int currentTurnCharacterViewID = playerCharacterManager.CharacterPhotonViewID[currentTurnPlayerID];
        Transform currentTurnCharacter = PhotonView.Find(currentTurnCharacterViewID).transform;

        Dice.Reset(currentTurnCharacter);

        string currentTurnPlayerName = PhotonPlayer.Find(currentTurnPlayerID).NickName;
        photonView.RPC("RpcCurrentTurnPlayerLabel", PhotonTargets.All, currentTurnPlayerName);        

        cameraController.FollowTarget = currentTurnCharacter;        
        
		if (PhotonNetwork.player.ID == currentTurnPlayerID)
		{
            gameLevelUIManager.ActivateRollButton();
        }
	}

    public void GameOver(string winnerNickName)
    {
        DebugUtility.Log(winnerNickName + " Win!");
    }
}
