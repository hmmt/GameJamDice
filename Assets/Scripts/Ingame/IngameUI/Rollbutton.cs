using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Rollbutton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    private const float min = 0.3f;
    private const float max = 0.7f;

    private float elapsed = 0;
    private float gauge = 0;

    private float recoverDelay = 0;


    private bool pointerDownInButton = false;

    [SerializeField] Image gaugeImage;
    [SerializeField] UnityEvent<float> onClick;


    private void Update()
    {
        if(pointerDownInButton)
        {
            elapsed += Time.deltaTime * 0.75f;
            gauge = Mathf.PingPong(elapsed, max - min) + min;
            gaugeImage.fillAmount = gauge;
        }
        else
        {
            if(recoverDelay > 0)
            {
                recoverDelay -= Time.deltaTime;
                return;
            }
            if(gauge > 0)
            {
                gauge -= Time.deltaTime * 2;
            }

            gaugeImage.fillAmount = Mathf.Max(0, gauge);
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IngameLogicManager.instance.CanRoll())
            return;
        elapsed = 0;
        pointerDownInButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerDownInButton = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(pointerDownInButton)
        {
            recoverDelay = 1f;
            onClick?.Invoke(gauge);
        }
        pointerDownInButton = false;
    }
}
