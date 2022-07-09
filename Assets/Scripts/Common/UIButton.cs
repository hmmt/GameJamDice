using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public enum ButtonSFXType
{
    click,
    close,
    unitRecruit,
    unitUpgrade,
    stageOpen
}

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField] RectTransform rtPivot;
    [SerializeField] float presseDownScale, pressUpScale;
    [SerializeField] ButtonSFXType buttonSfxType;
    [SerializeField] UnityEvent onClick;

    const float downDuration = 0.1f;
    const float upDuration = 0.1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        rtPivot.DOKill();
        rtPivot.DOScale(presseDownScale, downDuration).SetEase(Ease.Linear);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rtPivot.DOKill();
        rtPivot.DOScale(pressUpScale, upDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            rtPivot.DOScale(1f, upDuration).SetEase(Ease.OutBack);
        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}
