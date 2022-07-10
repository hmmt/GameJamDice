using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInventory : PopupBase
{
    [SerializeField] List<UIItemDiceSummary> diceSummaryList;
    [SerializeField] LeanGameObjectPool diceViewerPool;
    [SerializeField] Transform tfParent;

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
                diceSummary.SetDeck(deck)
                           .SetBehaviours(deck.behaviourDice?.BehavioursToList() ?? new List<BehaviourState>())
                           .SetActingPowers(deck.actingPowerDice?.ActingPowerToList() ?? new List<int>())
                           .SetActionOnClickReleaseBehaviour((currentDeck) =>
                           {
                               PermanentPlayer.instance.AddBehaviourDice(currentDeck.behaviourDice);
                               currentDeck.SetBehaviourDice(null);
                               UpdateDiceSummaryList(deckList);
                               UpdateInventory(PermanentPlayer.instance.startInventory.behaviourDiceList, PermanentPlayer.instance.startInventory.actingPowerDiceList);
                           })
                           .SetActionOnClickReleaseActingPower((currentDeck) =>
                           {
                               PermanentPlayer.instance.AddActingPowerDice(currentDeck.actingPowerDice);
                               currentDeck.SetActingPowerDice(null);
                               UpdateDiceSummaryList(deckList);
                               UpdateInventory(PermanentPlayer.instance.startInventory.behaviourDiceList, PermanentPlayer.instance.startInventory.actingPowerDiceList);
                           });

            }
            else
            {
                diceSummary.Clear();
            }
        }
        return this;
    }

    public PanelInventory UpdateInventory(List<S3BehaviourDiceData> behaviourDiceDataList, List<S3ActingPowerDiceData> actingPowerDiceDataList)
    {
        diceViewerPool.DespawnAll();
        var objectList = new List<GameObject>();

        behaviourDiceDataList.ForEach(data =>
        {
            GameObject prefab = null;
            diceViewerPool.TrySpawn(ref prefab, tfParent);
            var diceViewer = prefab.GetComponent<UIItemDiceViewer>();
            diceViewer.SetIconViewer((iconViewerList) =>
            {
                var behaviourList = data.BehavioursToList();
                for (int i = 0; i < 6; i++)
                {
                    var iconViewer = iconViewerList[i];
                    var behaviour = behaviourList[i];
                    var sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviour);
                    iconViewer.SetSpriteIcon(sprite);
                }
            });
            objectList.Add(prefab);
        });

        actingPowerDiceDataList.ForEach(data =>
        {
            GameObject prefab = null;
            diceViewerPool.TrySpawn(ref prefab, tfParent);
            var diceViewer = prefab.GetComponent<UIItemDiceViewer>();
            diceViewer.SetIconViewer((iconViewerList) =>
            {
                var actingPowerList = data.ActingPowerToList();
                for (int i = 0; i < 6; i++)
                {
                    var iconViewer = iconViewerList[i];
                    var actingPower = actingPowerList[i];
                    var sprite = SpriteManager.instance.GetActingPowerIconSprite(actingPower);
                    iconViewer.SetSpriteIcon(sprite);
                }
            });
            objectList.Add(prefab);
        });
        objectList.Shuffle();
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].transform.SetSiblingIndex(i);
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

    public void OnClickClose()
    {
        Close();
    }
}
