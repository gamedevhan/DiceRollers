using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	public MeshRenderer MeshRenderer { get; private set; }
	private const int blinkCount = 3;

	private void Awake()
	{
		MeshRenderer = GetComponentInChildren<MeshRenderer>();
		MeshRenderer.enabled = false;
	}

	public void LookatTarget(Vector3 targetPosition)
	{
		transform.LookAt(targetPosition);
	}

	private void OnMouseDown()
	{
		StartCoroutine(Blink());
	}
	
	public IEnumerator Blink()
	{		
		for (int i = 0; i < blinkCount; i++)
		{
			MeshRenderer.enabled = false;
			yield return new WaitForSeconds(0.25f);
			MeshRenderer.enabled = true;
			yield return new WaitForSeconds(0.25f);
			MeshRenderer.enabled = false;
			yield return new WaitForSeconds(0.25f);
		}		
	}
}
