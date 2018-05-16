using UnityEngine;

public class Dice : MonoBehaviour
{	
	// This event needs to be raised when dice is rolled
	public delegate void DiceRollEvent();
	public static event DiceRollEvent DiceRolled;
	
	public static int DiceResult;

	// For Test
	public void DiceButton()
	{
		DiceResult = Roll();
	}

	public int Roll()
	{	
		int diceResult = Random.Range(1, 7);
		Debug.Log("<color=yellow>Rolling a die! Result:</color> " + diceResult);

		if (DiceRolled != null)
			DiceRolled();
		return diceResult;
	}
}
