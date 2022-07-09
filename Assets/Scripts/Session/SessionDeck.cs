using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionDeck
{
    S3BehaviourDice _behaviourDice;
    S3ActingPowerDice _actingPowerDice;

    public S3BehaviourDice behaviourDice => _behaviourDice;
    public S3ActingPowerDice actingPowerDice => _actingPowerDice;

    public SessionDeck SetBehaviourDice(S3BehaviourDice behaviourDice)
    {
        this._behaviourDice = behaviourDice;
        return this;
    }

    public SessionDeck SetActingPowerDice(S3ActingPowerDice actingPowerDice)
    {
        this._actingPowerDice = actingPowerDice;
        return this;
    }
}


