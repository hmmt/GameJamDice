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
        rollButton.gameObject.SetActive(false);

        foreach(var rolledInfo in rolledInfos)
        {
            rolledInfo.SetToEmpty();
        }

        ingameLogicManager.isEffectPhase += () => waiting;

        //ingameLogicManager.AddActionOnStartBattle();    // ���� ���۽� ���� ���� ��� ���� ���� �۾�
        //ingameLogicManager.AddActionOnEndBattle();      // ���� ����� ���� �˾� ���� ���� �۾�


        ingameLogicManager.AddActionOnStartMyTurn(OnStartMyTurn);    // �� ���� ���۵� �� �� �� ���� Ÿ��Ʋ�� ���� ���� �۾�
        ingameLogicManager.AddActionOnRollCompleteMyTurn(OnRollCompleteMyTurn);
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
    private void OnStartMyTurn()
    {
        rollButton.gameObject.SetActive(true);
    }
    private void OnRollCompleteMyTurn(List<DiceConsequenceData> list)
    {
        rollButton.gameObject.SetActive(false);
        StartCoroutine(ShowRollDiceList(list, OnCompleteMyRollEffect));
    }
    private void OnCompleteMyRollEffect()
    {

    }

    private void OnStartEnemyTurn()
    {
        //rollButton.gameObject.SetActive(true);
    }
    private void OnRollCompleteEnemyTurn(UnitStatusData unit, List<DiceConsequenceData> list)
    {
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
