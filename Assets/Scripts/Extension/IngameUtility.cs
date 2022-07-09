using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class IngameUtility
{
    public static BehaviourState GetRandomBehaviourState(this S3BehaviourDiceData s3Dice)
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

    public static int GetRandomActingPower(this S3ActingPowerDiceData s3Dice)
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

    public static IEnumerator AnimateSpriteRenderer(this SpriteRenderer spriteRenderer, List<Sprite> targetSprites, float delay, SpriteAnimatorPlayChecker spriteAnimatorPlayChecker, bool isInfinite = false, Action onComplete = null)
    {
        do
        {
            spriteAnimatorPlayChecker.isPlaying = true;
            for (int i = 0; i < targetSprites.Count; i++)
            {
                var targetSprite = targetSprites[i];
                spriteRenderer.SetSprite(targetSprite);
                yield return CommonUtility.GetYieldSec(delay);
            }
        }
        while (isInfinite);
        spriteAnimatorPlayChecker.isPlaying = false;
        onComplete?.Invoke();
    }
}

public class SpriteAnimatorPlayChecker
{
    public bool isPlaying;
}