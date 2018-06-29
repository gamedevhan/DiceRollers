using System;
using UnityEngine;

public class Dice : Photon.MonoBehaviour
{
	public int DiceResult { get; private set; }

	private MeshRenderer meshRenderer;
	private Animator animator;

	public static event Action<int> DiceRoll = delegate(int diceResult) { };

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
		DiceResult = UnityEngine.Random.Range(1, 7);
		photonView.RPC("Roll", PhotonTargets.All, DiceResult);
	}

	[PunRPC]
	private void Roll(int diceResult)
	{	
		meshRenderer.enabled = true;

		// TODO: Make this coroutine so Diceroll event get published after animation finished playing
		animator.Play("Dice" + diceResult);		
		DiceRoll(diceResult);
	}
}
