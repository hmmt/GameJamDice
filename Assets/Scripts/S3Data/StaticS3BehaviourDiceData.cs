using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticS3BehaviourDice", menuName = "StaticData/Create StaticS3BehaviourDice")]
[Serializable]
public class StaticS3BehaviourDiceData : StaticS3DataBase<S3BehaviourDiceData>
{

}

[Serializable]
public class S3BehaviourDiceData : S3DataBase
{
    public int index;
    public bool isDefaultDice;
    public DiceCategory diceCategory;
    public BehaviourState diceFace_1;
    public BehaviourState diceFace_2;
    public BehaviourState diceFace_3;
    public BehaviourState diceFace_4;
    public BehaviourState diceFace_5;
    public BehaviourState diceFace_6;
    public int shopPrice;

    public override void InitData(string[] data)
    {
        var i = 0;
        index = int.Parse(data[i++]);
        isDefaultDice = bool.Parse(data[i++]);
        diceCategory = data[i++].ToEnum<DiceCategory>();
        diceFace_1 = data[i++].ToEnum<BehaviourState>();
        diceFace_2 = data[i++].ToEnum<BehaviourState>();
        diceFace_3 = data[i++].ToEnum<BehaviourState>();
        diceFace_4 = data[i++].ToEnum<BehaviourState>();
        diceFace_5 = data[i++].ToEnum<BehaviourState>();
        diceFace_6 = data[i++].ToEnum<BehaviourState>();
        shopPrice = int.Parse(data[i++]);
    }
}

