using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameRolledInfo : MonoBehaviour
{
    [SerializeField] Image diceTypeImage;
    [SerializeField] TMPro.TextMeshProUGUI diceNumber;
    

    public void SetDice(DiceConsequenceData data)
    {
#if false
        switch ( data.behaviourState)
        {
            case BehaviourState.none:
                diceType.text = "X";
                break;
            case BehaviourState.offense:
                diceType.text = "공격";
                break;
            case BehaviourState.defense:
                diceType.text = "방어";
                break;
            case BehaviourState.poison:
                diceType.text = "독";
                break;
            case BehaviourState.recovery:
                diceType.text = "회복";
                break;
            case BehaviourState.lightning:
                diceType.text = "연쇄";
                break;
        }
#endif
        diceTypeImage.enabled = true;
        diceTypeImage.sprite = SpriteManager.instance.GetBevaiourIconSprite((int)data.behaviourState);
        diceNumber.text = data.actingPower.ToString();
    }

    public void SetToEmpty()
    {
        diceTypeImage.enabled = false;
        diceNumber.text = "";
    }
}
