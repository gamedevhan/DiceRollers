using System;
using System.Collections;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
	public bool HideAfterFinish = true;
	public float TimeLeft = 5f;
	public float TimerActivateThreshold = 0.9f;
	public UILabel countDownTimer;	
	public string FinishText = "Game Start!";

	[SerializeField]
	private GameObject timerGameObject;
		
	public static event Action CountDownFinish = delegate{ };

	private void Start()
	{	
		timerGameObject.SetActive(false);
	}
	
	[PunRPC]
	private void StartCountDown()
	{
		StartCoroutine(CountDownStart());
	}

	private IEnumerator CountDownStart()
	{
		if (!timerGameObject.activeInHierarchy)
		{
			timerGameObject.SetActive(true);
		}

		TimeLeft += TimerActivateThreshold;

		while (TimeLeft > 0)
		{
			TimeLeft -= Time.deltaTime;
			
			countDownTimer.text = ((int)TimeLeft).ToString();

			if (HideAfterFinish && TimeLeft <= 2 - TimerActivateThreshold)
			{
				countDownTimer.text = FinishText;
			}

			if (HideAfterFinish && TimeLeft <= 1 - TimerActivateThreshold)
			{
				timerGameObject.SetActive(false);
			}

			yield return null;
		}

		CountDownFinish();
	}
}