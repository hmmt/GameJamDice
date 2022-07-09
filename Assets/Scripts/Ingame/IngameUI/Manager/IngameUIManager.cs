using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    SessionPlayer player;

    [Header("UI inspector")]
    [SerializeField] UIButton rollButton;
    [SerializeField] IngameRolledInfo[] rolledInfos;
    [SerializeField] DiceAnimatator diceAnimator;

    public bool waiting { private set; get; }

    private void Awake()
    {
        foreach (var rolledInfo in rolledInfos)
        {
            rolledInfo.SetToEmpty();
        }
    }
    public void Init(IngameLogicManager ingameLogicManager)
    {
        if (rollButton != null)
            rollButton.gameObject.SetActive(false);

        foreach(var rolledInfo in rolledInfos)
        {
            rolledInfo.SetToEmpty();
        }

        //ingameLogicManager.AddActionOnStartBattle();    // ���� ���۽� ���� ���� ��� ���� ���� �۾�
        //ingameLogicManager.AddActionOnEndBattle();      // ���� ����� ���� �˾� ���� ���� �۾�


        ingameLogicManager.AddActionOnStartTurn(OnStartTurn);    // ���� ���۵� �� �� ���� Ÿ��Ʋ�� ���� ���� �۾�
        ingameLogicManager.AddActionOnRollCompleteTurn(OnRollCompleteTurn); // �ֻ��� ������ ���� �۾�
        ingameLogicManager.AddActionOnEndTurn(OnEndTurn); // � �ֻ��� ����� ���Դ��� �˾� ���� ���� �۾�
        //ingameLogicManager.AddActionOnEndMyTurn();      // �� �Ͽ��� � �ֻ��� ����� ���Դ��� �˾� ���� ���� �۾�
        //ingameLogicManager.AddActionOnStartEnemyTurn(); // �� ���� ���۵� �� �� �� ���� Ÿ��Ʋ�� ���� ���� �۾�
        //ingameLogicManager.AddActionOnEndEnemyTurn();   // �� �Ͽ��� � �ֻ��� ����� ���Դ��� �˾� ���� ���� �۾�
    }

    public void Clear()
    {

    }

    public IngameUIManager SetPlayer(SessionPlayer player)
    {
        this.player = player;
        return this;
    }

    IEnumerator ShowRollDice(DiceConsequenceData dcdata, int diceIndex, Action onComplete)
    {
        // ���� ������ ����
        waiting = true;
        yield return null;

        diceAnimator.PlayAnimation(dcdata.behaviourState, dcdata.actingPower, diceIndex, delegate ()
        {
            waiting = false;
            try
            {
                rolledInfos[diceIndex].SetDice(dcdata);
            }
            catch(Exception e)
            {

            }
        });

    }

#region callback
    private void OnStartTurn(UnitStatusData unit)
    {
        if (rollButton != null)
            rollButton.gameObject.SetActive(true);

        for (int i = 0; i < rolledInfos.Length; i++)
        {
            rolledInfos[i].SetToEmpty();
        }
    }
    private void OnRollCompleteTurn(UnitStatusData unit, DiceConsequenceData dcdata, int diceIndex)
    {
        if (rollButton != null)
            rollButton.gameObject.SetActive(false);
        StartCoroutine(ShowRollDice(dcdata, diceIndex, OnCompleteMyRollEffect));
    }
    private void OnCompleteMyRollEffect()
    {

    }

    private void OnEndTurn(UnitStatusData unit)
    {
        //rollButton.gameObject.SetActive(true);
    }
#endregion


#region event trigger
    public void OnClickRollTheDice()
    {
        IngameLogicManager.instance.RollMyDice();
    }

    public void OnClickTestButton_battleSkip()
    {
    }

#endregion
}
