using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 한 판이 끝나더라도 유지되는 정보
/// </summary>
public class PermanentPlayer
{
    private static PermanentPlayer _instance;
    public static PermanentPlayer instance => _instance ??= new PermanentPlayer();

    public StartInventory startInventory = new StartInventory();

    public bool initialized = false;
    public void InitializePlayer()
    {
        // 게임을 처음 시작할 때 초기화
        initialized = true;

        // 임시로 FindObjectOfType 씀
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
