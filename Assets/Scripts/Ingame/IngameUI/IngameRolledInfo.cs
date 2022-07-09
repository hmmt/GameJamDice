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
        diceType.text = data.behaviourState.ToString();
        diceNumber.text = data.actingPower.ToString();
    }

    public void SetToEmpty()
    {
        diceType.text = "";
        diceNumber.text = "";
    }
}
