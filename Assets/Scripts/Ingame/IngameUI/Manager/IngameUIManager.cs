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

        //ingameLogicManager.AddActionOnStartBattle();    // 전투 시작시 전투 시작 뷰어 띄우기 등의 작업
        //ingameLogicManager.AddActionOnEndBattle();      // 전투 종료시 보상 팝업 띄우기 등의 작업


        ingameLogicManager.AddActionOnStartTurn(OnStartTurn);    // 턴이 시작될 때 턴 시작 타이틀을 띄우는 등의 작업
        ingameLogicManager.AddActionOnRollCompleteTurn(OnRollCompleteTurn); // 주사위 굴리는 연출 작업
        ingameLogicManager.AddActionOnEndTurn(OnEndTurn); // 어떤 주사위 결과가 나왔는지 팝업 띄우기 등의 작업
        //ingameLogicManager.AddActionOnEndMyTurn();      // 내 턴에서 어떤 주사위 결과가 나왔는지 팝업 띄우기 등의 작업
        //ingameLogicManager.AddActionOnStartEnemyTurn(); // 적 턴이 시작될 때 적 턴 시작 타이틀을 띄우는 등의 작업
        //ingameLogicManager.AddActionOnEndEnemyTurn();   // 적 턴에서 어떤 주사위 결과가 나왔는지 팝업 띄우기 등의 작업
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
        // 대충 굴리는 연출

        yield return null;

        // 연출 완료
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
