using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class IngamePanelInventory : PopupBase
{
    [SerializeField] List<UIItemDiceSummary> diceSummaryList;
    [SerializeField] LeanGameObjectPool diceViewerPool;
    [SerializeField] Transform tfParent;
    S3BehaviourDiceData selectedBehaviourData;
    S3ActingPowerDiceData selectedActingPowerData;


    public IngamePanelInventory UpdateDiceSummaryList(List<SessionDeck> deckList)
    {
        var count = diceSummaryList.Count;
        for (int i = 0; i < count; i++)
        {
            var diceSummary = diceSummaryList[i];
            var enableDeck = i < deckList.Count;
            if (enableDeck)
            {
                var deck = deckList[i];
                diceSummary.SetIndex(i)
                           .SetDeck(deck)
                           .SetBehaviours(deck.behaviourDice?.BehavioursToList() ?? new List<BehaviourState>())
                           .SetActingPowers(deck.actingPowerDice?.ActingPowerToList() ?? new List<int>())
                           .SetActionOnClickReleaseBehaviour((currentDeck) =>
                           {
                               if (currentDeck.behaviourDice == null)
                                   return;

                               PermanentPlayer.instance.AddBehaviourDice(currentDeck.behaviourDice);
                               currentDeck.SetBehaviourDice(null);
                               UpdateDiceSummaryList(SessionPlayer.instance.deck);
                               UpdateInventory(SessionPlayer.instance.inventory.behaviourDiceList, SessionPlayer.instance.inventory.actingPowerDiceList);
                           })
                           .SetActionOnClickReleaseActingPower((currentDeck) =>
                           {
                               if (currentDeck.behaviourDice == null)
                                   return;

                               PermanentPlayer.instance.AddActingPowerDice(currentDeck.actingPowerDice);
                               currentDeck.SetActingPowerDice(null);
                               UpdateDiceSummaryList(SessionPlayer.instance.deck);
                               UpdateInventory(SessionPlayer.instance.inventory.behaviourDiceList, SessionPlayer.instance.inventory.actingPowerDiceList);
                           })
                           .SetActionOnClickAddBehaviour((index, currentDeck) =>
                           {
                               var currentBehaviour = currentDeck.behaviourDice;

                               if (selectedBehaviourData == null)
                                   return;

                               PermanentPlayer.instance.RemoveBehaviourDice(selectedBehaviourData);
                               if (currentBehaviour != null)
                               {
                                   PermanentPlayer.instance.AddBehaviourDice(currentBehaviour);
                               }

                               currentDeck.SetBehaviourDice(selectedBehaviourData);
                               var targetDeck = diceSummaryList.Find(data => data.index == index);
                               targetDeck.SetBehaviours(selectedBehaviourData.BehavioursToList());

                               UpdateInventory(SessionPlayer.instance.inventory.behaviourDiceList, SessionPlayer.instance.inventory.actingPowerDiceList);
                               diceSummaryList.ForEach(diceSummary =>
                               {
                                   diceSummary.SetActiveReleaseActingPowerBtn(true)
                                              .SetActiveReleaseBehaviourBtn(true)
                                              .SetActiveAddBehaviourBtn(false)
                                              .SetActiveAddActingPowerBtn(false);
                               });
                           })
                           .SetActionOnClickAddActingPower((index, currentDeck) =>
                           {
                               var currentActingPower = currentDeck.actingPowerDice;

                               if (selectedActingPowerData == null)
                                   return;

                               PermanentPlayer.instance.RemoveActingPowerDice(selectedActingPowerData);
                               if (currentActingPower != null)
                               {
                                   PermanentPlayer.instance.AddActingPowerDice(currentActingPower);
                               }

                               currentDeck.SetActingPowerDice(selectedActingPowerData);
                               var targetDeck = diceSummaryList.Find(data => data.index == index);
                               targetDeck.SetActingPowers(selectedActingPowerData.ActingPowerToList());

                               UpdateInventory(SessionPlayer.instance.inventory.behaviourDiceList, SessionPlayer.instance.inventory.actingPowerDiceList);
                               diceSummaryList.ForEach(diceSummary =>
                               {
                                   diceSummary.SetActiveReleaseActingPowerBtn(true)
                                              .SetActiveReleaseBehaviourBtn(true)
                                              .SetActiveAddBehaviourBtn(false)
                                              .SetActiveAddActingPowerBtn(false);
                               });
                           });
            }
            else
            {
                diceSummary.Clear();
            }
        }
        return this;
    }

    public IngamePanelInventory UpdateInventory(List<S3BehaviourDiceData> behaviourDiceDataList, List<S3ActingPowerDiceData> actingPowerDiceDataList)
    {
        diceViewerPool.DespawnAll();
        var objectList = new List<GameObject>();

        behaviourDiceDataList.ForEach(data =>
        {
            GameObject prefab = null;
            diceViewerPool.TrySpawn(ref prefab, tfParent);
            var diceViewer = prefab.GetComponent<UIItemDiceViewer>();
            diceViewer.SetType(1)
                      .SetIndex(data.index)
                      .SetIconViewer((iconViewerList) =>
                      {
                          var behaviourList = data.BehavioursToList();
                          for (int i = 0; i < 6; i++)
                          {
                              var iconViewer = iconViewerList[i];
                              var behaviour = behaviourList[i];
                              var sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviour);
                              iconViewer.SetSpriteIcon(sprite);
                          }
                      })
                      .SetActionOnClick((type, index) =>
                      {
                          selectedActingPowerData = null;
                          selectedBehaviourData = data;
                          diceSummaryList.ForEach(diceSummary =>
                          {
                              diceSummary.SetActiveReleaseBehaviourBtn(false)
                                         .SetActiveReleaseActingPowerBtn(false)
                                         .SetActiveAddBehaviourBtn(true)
                                         .SetActiveAddActingPowerBtn(false);
                          });
                      });
            objectList.Add(prefab);
        });

        actingPowerDiceDataList.ForEach(data =>
        {
            GameObject prefab = null;
            diceViewerPool.TrySpawn(ref prefab, tfParent);
            var diceViewer = prefab.GetComponent<UIItemDiceViewer>();
            diceViewer.SetType(0)
                      .SetIndex(data.index)
                      .SetIconViewer((iconViewerList) =>
                      {
                          var actingPowerList = data.ActingPowerToList();
                          for (int i = 0; i < 6; i++)
                          {
                              var iconViewer = iconViewerList[i];
                              var actingPower = actingPowerList[i];
                              var sprite = SpriteManager.instance.GetActingPowerIconSprite(actingPower);
                              iconViewer.SetSpriteIcon(sprite);
                          }
                      })
                      .SetActionOnClick((type, index) =>
                      {
                          selectedBehaviourData = null;
                          selectedActingPowerData = data;
                          diceSummaryList.ForEach(diceSummary =>
                          {
                              diceSummary.SetActiveReleaseBehaviourBtn(false)
                                         .SetActiveReleaseActingPowerBtn(false)
                                         .SetActiveAddBehaviourBtn(false)
                                         .SetActiveAddActingPowerBtn(true);
                          });
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
