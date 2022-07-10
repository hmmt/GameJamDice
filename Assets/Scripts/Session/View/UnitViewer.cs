using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitViewer : MonoBehaviour
{
    public enum SpriteState
    {
        Idle,
        Attack,
    }


    [SerializeField] SpriteRenderer rendererRoot;
    [SerializeField] HUDProgressBar hpBar;

    [SerializeField] GameObject shieldRoot;
    [SerializeField] TextMeshPro shieldText;

    [SerializeField] GameObject poisonRoot;
    [SerializeField] TextMeshPro poisonText;

    [SerializeField] GameObject selectMark;

    private SpriteSet spriteSet = null;

    public UnitStatusData data { private set; get; }

    private System.Action<UnitViewer> onClickCallback;

    public void Init(System.Action<UnitViewer> onClickCallback)
    {
        this.onClickCallback = onClickCallback;
    }

    public void SetIngameUnitData(UnitStatusData data)
    {
        this.data = data;
    }

    public void LoadPlayerSprites()
    {
        rendererRoot.flipX = true;
        spriteSet = SpriteManager.instance.GetSpriteSet(0);

        StopAllCoroutines();
        SetMotionToIdle();
    }
    public void LoadMonsterSprites(int monsterIndex)
    {
        rendererRoot.flipX = false;
        spriteSet = SpriteManager.instance.GetSpriteSet(monsterIndex + 1);
        StopAllCoroutines();
        SetMotionToIdle();
    }


    private void Update()
    {
        if (data == null)
            return;
        // 대충 유닛 ui 갱신

        hpBar.SetMaxValue(data.maxHp);
        hpBar.SetCurrentValue(data.hp);
        hpBar.CalculateProgress();

        if (data.shield > 0)
        {
            shieldRoot.SetActive(true);
            shieldText.text = data.shield.ToString();
        }
        else
        {
            shieldRoot.SetActive(false);
        }

        if(data.posionCount > 0)
        {
            poisonRoot.SetActive(true);
            poisonText.text = data.shield.ToString();
        }
        else
        {
            poisonRoot.SetActive(false);
        }

        if (data.isDead)
        {
            rendererRoot.gameObject.SetActive(false);
            hpBar.gameObject.SetActive(false);
        }
        else
        {
            rendererRoot.gameObject.SetActive(true);
            hpBar.gameObject.SetActive(true);
        }

        
    }

    public void SetMotionToAttack()
    {
        StopAllCoroutines();
        if (spriteSet != null)
        {
            StartCoroutine(PlaySpriteSet(spriteSet.attack));
        }
    }

    public void SetMotionToMove()
    {
        StopAllCoroutines();
        if (spriteSet != null)
        {
            StartCoroutine(PlayLoop(spriteSet.move));
        }
    }

    public void SetMotionToIdle()
    {
        StopAllCoroutines();
        if (spriteSet != null)
        {
            StartCoroutine(PlayLoop(spriteSet.idle));
        }
    }

    IEnumerator PlayLoop(List<Sprite> list)
    {
        if (list == null || list.Count == 0)
            yield break;

        int index = 0;
        while(true)
        {
            index %= list.Count;
            rendererRoot.sprite = list[index++];
            yield return new WaitForSeconds(0.08f);
        }
    }

    IEnumerator PlaySpriteSet(List<Sprite> list)
    {
        for(int i=0; i<list.Count; i++)
        {
            rendererRoot.sprite = list[i];
            yield return new WaitForSeconds(0.05f);
        }

        if(spriteSet != null)
        {
            StartCoroutine(PlayLoop(spriteSet.idle));
        }
    }


    public void ShowSelectionMark()
    {
        if (data == null || data.isDead)
            return;
        selectMark.SetActive(true);
    }

    public void HideSelectionMark()
    {
        selectMark.SetActive(false);
    }


    #region event trigger
    public void OnClick()
    {
        onClickCallback?.Invoke(this);
    }
    #endregion
}
