using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionInventory
{
    // ±Õ¬˙¿∏¥œ ¿œ¥‹ ∆€∫Ì∏Ø
    public List<S3BehaviourDiceData> behaviourDiceList = new List<S3BehaviourDiceData>();
    public List<S3ActingPowerDiceData> actingPowerDiceList = new List<S3ActingPowerDiceData>();
    public List<SessionItem> itemList = new List<SessionItem>();

    public void InitializeByPermanent(PermanentPlayer player)
    {
        //player.startInventory
    }
}
