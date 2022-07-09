using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    SessionPlayer player;

    [Header("UI inspector")]
    [SerializeField] Button rollButton;
    [SerializeField] IngameRolledInfo[] rolledInfos;

    private bool waiting = false;

    public void Init(IngameLogicManager ingameLogicManager)
    {
        if (rollButton != null)
            rollButton.gameObject.SetActive(false);

        foreach(var rolledInfo in rolledInfos)
        {
            rolledInfo.SetToEmpty();
        }

        ingameLogicManager.isEffectPhase += () => waiting;

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

    IEnumerator ShowRollDiceList(List<DiceConsequenceData> list, Action onComplete)
    {
        foreach (var rolledInfo in rolledInfos)
        {
            rolledInfo.SetToEmpty();
        }

        yield return null;
        // ���� ������ ����

        yield return null;

        // ���� �Ϸ�
        for (int i = 0; i < rolledInfos.Length; i++)
        {
            if (i < list.Count)
            {
                rolledInfos[i].SetDice(list[i]);
            }
            else
            {
                rolledInfos[i].SetToEmpty();
            }
        }
        onComplete.Invoke();
    }

    #region callback
    private void OnStartTurn(UnitStatusData unit)
    {
        if (rollButton != null)
            rollButton.gameObject.SetActive(true);
    }
    private void OnRollCompleteTurn(UnitStatusData unit, List<DiceConsequenceData> list)
    {
        if (rollButton != null)
            rollButton.gameObject.SetActive(false);
        StartCoroutine(ShowRollDiceList(list, OnCompleteMyRollEffect));
    }
    private void OnCompleteMyRollEffect()
    {

    }

    private void OnEndTurn(UnitStatusData unit)
    {
        //rollButton.gameObject.SetActive(true);
    }
    private void OnRollCompleteEnemyTurn(UnitStatusData unit, List<DiceConsequenceData> list)
    {
        if (rollButton != null)
            rollButton.gameObject.SetActive(false);
        StartCoroutine(ShowRollDiceList(list, OnCompleteEnemyRollEffect));
    }
    private void OnCompleteEnemyRollEffect()
    {

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
