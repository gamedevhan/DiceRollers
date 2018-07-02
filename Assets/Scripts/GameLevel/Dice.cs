using System;
using System.Collections;
using UnityEngine;

public class Dice : Photon.MonoBehaviour
{
	public int DiceResult { get; private set; }

	private MeshRenderer meshRenderer;
	private Animator animator;

	public static event Action<int> DiceRollEvent = delegate(int diceResult) { };

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
		StartCoroutine(Roll());
	}

	private IEnumerator Roll()
	{
		DiceResult = UnityEngine.Random.Range(1, 7);
		photonView.RPC("PlayRollAnimation", PhotonTargets.All, DiceResult);

		yield return new WaitForSeconds(1f);
		DiceRollEvent(DiceResult);
	}

	[PunRPC]
	private void PlayRollAnimation(int diceResult)
	{	
		meshRenderer.enabled = true;
		animator.Play("Dice" + diceResult);				
	}
}
