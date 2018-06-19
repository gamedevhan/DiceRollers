using UnityEngine;

public class LevelTransitionManager : MonoBehaviour
{
	public int SelectedCharacterIndex = 0;
	public string[] LevelNames;

	public static LevelTransitionManager Instance = null;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;			
		}
		else
		{
			DestroyGameObject();
		}
	}

	public void DestroyGameObject()
	{
		Destroy(gameObject);
	}
}
