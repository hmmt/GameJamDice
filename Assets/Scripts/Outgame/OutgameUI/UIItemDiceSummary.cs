using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemDiceSummary : MonoBehaviour
{
    [SerializeField] List<IconViewer> behaviourIconList;
    [SerializeField] List<IconViewer> actingPowerIconList;

    public UIItemDiceSummary SetBehaviours(List<BehaviourState> behaviourList)
    {
        var count = behaviourIconList.Count;
        for (int i = 0; i < count; i++)
        {
            var behaviourState = behaviourList[i];
            var behaviourIcon = behaviourIconList[i];
            var sprite = SpriteManager.instance.GetBevaiourIconSprite((int)behaviourState);
            behaviourIcon.SetSpriteIcon(sprite);
        }
        return this;
    }

    public UIItemDiceSummary SetActingPowers(List<int> actingPowerList)
    {
        var count = behaviourIconList.Count;
        for (int i = 0; i < count; i++)
        {
            var actingPower = actingPowerList[i];
            var actingPowerIcon = actingPowerIconList[i];
            var sprite = SpriteManager.instance.GetActingPowerIconSprite(actingPower);
            actingPowerIcon.SetSpriteIcon(sprite);
        }
        return this;
    }

    public void Clear()
    {
        behaviourIconList.ForEach(iconViewer => iconViewer.SetActive(false));
        actingPowerIconList.ForEach(iconViewer => iconViewer.SetActive(false));
    }
}
