using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DiceConsequenceData 
{
    public BehaviourState behaviourState { get; private set; }

    public int actingPower { get; private set; }

    public DiceConsequenceData(BehaviourState behaviourState, int actingPower)
    {
        this.behaviourState = behaviourState;
        this.actingPower = actingPower;
    }
}
