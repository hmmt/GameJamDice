using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IngameUtility
{
    public static BehaviourState GetRandomBehaviourState(this S3BehaviourDice s3Dice)
    {
        var diceCount = 6;
        var randomValue = Random.Range(0, diceCount);
        switch (randomValue)
        {
            case 0: return s3Dice.diceFace_1;
            case 1: return s3Dice.diceFace_2;
            case 2: return s3Dice.diceFace_3;
            case 3: return s3Dice.diceFace_4;
            case 4: return s3Dice.diceFace_5;
            case 5: return s3Dice.diceFace_6;
            default: return s3Dice.diceFace_1;
        }
    }

    public static int GetRandomActingPower(this S3ActingPowerDice s3Dice)
    {
        var diceCount = 6;
        var randomValue = Random.Range(0, diceCount);
        switch (randomValue)
        {
            case 0: return s3Dice.diceFace_1;
            case 1: return s3Dice.diceFace_2;
            case 2: return s3Dice.diceFace_3;
            case 3: return s3Dice.diceFace_4;
            case 4: return s3Dice.diceFace_5;
            case 5: return s3Dice.diceFace_6;
            default: return s3Dice.diceFace_1;
        }
    }
}
