using UnityEngine;

public class CameraController : MonoBehaviour
{	
	public Transform FollowTarget { get; set; }

	private Vector3 offset;
	private Camera mainCamera;

	private void Awake()
	{
		mainCamera = Camera.main;
	}

	private void Start()
	{
		offset = mainCamera.transform.position;
	}

	private void LateUpdate()
	{
		if (FollowTarget != null)
		{
			mainCamera.transform.position = FollowTarget.position + offset;
		}
	}
}
