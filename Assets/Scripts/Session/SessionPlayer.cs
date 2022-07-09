using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �α׶���ũ �� �� �ϴ� ���� �����Ǵ� ����
/// </summary>
public class SessionPlayer
{
    private static SessionPlayer _instance;
    public static SessionPlayer instance => _instance ??= new SessionPlayer();

    public List<SessionDeck> deck = new List<SessionDeck>();
    public SessionDungeon currentDungeon = new SessionDungeon();
    public SessionInventory inventory = new SessionInventory();

    /// <summary>
    /// ���� �÷��̾ �������� �ִ� ��ġ
    /// </summary>
    public int currentDugeonNodeId;
    /// <summary>
    /// ���° ��������
    /// </summary>
    public int dungeonStageLevel;

    public UnitStatusData unitData = new UnitStatusData();
    

    /// <summary>
    /// ���� �� �� ������ �� �ʱ�ȭ
    /// </summary>
    public void StartGameSession(PermanentPlayer player)
    {
        // ���� ����
        deck.Clear();
        currentDungeon = new SessionDungeon();
        inventory = new SessionInventory();

        foreach(var dice in player.startInventory.actionPowerDiceList)
        {
            inventory.actingPowerDiceList.Add(dice);
        }
        foreach (var dice in player.startInventory.behaviourDiceList)
        {
            inventory.behaviourDiceList.Add(dice);
        }


        // �ֻ��� ����
        for (int i=0; i< player.startInventory.actionPowerDiceList.Count; i++)
        {
            if (i >= player.startInventory.behaviourDiceList.Count)
                break;
            deck.Add(new SessionDeck()
            .SetActingPowerDice(player.startInventory.actionPowerDiceList[i])
            .SetBehaviourDice(player.startInventory.behaviourDiceList[i]));
        }

        // ���� ����
        currentDungeon.SetToDummy();
    }

    
}
