using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 로그라이크 한 판 하는 동안 유지되는 정보
/// </summary>
public class SessionPlayer
{
    private static SessionPlayer _instance;
    public static SessionPlayer instance => _instance ??= new SessionPlayer();

    public List<SessionDeck> deck = new List<SessionDeck>();
    public SessionDungeon currentDungeon = new SessionDungeon();
    public SessionInventory inventory = new SessionInventory();

    /// <summary>
    /// 현재 플레이어가 던전에서 있는 위치
    /// </summary>
    public int currentDugeonNodeId;
    /// <summary>
    /// 몇번째 던전인지
    /// </summary>
    public int dungeonStageLevel;

    public UnitStatusData unitData = new UnitStatusData();
    

    /// <summary>
    /// 게임 한 판 시작할 때 초기화
    /// </summary>
    public void StartGameSession(PermanentPlayer player)
    {
        // 대충 시작
        deck.Clear();
        currentDungeon = new SessionDungeon();
        inventory = new SessionInventory();

        //foreach(var dice in player.startInventory.actionPowerDiceList)
        //{
        //    inventory.actingPowerDiceList.Add(dice);
        //}
        //foreach (var dice in player.startInventory.behaviourDiceList)
        //{
        //    inventory.behaviourDiceList.Add(dice);
        //}
        foreach (var currentDeck in player.startInventory.currentDeckList)
        {
            //inventory.actingPowerDiceList.Add(currentDeck.actingPowerDice);
            //inventory.behaviourDiceList.Add(currentDeck.behaviourDice);
            deck.Add(new SessionDeck().SetBehaviourDice(currentDeck.behaviourDice)
                                      .SetActingPowerDice(currentDeck.actingPowerDice));
        }

        //// 주사위 세팅
        //for (int i=0; i< player.startInventory.actionPowerDiceList.Count; i++)
        //{
        //    if (i >= player.startInventory.behaviourDiceList.Count)
        //        break;
        //    deck.Add(new SessionDeck()
        //    .SetActingPowerDice(player.startInventory.actionPowerDiceList[i])
        //    .SetBehaviourDice(player.startInventory.behaviourDiceList[i]));
        //}

        // 던전 세팅
        currentDungeon.SetToDummy();
    }

    
}
