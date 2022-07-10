using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutgameUIManager : MonoBehaviour
{
    [SerializeField] PanelInventory panelInventory;
    [SerializeField] PanelShop panelShop;

    public void OnClickOpenPanelInventory()
    {
        var deckList = PermanentPlayer.instance.startInventory.currentDeckList;
        var behaviourDiceList = PermanentPlayer.instance.startInventory.behaviourDiceList;
        var actingPowerDiceList = PermanentPlayer.instance.startInventory.actingPowerDiceList;
        panelInventory.UpdateDiceSummaryList(deckList)
                      .UpdateInventory(behaviourDiceList, actingPowerDiceList)
                      .Open();
    }

    public void OnClickOpenPanelShop()
    {
        panelShop.Open();
    }

    public void OnClickToIngame()
    {

    }

    public void OnClickToPvp()
    {

    }
}
