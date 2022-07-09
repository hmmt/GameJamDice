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

        startInventory.behaviourDiceList.Add(staticDataManager.GetBehaviourDice(x => x.index == 0));
        startInventory.behaviourDiceList.Add(staticDataManager.GetBehaviourDice(x => x.index == 1));
        startInventory.behaviourDiceList.Add(staticDataManager.GetBehaviourDice(x => x.index == 2));

        startInventory.actionPowerDiceList.Add(staticDataManager.GetS3Data<StaticS3ActingPowerDiceData>().datas.Find(x => x.index == 0));
        startInventory.actionPowerDiceList.Add(staticDataManager.GetS3Data<StaticS3ActingPowerDiceData>().datas.Find(x => x.index == 0));
        startInventory.actionPowerDiceList.Add(staticDataManager.GetS3Data<StaticS3ActingPowerDiceData>().datas.Find(x => x.index == 0));
    }
}
