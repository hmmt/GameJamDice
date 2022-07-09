using System.Collections;
using System.Collections.Generic;
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

    private SpriteSet spriteSet = null;

    public UnitStatusData data { private set; get; }

    public void SetIngameUnitData(UnitStatusData data)
    {
        this.data = data;
    }

    public void LoadPlayerSprites()
    {
        spriteSet = SpriteManager.instance.GetSpriteSet(0);

        StopAllCoroutines();
        SetMotionToIdle();
    }
    public void LoadMonsterSprites(int monsterIndex)
    {
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


}
