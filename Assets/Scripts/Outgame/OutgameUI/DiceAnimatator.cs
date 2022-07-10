using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceAnimatator : MonoBehaviour
{
    [SerializeField] Sprite[] behaviourDices;
    [SerializeField] Sprite[] powerDices;

    [SerializeField] Image behaviourDiceImg;
    [SerializeField] Image powerDiceImg;

    [SerializeField] IngameRolledInfo icon;
    [SerializeField] RectTransform[] locators;
    [SerializeField] RectTransform startLocator;

    int diceIndex;
    Action callback;
    public void PlayAnimation(BehaviourState behaviourType, int dicePower, int diceIndex, Action callback)
    {
        gameObject.SetActive(true);
        icon.gameObject.SetActive(false);

        behaviourDiceImg.enabled = true;
        powerDiceImg.enabled = true;
        try
        {
            behaviourDiceImg.sprite = behaviourDices[(int)behaviourType];
            powerDiceImg.sprite = powerDices[dicePower - 1];
        }
        catch(Exception e)
        {

        }

        icon.SetDice(new DiceConsequenceData(behaviourType, dicePower));

        this.callback = callback;

        this.diceIndex = diceIndex;
    }

    public void OnEndDiceAnimator()
    {
        StartCoroutine(GotoSlot(diceIndex));
    }

    IEnumerator GotoSlot(int index)
    {
        icon.gameObject.SetActive(true);
        behaviourDiceImg.enabled = false;
        powerDiceImg.enabled = false;
        float elapsedTime = 0;

        while(elapsedTime < 1)
        {
            icon.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startLocator.anchoredPosition, locators[index].anchoredPosition, elapsedTime * elapsedTime);
            icon.transform.localScale = new Vector3(2 - elapsedTime, 2 - elapsedTime, 1);
            yield return null;
            elapsedTime += Time.deltaTime * 3;
        }

        icon.gameObject.SetActive(false);
        gameObject.SetActive(false);

        callback?.Invoke();
    }
}
