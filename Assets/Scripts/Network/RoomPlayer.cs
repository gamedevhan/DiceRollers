using UnityEngine;

public class RoomPlayer : MonoBehaviour
{	
	public GameObject[] Characters;
	public PhotonPlayer PhotonPlayer { get; private set; }	
	public UILabel PlayerName;

	[SerializeField] private Transform charactersParent;

	private int currentCharacter = 0;

	private void Start()
	{	
		FaceCamera();
	}

	public void OnLeftButtonClick()
	{
		if (currentCharacter != 0) { currentCharacter--; }
		else currentCharacter = Characters.Length - 1;
		
		DisplayCurrnetCharacter();		
	}

	public void OnRightButtonClick()
	{
		if (currentCharacter != Characters.Length - 1) { currentCharacter++; }
		else currentCharacter = 0;
		
		DisplayCurrnetCharacter();
	}

	private void DisplayCurrnetCharacter()
	{
		foreach (Transform character in charactersParent)
		{
			character.gameObject.SetActive(false);
		}

		Characters[currentCharacter].SetActive(true);		
	}

	private void FaceCamera()
	{
		Camera camera = Camera.main;
		Vector3 targetPosition = new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z);
		charactersParent.LookAt(targetPosition);
	}
	
	public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
	{
		PlayerName.text = photonPlayer.NickName;
	}
}
