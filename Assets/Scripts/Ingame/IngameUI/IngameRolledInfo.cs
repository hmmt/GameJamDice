using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameRolledInfo : MonoBehaviour
{
    [SerializeField] Text diceType;
    [SerializeField] Text diceNumber;
    

    public void SetDice(DiceConsequenceData data)
    {

        switch ( data.behaviourState)
        {
            case BehaviourState.none:
                diceType.text = "X";
                break;
            case BehaviourState.offense:
                diceType.text = "����";
                break;
            case BehaviourState.defense:
                diceType.text = "���";
                break;
            case BehaviourState.poison:
                diceType.text = "��";
                break;
            case BehaviourState.recovery:
                diceType.text = "ȸ��";
                break;
            case BehaviourState.lightning:
                diceType.text = "����";
                break;
        }

        diceNumber.text = data.actingPower.ToString();
    }

    public void SetToEmpty()
    {
        diceType.text = "";
        diceNumber.text = "";
    }
}
