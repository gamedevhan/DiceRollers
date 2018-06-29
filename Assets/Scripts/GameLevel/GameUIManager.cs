using UnityEngine;

public class GameUIManager : MonoBehaviour
{
	[SerializeField]
	private GameObject RollButton;

	public static GameUIManager Instance = null;

	private void Awake()
	{
		if (Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	private void OnEnable()
	{
		PhotonNetwork.OnEventCall += OnTurnBegin;
	}

	private void OnDisable()
	{
		PhotonNetwork.OnEventCall -= OnTurnBegin;
	}

	public void Roll()
	{
		StartCoroutine(Dice.Instance.Roll());
		RollButton.SetActive(false);
	}

	private void OnTurnBegin(byte eventcode, object content, int senderid)
	{
		if (eventcode != PhotonEventCode.TurnBegin)
			return;

		if ((int)content != PhotonNetwork.player.ID && RollButton.activeInHierarchy)
		{
			RollButton.SetActive(false);
		}
		else if (!RollButton.activeInHierarchy)
		{
			RollButton.SetActive(true);
		}
	}
}
