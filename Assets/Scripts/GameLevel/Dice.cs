﻿using System;
using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
	public int DiceResult { get; private set; }
	public MeshRenderer MeshRednderer;

	private Animator animator;
	private Vector3 offSetFromCamera;
	private PhotonView photonView;

	public static event Action<int> DiceRollEvent = delegate(int diceResult) { };

	private void Awake()
	{		
		MeshRednderer = GetComponent<MeshRenderer>();
		animator = GetComponent<Animator>();
		photonView = GetComponent<PhotonView>();
	}
	
	private void Start()
	{
		MeshRednderer.enabled = false;
		offSetFromCamera = Camera.main.transform.position - transform.position;
	}
		
	public IEnumerator Roll()
	{
		DiceResult = UnityEngine.Random.Range(1, 7);
		DebugUtility.Log(DiceResult);
		photonView.RPC("PlayRollAnimation", PhotonTargets.All, DiceResult);

		yield return new WaitForSeconds(2f);
		DiceRollEvent(DiceResult);
        yield return new WaitForSeconds(1f);
        MeshRednderer.enabled = false;
	}

	public void Reset()
	{
		transform.position =  Camera.main.transform.position - offSetFromCamera;
		transform.rotation = Quaternion.identity;
	}

	[PunRPC]
	private void PlayRollAnimation(int diceResult)
	{	
		MeshRednderer.enabled = true;
		animator.Play("Dice" + diceResult);	
	}
}
