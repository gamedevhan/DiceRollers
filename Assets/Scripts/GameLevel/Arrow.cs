using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{    
	public MeshRenderer MeshRenderer { get; private set; }
    public BoxCollider Collider { get; private set; }
    private IntersectionTile intersectionTile;
    private const int blinkCount = 3;
    private PhotonView photonView;

    private void Awake()
	{
		MeshRenderer = GetComponentInChildren<MeshRenderer>();
		MeshRenderer.enabled = false;

        intersectionTile = GetComponentInParent<IntersectionTile>();
        Collider = GetComponent<BoxCollider>();

        photonView = GetComponent<PhotonView>();
	}

	public void LookatTarget(Vector3 targetPosition)
	{
		transform.LookAt(targetPosition);
	}

	private void OnMouseDown()
	{
        photonView.RPC("OnArrowPress", PhotonTargets.All);
	}

    [PunRPC]
    public void OnArrowPress()
    {
        StartCoroutine(ArrowPressCoroutine());
    }
	
	public IEnumerator ArrowPressCoroutine()
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

        intersectionTile.OnArrowPress(this);
    }
}
