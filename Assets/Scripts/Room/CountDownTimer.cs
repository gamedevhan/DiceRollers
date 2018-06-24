using System;
using System.Collections;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
	public bool HideAfterFinish = true;
	public float TimeLeft = 10f;
	public float TimerActivateThreshold = 0.99f;
	[Tooltip("Sprite to display every second, set this 1 higher than TimeLeft if you want to display a image at 0 second")]
	public Sprite[] Sprites;

	[SerializeField]
	private UI2DSprite timerImage;
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

			int spriteCount = Sprites.Length;

			if ((int)TimeLeft > spriteCount)
			{
				timerImage.sprite2D = Sprites[spriteCount - 1];
			}
			else if ((int)TimeLeft <= 0)
			{
				timerImage.sprite2D = Sprites[0];
			}
			else
			{
				timerImage.sprite2D = Sprites[(int)TimeLeft];
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