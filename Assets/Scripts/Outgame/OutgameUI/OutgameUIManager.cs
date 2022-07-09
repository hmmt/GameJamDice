using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutgameUIManager : MonoBehaviour
{
    [SerializeField] PanelInventory panelInventory;

    public void OnClickOpenPanelInventory()
    {
        var deckList = PermanentPlayer.instance.startInventory.currentDeckList;
        panelInventory.UpdateDiceSummaryList(deckList)
                      .Open();
    }

    public void OnClickOpenPanelShop()
    {

    }

    public void OnClickToIngame()
    {

    }

    public void OnClickToPvp()
    {

    }
}
