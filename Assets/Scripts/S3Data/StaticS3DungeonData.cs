using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticS3DungeonData", menuName = "StaticData/Create StaticS3DungeonData")]
public class StaticS3DungeonData : StaticS3DataBase<S3DungeonData>
{
    
}

[Serializable]
public class S3DungeonData : S3DataBase
{
    public int index;
    public List<int> monsterIndexList;
    public List<int> behaviourDiceRewardList;
    public List<int> actingPowerDiceRewardList;

    public override void InitData(string[] data)
    {
        monsterIndexList = new List<int>();
        behaviourDiceRewardList = new List<int>();
        actingPowerDiceRewardList = new List<int>();

        var i = 0;
        index = int.Parse(data[i++]);
        var monsterIndexListData = data[i++].Split(',');
        for (int n = 0; n < monsterIndexListData.Length; n++)
        {
            var targetData = int.Parse(monsterIndexListData[n]);
            monsterIndexList.Add(targetData);
        }
        var behaviourDiceRewardListData = data[i++].Split(',');
        for (int n = 0; n < behaviourDiceRewardListData.Length; n++)
        {
            var targetData = int.Parse(behaviourDiceRewardListData[n]);
            behaviourDiceRewardList.Add(targetData);
        }
        var actingPowerDiceRewardListData = data[i++].Split(',');
        for (int n = 0; n < actingPowerDiceRewardListData.Length; n++)
        {
            var targetData = int.Parse(actingPowerDiceRewardListData[n]);
            actingPowerDiceRewardList.Add(targetData);
        }
    }
}
