using System.Collections;
using UnityEngine;

public class GameLevelUIManager : MonoBehaviour
{
    public UIButton RollButton;        

    [SerializeField]
    private UILabel currentTurnPlayerLabel;
    private const float turnPlayerLabelHideDelay = 1.5f;
    private Dice dice;

    private void Start()
    {
        dice = GameManager.Instance.Dice;
        RollButton.gameObject.SetActive(false);        
        currentTurnPlayerLabel.gameObject.SetActive(false);
    }

    [PunRPC]
    public void RpcCurrentTurnPlayerLabel(string currentTurnPlayerName)
    {
        StartCoroutine(ShowCurrentTurnPlayerLabel(currentTurnPlayerName));
    }

    private IEnumerator ShowCurrentTurnPlayerLabel(string currentTurnPlayerName)
    {
        currentTurnPlayerLabel.text = currentTurnPlayerName + "'s Turn";
        currentTurnPlayerLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(turnPlayerLabelHideDelay);
        currentTurnPlayerLabel.gameObject.SetActive(false);
    }

    public void ActivateRollButton()
    {
        RollButton.gameObject.SetActive(true);
    }

    public void DeactivateRollButton()
    {
        RollButton.gameObject.SetActive(false);
    }

    public void OnRollButtonPress()
    {
        DeactivateRollButton();
        dice.MeshRednderer.enabled = true;
        StartCoroutine(dice.Roll());
    }

    #region Debug Buttons

    public void OnRoll4Press()
    {
        DeactivateRollButton();        
        dice.MeshRednderer.enabled = true;
        StartCoroutine(dice.RollFour());
    }

    public void OnRoll6Press()
    {
        DeactivateRollButton();        
        dice.MeshRednderer.enabled = true;
        StartCoroutine(dice.RollSix());
    }

    #endregion
}
