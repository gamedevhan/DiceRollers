using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{	
	// This event needs to be raised when dice is rolled
	public delegate void DiceRollEvent();
	public static event DiceRollEvent DiceRolled;
	
	public static int DiceResult;
	
	private MeshRenderer meshRenderer;
	
	private void Start()
	{			
		meshRenderer = GetComponent<MeshRenderer>();		
	}
		
	public void Roll()
	{
		DiceResult = Random.Range(1, 7);
		
		if (DiceRolled != null)
			DiceRolled();
		Debug.Log("<color=yellow>Rolling a die! Result:</color> " + DiceResult);
	}

	private void ResetDice()
	{
		meshRenderer.enabled = false;		
	}
}
