using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ���� �������� �����Ǵ� ����
/// </summary>
public class PermanentPlayer
{
    private static PermanentPlayer _instance;
    public static PermanentPlayer instance => _instance ??= new PermanentPlayer();

    public StartInventory startInventory = new StartInventory();

    public bool initialized = false;
    public void InitializePlayer()
    {
        // ������ ó�� ������ �� �ʱ�ȭ
        initialized = true;

        // �ӽ÷� FindObjectOfType ��
        StaticDataManager staticDataManager = Object.FindObjectOfType<StaticDataManager>();

        startInventory.Clear();
        var defaultBehaviourDice = staticDataManager.GetBehaviourDice(x => x.isDefaultDice);
        var actingPowerDice = staticDataManager.GetActionPowerDice(x => x.isDefaultDice);


        startInventory.currentDeckList.Add(new SessionDeck().SetBehaviourDice(defaultBehaviourDice)
                                                            .SetActingPowerDice(actingPowerDice));
        startInventory.currentDeckList.Add(new SessionDeck().SetBehaviourDice(defaultBehaviourDice)
                                                            .SetActingPowerDice(actingPowerDice));
        startInventory.currentDeckList.Add(new SessionDeck().SetBehaviourDice(defaultBehaviourDice)
                                                            .SetActingPowerDice(actingPowerDice));
    }
}
