using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IngameUIManager : MonoBehaviour
{
    SessionPlayer player;

    public void Init(IngameLogicManager ingameLogicManager)
    {
        //ingameLogicManager.AddActionOnStartBattle();    // ���� ���۽� ���� ���� ��� ���� ���� �۾�
        //ingameLogicManager.AddActionOnEndBattle();      // ���� ����� ���� �˾� ���� ���� �۾�
        //ingameLogicManager.AddActionOnStartMyTurn();    // �� ���� ���۵� �� �� �� ���� Ÿ��Ʋ�� ���� ���� �۾�
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

    private void RollTheDice()
    {
        var consequnceList = new List<DiceConsequenceData>();
        player.deck.ForEach(deck =>
        {
            consequnceList.Add(new DiceConsequenceData(deck.behaviourDice.GetRandomBehaviourState(),
                                                       deck.actingPowerDice.GetRandomActingPower()));
        });
        //IngameLogicManager.instance.InvokeOnEndMyTurn(consequnceList);
        // onEndMyTurn ȣ��
    }
    #region event trigger
    public void OnClickRollTheDice()
    {
        RollTheDice();
    }

    public void OnClickTestButton_battleSkip()
    {
        IngameLogicManager.instance.InvokeOnEndBattle();
    }

    #endregion
}
