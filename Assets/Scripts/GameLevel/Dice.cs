using System;
using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
	public int DiceResult { get; private set; }
	public MeshRenderer MeshRednderer { get; private set; }

	private Animator animator;
	private Vector3 offSet;
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
        offSet = transform.position;
	}
		
	public IEnumerator Roll()
	{
		DiceResult = UnityEngine.Random.Range(1, 7);
		DebugUtility.Log("Rolling! Result is: " + DiceResult);
		photonView.RPC("PlayRollAnimation", PhotonTargets.All, DiceResult);

		yield return new WaitForSeconds(1f);
		DiceRollEvent(DiceResult);
        yield return new WaitForSeconds(1f);
        MeshRednderer.enabled = false;
	}

	public void Reset(Transform currentTurnCharacter)
	{
        MeshRednderer.enabled = false;
        transform.position = currentTurnCharacter.position + offSet;
        transform.rotation = Quaternion.identity;
	}

	[PunRPC]
	private void PlayRollAnimation(int diceResult)
	{	
		MeshRednderer.enabled = true;
		animator.Play("Dice" + diceResult);	
	}

    public IEnumerator RollFour()
    {
        DiceResult = 4;
        DebugUtility.Log("Rolling! Result is: " + DiceResult);
        photonView.RPC("PlayRollAnimation", PhotonTargets.All, DiceResult);

        yield return new WaitForSeconds(1f);
        DiceRollEvent(DiceResult);
        yield return new WaitForSeconds(1f);
        MeshRednderer.enabled = false;
    }

    public IEnumerator RollSix()
    {
        DiceResult = 6;
        DebugUtility.Log("Rolling! Result is: " + DiceResult);
        photonView.RPC("PlayRollAnimation", PhotonTargets.All, DiceResult);

        yield return new WaitForSeconds(1f);
        DiceRollEvent(DiceResult);
        yield return new WaitForSeconds(1f);
        MeshRednderer.enabled = false;
    }
    
}
