using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticS3MonsterData", menuName = "StaticData/Create StaticS3MonsterData")]
[Serializable]
public class StaticS3MonsterData : StaticS3DataBase<S3MonsterData>
{

}

[Serializable]
public class S3MonsterData : S3DataBase
{
    public int index;
    public string monsterName;
    public int hitpoint;
    public int behaviourDice_1;
    public int behaviourDice_2;
    public int behaviourDice_3;
    public int behaviourDice_4;
    public int behaviourDice_5;
    public int actingPowerDice_1;
    public int actingPowerDice_2;
    public int actingPowerDice_3;
    public int actingPowerDice_4;
    public int actingPowerDice_5;

    public override void InitData(string[] data)
    {
        var i = 0;
        index = int.Parse(data[i++]);
        monsterName = data[i++];
        hitpoint = int.Parse(data[i++]);
        behaviourDice_1 = int.Parse(data[i++]);
        behaviourDice_2 = int.Parse(data[i++]);
        behaviourDice_3 = int.Parse(data[i++]);
        behaviourDice_4 = int.Parse(data[i++]);
        behaviourDice_5 = int.Parse(data[i++]);
        actingPowerDice_1 = int.Parse(data[i++]);
        actingPowerDice_2 = int.Parse(data[i++]);
        actingPowerDice_3 = int.Parse(data[i++]);
        actingPowerDice_4 = int.Parse(data[i++]);
        actingPowerDice_5 = int.Parse(data[i++]);
    }
}
