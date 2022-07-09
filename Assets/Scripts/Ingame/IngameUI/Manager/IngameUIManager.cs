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

    IEnumerator ShowRollDice(DiceConsequenceData dcdata, int diceIndex, Action onComplete)
    {
        // 대충 굴리는 연출
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
