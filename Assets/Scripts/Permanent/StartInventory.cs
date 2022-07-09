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
    public List<SessionDeck> currentDeckList = new List<SessionDeck>();
    //public List<S3BehaviourDiceData> behaviourDiceList = new List<S3BehaviourDiceData>();
    //public List<S3ActingPowerDiceData> actionPowerDiceList = new List<S3ActingPowerDiceData>();

    public void Clear()
    {
        //behaviourDiceList.Clear();
        //actionPowerDiceList.Clear();
    }
}
