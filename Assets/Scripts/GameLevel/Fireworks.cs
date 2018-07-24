using System.Collections;
using UnityEngine;

public class Fireworks : MonoBehaviour
{
    #region FireWorksTest
    public bool Test;
    public bool instantiated = false;
    private void Update()
    {
        if (Test && instantiated)
        {
            instantiated = false;
            LaunchFireworks();
            StartCoroutine(LaunchFireworks());
        }
    }
    #endregion
    
    [SerializeField]
    private GameObject[] fireWorks;

    [PunRPC]
    private void RpcInstantiateFireworks()
    {
        StartCoroutine(LaunchFireworks());
    }

    private IEnumerator LaunchFireworks()
    {
        for (int i = 0; i < fireWorks.Length; i++)
        {
            Vector3 position = new Vector3(0, transform.position.y + 3, 0);
            Instantiate(fireWorks[i], position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
