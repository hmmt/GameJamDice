using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopupDice : MonoBehaviour
{
    [SerializeField] Image[] faces;

    public void SetData(S3BehaviourDiceData behaviourDice)
    {
        faces[0].sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviourDice.diceFace_1);
        faces[1].sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviourDice.diceFace_2);
        faces[2].sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviourDice.diceFace_3);
        faces[3].sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviourDice.diceFace_4);
        faces[4].sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviourDice.diceFace_5);
        faces[5].sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviourDice.diceFace_6);
    }

    public void SetData(S3ActingPowerDiceData actingPowerDice)
    {
        faces[0].sprite = SpriteManager.instance.GetActingPowerIconSprite((int)actingPowerDice.diceFace_1);
        faces[1].sprite = SpriteManager.instance.GetActingPowerIconSprite((int)actingPowerDice.diceFace_2);
        faces[2].sprite = SpriteManager.instance.GetActingPowerIconSprite((int)actingPowerDice.diceFace_3);
        faces[3].sprite = SpriteManager.instance.GetActingPowerIconSprite((int)actingPowerDice.diceFace_4);
        faces[4].sprite = SpriteManager.instance.GetActingPowerIconSprite((int)actingPowerDice.diceFace_5);
        faces[5].sprite = SpriteManager.instance.GetActingPowerIconSprite((int)actingPowerDice.diceFace_6);
    }
}
