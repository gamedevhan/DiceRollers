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
        Camera camera = Camera.main;
        for (int i = 0; i < fireWorks.Length; i++)
        {
            // Position is hard coded, need to find better way
            Vector3 position = new Vector3(GetComponent<Tile>().transform.position.x + 2 * i - 3, 3, GetComponent<Tile>().transform.position.z + 1);
            Instantiate(fireWorks[i], position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
