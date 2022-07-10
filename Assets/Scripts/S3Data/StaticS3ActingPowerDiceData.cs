using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticS3ActingPowerDice", menuName = "StaticData/Create StaticS3ActingPowerDice")]
[Serializable]
public class StaticS3ActingPowerDiceData : StaticS3DataBase<S3ActingPowerDiceData>
{

}

[Serializable]
public class S3ActingPowerDiceData : S3DataBase
{
    public int index;
    public bool isDefaultDice;
    public DiceCategory diceCategory;
    public int diceFace_1;
    public int diceFace_2;
    public int diceFace_3;
    public int diceFace_4;
    public int diceFace_5;
    public int diceFace_6;
    public int shopPrice;

    public override void InitData(string[] data)
    {
        var i = 0;
        index = int.Parse(data[i++]);
        isDefaultDice = bool.Parse(data[i++]);
        diceCategory = data[i++].ToEnum<DiceCategory>();
        diceFace_1 = int.Parse(data[i++]);
        diceFace_2 = int.Parse(data[i++]);
        diceFace_3 = int.Parse(data[i++]);
        diceFace_4 = int.Parse(data[i++]);
        diceFace_5 = int.Parse(data[i++]);
        diceFace_6 = int.Parse(data[i++]);
        shopPrice = int.Parse(data[i++]);
    }

    public S3ActingPowerDiceData ToMemberWiseClone()
        => MemberwiseClone() as S3ActingPowerDiceData;
}