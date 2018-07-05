using System;
using System.Collections;
using UnityEngine;

public class Dice : Photon.MonoBehaviour
{
	public int DiceResult { get; private set; }

	private MeshRenderer meshRenderer;
	private Animator animator;
	private Vector3 offSetFromCamera;

	public static event Action<int> DiceRollEvent = delegate(int diceResult) { };

	private void Awake()
	{		
		meshRenderer = GetComponent<MeshRenderer>();
		animator = GetComponent<Animator>();		
	}
	
	private void OnEnable()
	{
		GameUIManager.RollButtonPress += OnRollButtonPressed;
	}

	private void OnDisable()
	{
		GameUIManager.RollButtonPress -= OnRollButtonPressed;
	}

	private void Start()
	{
		meshRenderer.enabled = false;
		offSetFromCamera = Camera.main.transform.position - transform.position;
	}

	private void OnRollButtonPressed()
	{
        // TODO: Move this line to somewhere else
        // transform.position = Camera.main.transform.position - offSetFromCamera; 
        StartCoroutine(Roll());
	}

	private IEnumerator Roll()
	{
		DiceResult = UnityEngine.Random.Range(1, 7);
		DebugUtility.Log(DiceResult);
		photonView.RPC("PlayRollAnimation", PhotonTargets.All, DiceResult);

		yield return new WaitForSeconds(2f);
		DiceRollEvent(DiceResult);
        yield return new WaitForSeconds(1f);
        meshRenderer.enabled = false;
	}

	[PunRPC]
	private void PlayRollAnimation(int diceResult)
	{	
		meshRenderer.enabled = true;
		animator.Play("Dice" + diceResult);	
	}
}
