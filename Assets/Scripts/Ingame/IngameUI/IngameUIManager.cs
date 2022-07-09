using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IngameUIManager : MonoBehaviour
{
    SessionPlayer player;

    public void Init(IngameLogicManager ingameLogicManager)
    {
        //ingameLogicManager.AddActionOnStartBattle();    // 전투 시작시 전투 시작 뷰어 띄우기 등의 작업
        //ingameLogicManager.AddActionOnEndBattle();      // 전투 종료시 보상 팝업 띄우기 등의 작업
        //ingameLogicManager.AddActionOnStartMyTurn();    // 내 턴이 시작될 때 내 턴 시작 타이틀을 띄우는 등의 작업
        //ingameLogicManager.AddActionOnEndMyTurn();      // 내 턴에서 어떤 주사위 결과가 나왔는지 팝업 띄우기 등의 작업
        //ingameLogicManager.AddActionOnStartEnemyTurn(); // 적 턴이 시작될 때 적 턴 시작 타이틀을 띄우는 등의 작업
        //ingameLogicManager.AddActionOnEndEnemyTurn();   // 적 턴에서 어떤 주사위 결과가 나왔는지 팝업 띄우기 등의 작업
    }

    public IngameUIManager SetPlayer(SessionPlayer player)
    {
        this.player = player;
        return this;
    }

    public void OnClickRollTheDice()
    {
        RollTheDice();
    }

    private void RollTheDice()
    {
        var consequnceList = new List<DiceConsequenceData>();
        player.deck.ForEach(deck =>
        {
            consequnceList.Add(new DiceConsequenceData(deck.behaviourDice.GetRandomBehaviourState(),
                                                       deck.actingPowerDice.GetRandomActingPower()));
        });
        IngameLogicManager.instance.InvokeOnEndMyTurn(consequnceList);
        // onEndMyTurn 호출
    }

    
}
