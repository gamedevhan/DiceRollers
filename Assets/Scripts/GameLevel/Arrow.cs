using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	public MeshRenderer MeshRenderer { get; private set; }

	private void Awake()
	{
		MeshRenderer = GetComponentInChildren<MeshRenderer>();
		MeshRenderer.enabled = false;
	}

	public void LookatTarget(Vector3 targetPosition)
	{
		transform.LookAt(targetPosition);
	}

	public Arrow OnArrowPressed()
	{
		StartCoroutine(Blink());
		return this;
	}

	public IEnumerator Blink()
	{		
		MeshRenderer.enabled = false;
		yield return new WaitForSeconds(0.25f);
		MeshRenderer.enabled = true;
		yield return new WaitForSeconds(0.25f);
		MeshRenderer.enabled = false;
		yield return new WaitForSeconds(0.25f);
	}
}
