using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionDeck
{
    S3BehaviourDiceData _behaviourDice;
    S3ActingPowerDiceData _actingPowerDice;

    public S3BehaviourDiceData behaviourDice => _behaviourDice;
    public S3ActingPowerDiceData actingPowerDice => _actingPowerDice;

    public SessionDeck SetBehaviourDice(S3BehaviourDiceData behaviourDice)
    {
        this._behaviourDice = behaviourDice;
        return this;
    }

    public SessionDeck SetActingPowerDice(S3ActingPowerDiceData actingPowerDice)
    {
        this._actingPowerDice = actingPowerDice;
        return this;
    }
}


