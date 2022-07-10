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
        panelInventory.UpdateDiceSummaryList(deckList)
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
