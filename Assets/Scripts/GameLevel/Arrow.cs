using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public bool IsActive { get; private set; }
    public MeshRenderer MeshRenderer { get; private set; }
    public BoxCollider Collider { get; private set; }
    public IntersectionTile IntersectionTile { get; set; }    
    public PhotonView PhotonView { get; private set; }
    public Tile PointingTile { get; set; }

    //private bool hasPressed;
    //private bool HasPressed
    //{
    //    get
    //    {
    //        return hasPressed;
    //    }
    //    set
    //    {
    //        hasPressed = HasPressed;
    //        if (hasPressed)
    //        {
    //            IntersectionTile.OnArrowPress(this);
    //        }
    //    }
    //}
        
    private const int blinkCount = 3;

    private void Awake()
    {
        MeshRenderer = GetComponentInChildren<MeshRenderer>();
        Collider = GetComponent<BoxCollider>();
        PhotonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        SetActivte(false);
    }

    private void OnMouseDown()
    {
        PhotonView.RPC("RpcBlinkArrow", PhotonTargets.All);
    }

    public void SetActivte(bool isActive = true)
    {
        MeshRenderer.enabled = isActive;
        Collider.enabled = isActive;
        IsActive = isActive;
    }

    [PunRPC]
    public void RpcBlinkArrow()
    {
        StartCoroutine(BlinkArrow());
    }

    public IEnumerator BlinkArrow()
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

        if (PhotonNetwork.player.ID == GameManager.Instance.TurnManager.CurrentTurnPlayerID)
        {
            //HasPressed = true;
            IntersectionTile.OnArrowPress(PhotonView.viewID);
        }
    }
}