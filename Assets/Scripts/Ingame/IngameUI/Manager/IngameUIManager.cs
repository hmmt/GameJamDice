using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    public enum Turn
    {
        None,
        MyTurn,
        OpponentTurn,
    }


    SessionPlayer player;

    [Header("UI inspector")]
    [SerializeField] UIButton rollButton;
    [SerializeField] IngameRolledInfo[] rolledInfos;
    [SerializeField] DiceAnimatator diceAnimator;

    [Header("turn alram")]
    [SerializeField] GameObject alaramRoot;
    [SerializeField] GameObject myTurnImage;
    [SerializeField] GameObject opponentTurnImage;
    [SerializeField] GameObject winImage;
    [SerializeField] GameObject loseImage;


    private Turn _lastTurn = Turn.None;

    #region selection
    private int _selectedIndex = -1;
    #endregion

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
        waiting = false;
        _selectedIndex = -1;
        if (rollButton != null)
            rollButton.gameObject.SetActive(false);

        foreach(var rolledInfo in rolledInfos)
        {
            rolledInfo.SetToEmpty();
        }

        ingameLogicManager.AddActionOnStartBattle(OnStartBattle);    // 전투 시작시 전투 시작 뷰어 띄우기 등의 작업
        ingameLogicManager.AddActionOnEndBattle(OnEndBattle);      // 전투 종료시 보상 팝업 띄우기 등의 작업


        ingameLogicManager.AddActionOnStartTurn(OnStartTurn);    // 턴이 시작될 때 턴 시작 타이틀을 띄우는 등의 작업
        ingameLogicManager.AddActionOnRollCompleteTurn(OnRollCompleteTurn); // 주사위 굴리는 연출 작업
        ingameLogicManager.AddActionOnUseDice(OnUseDice);
        ingameLogicManager.AddActionOnEndTurn(OnEndTurn); // 어떤 주사위 결과가 나왔는지 팝업 띄우기 등의 작업

        ingameLogicManager.AddActionOnSelectDiceSlot(OnSelectDice);

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

    IEnumerator ShowRollDice(DiceConsequenceData dcdata, int diceIndex)
    {
        // 대충 굴리는 연출
        waiting = true;
        yield return null;

        diceAnimator.PlayAnimation(dcdata.behaviourState, dcdata.actingPower, diceIndex, delegate ()
        {
            waiting = false;
            try
            {
                rolledInfos[diceIndex].SetDice(dcdata,
                    delegate ()
                    {
                        IngameLogicManager.instance.SelectDiceSlot(diceIndex);
                    });

            }
            catch (Exception e)
            {

            }
        });

    }

#region callback
    private void OnStartBattle()
    {
        _lastTurn = Turn.None;
    }
    private void OnStartTurn(UnitStatusData unit)
    {
        if (rollButton != null)
            rollButton.gameObject.SetActive(true);

        for (int i = 0; i < rolledInfos.Length; i++)
        {
            rolledInfos[i].SetToEmpty();
        }
        Turn turn = unit.isPlayer ? Turn.MyTurn : Turn.OpponentTurn;

        if (turn != _lastTurn)
        {
            StartCoroutine(StartPopup(turn == Turn.MyTurn ? myTurnImage : opponentTurnImage));
        }

        _lastTurn = turn;
    }
    IEnumerator StartPopup(GameObject targetPopupImg)
    {
        myTurnImage.SetActive(false);
        opponentTurnImage.SetActive(false);
        winImage.SetActive(false);
        loseImage.SetActive(false);

        alaramRoot.SetActive(true);
        targetPopupImg.SetActive(true);

        waiting = true;
        // GameSessionManager의 WaitAfterClear 대기시간보다 짧아야 함
        yield return new WaitForSeconds(1.8f);

        waiting = false;
        alaramRoot.SetActive(false);
    }

    private void OnEndBattle(bool b)
    {
        StartCoroutine(StartPopup(b ? winImage : loseImage));
    }
    private void OnRollCompleteTurn(UnitStatusData unit, DiceConsequenceData dcdata, int diceIndex)
    {
        StartCoroutine(ShowRollDice(dcdata, diceIndex));
    }

    private void OnSelectDice(int diceIndex)
    {
        foreach(var r in rolledInfos)
        {
            r.SetSelected(false);
        }

        if(diceIndex >= 0 && diceIndex < rolledInfos.Length)
        {
            rolledInfos[diceIndex].SetSelected(true);
        }
    }
    private void OnUseDice(UnitStatusData unit, DiceConsequenceData diceData, int diceIndex, List<ActionResultData> resultList)
    {
        foreach (var r in rolledInfos)
        {
            r.SetSelected(false);
        }
        try
        {
            rolledInfos[diceIndex].SetToUsed();
        }
        catch(Exception e)
        {

        }
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
