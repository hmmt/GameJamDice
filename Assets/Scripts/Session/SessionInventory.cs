using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionInventory
{
    // ±Õ¬˙¿∏¥œ ¿œ¥‹ ∆€∫Ì∏Ø
    public List<S3BehaviourDice> behaviourDiceList = new List<S3BehaviourDice>();
    public List<S3ActingPowerDice> actingPowerDiceList = new List<S3ActingPowerDice>();
    public List<SessionItem> itemList = new List<SessionItem>();

    public void InitializeByPermanent(PermanentPlayer player)
    {
        player.startInventory
    }
}
