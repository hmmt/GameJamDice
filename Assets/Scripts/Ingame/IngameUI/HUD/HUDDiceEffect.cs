using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HUDDiceEffect : MonoBehaviour
{
    [SerializeField] RectTransform rtLeftDice, rtRightDice;

    public HUDDamageEffect PlayDiceEffect()
    {
        rtLeftDice.DOKill();
        rtRightDice.DOKill();

        rtLeftDice.anchoredPosition = new Vector2(419f, 15f);
        rtRightDice.anchoredPosition = new Vector2(531f, 15f);

        // sequence1 = left 252, 26 / right 222, -17
        // sequence2 = left 127, 3 / right 141, -25
        // sequence3 = left -68, -20 / right 89, -20

        rtLeftDice.DOJumpAnchorPos(new Vector2(252f, 26f), 10f, 1, 0.75f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            rtLeftDice.DOJumpAnchorPos()
        })
    }
}
