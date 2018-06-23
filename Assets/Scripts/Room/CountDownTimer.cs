using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
	[SerializeField]
	private float timeRemaining = 5f;
	private const float timerActivateThreshold = 0.999f;

	[SerializeField]
	private UILabel countDownText;

	private void OnEnable()
	{
		LevelTransitionManager.GameStart += OnGameStart;		
	}

	private void OnDisable()
	{
		LevelTransitionManager.GameStart -= OnGameStart;
	}
	
	private void OnGameStart()
	{
		if (!countDownText.gameObject.GetActive())
		{			
			countDownText.gameObject.SetActive(true);
		}
		timeRemaining += timerActivateThreshold;
		StartCoroutine(StartCountDown());
	}

	private IEnumerator StartCountDown()
	{
		while (timeRemaining > 0)
		{			
			timeRemaining -= Time.deltaTime;
			countDownText.text = ((int)timeRemaining).ToString();

			if (timeRemaining <= 2 - timerActivateThreshold)
			{
				countDownText.text = "Game Start";
			}

			yield return null;
		}
	}
}
