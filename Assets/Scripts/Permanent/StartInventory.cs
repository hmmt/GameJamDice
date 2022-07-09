using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시작 덱
/// 
/// 후에 다른 덱(세션 덱 등) 클래스와 합쳐야 할 수도 있음
/// </summary>
public class StartInventory
{
    // 귀찮으니 일단 public
    public List<S3BehaviourDice> behaviourDiceList = new List<S3BehaviourDice>();
    public List<S3ActingPowerDice> actionPowerDiceList = new List<S3ActingPowerDice>();

    public void Clear()
    {
        behaviourDiceList.Clear();
        actionPowerDiceList.Clear();
    }
}
