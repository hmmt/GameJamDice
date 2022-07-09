using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticS3ActingPowerDice", menuName = "StaticData/Create StaticS3ActingPowerDice")]
[Serializable]
public class StaticS3ActingPowerDice : StaticS3DataBase<S3ActingPowerDice>
{

}

[Serializable]
public class S3ActingPowerDice : S3DataBase
{
    public int index;
    public bool isDefaultDice;
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
        diceFace_1 = int.Parse(data[i++]);
        diceFace_2 = int.Parse(data[i++]);
        diceFace_3 = int.Parse(data[i++]);
        diceFace_4 = int.Parse(data[i++]);
        diceFace_5 = int.Parse(data[i++]);
        diceFace_6 = int.Parse(data[i++]);
        shopPrice = int.Parse(data[i++]);
    }
}