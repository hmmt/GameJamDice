using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInventory : PopupBase
{
    [SerializeField] List<UIItemDiceSummary> diceSummaryList;

    public PanelInventory UpdateDiceSummaryList(List<SessionDeck> deckList)
    {
        var count = diceSummaryList.Count;
        for (int i = 0; i < count; i++)
        {
            var diceSummary = diceSummaryList[i];
            var enableDeck = i < deckList.Count;
            if (enableDeck)
            {
                var deck = deckList[i];
                diceSummary.SetBehaviours(deck.behaviourDice.BehavioursToList())
                           .SetActingPowers(deck.actingPowerDice.ActingPowerToList());
            }
            else
            {
                diceSummary.Clear();
            }
        }
        return this;
    }

    public override void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

}
