using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class IngameRolledInfo : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Image diceTypeImage;
    [SerializeField] TMPro.TextMeshProUGUI diceNumber;

    [SerializeField] Image selected;


    private Action clickCallback;

    public void SetDice(DiceConsequenceData data, Action onclick = null)
    {
#if false
        switch ( data.behaviourState)
        {
            case BehaviourState.none:
                diceType.text = "X";
                break;
            case BehaviourState.offense:
                diceType.text = "공격";
                break;
            case BehaviourState.defense:
                diceType.text = "방어";
                break;
            case BehaviourState.poison:
                diceType.text = "독";
                break;
            case BehaviourState.recovery:
                diceType.text = "회복";
                break;
            case BehaviourState.lightning:
                diceType.text = "연쇄";
                break;
        }
#endif
        diceTypeImage.color = Color.white;
        diceNumber.color = Color.white;

        diceTypeImage.enabled = true;
        diceTypeImage.sprite = SpriteManager.instance.GetBevaiourIconSprite((int)data.behaviourState);
        diceNumber.text = data.actingPower.ToString();

        clickCallback = onclick;
    }

    public void SetToEmpty()
    {
        diceTypeImage.enabled = false;
        diceNumber.text = "";
        if (selected != null)
            selected.enabled = false;

        clickCallback = null;
    }

    public void SetToUsed()
    {
        diceTypeImage.color = new Color(1, 1, 1, 0.5f);
        diceNumber.color = new Color(1, 1, 1, 0.5f);
    }

    public void SetSelected(bool b)
    {
        if (selected != null)
            selected.enabled = b;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clickCallback?.Invoke();
    }
}
