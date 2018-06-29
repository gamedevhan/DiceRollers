using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : Photon.MonoBehaviour
{
	public int DiceResult { get; private set; }

	private MeshRenderer meshRenderer;
	private Animator animator;

	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		animator = GetComponent<Animator>();
		meshRenderer.enabled = false;
	}

	private void OnEnable()
	{
		GameUIManager.RollButtonPress += OnRollButtonPressed;
	}

	private void OnDestroy()
	{
		GameUIManager.RollButtonPress -= OnRollButtonPressed;
	}

	private void OnRollButtonPressed()
	{
		DiceResult = Random.Range(1, 7);		
		photonView.RPC("Roll", PhotonTargets.All, DiceResult);
	}

	[PunRPC]
	private void Roll(int diceResult)
	{		
		meshRenderer.enabled = true;		
		animator.Play("Dice" + diceResult);
	}
}
