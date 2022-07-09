using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitViewer : MonoBehaviour
{
    [SerializeField] GameObject rendererRoot;

    UnitStatusData data;

    public void SetIngameUnitData(UnitStatusData data)
    {
        this.data = data;
    }

    public void LoadPlayerSprites()
    {

    }
    public void LoadMonsterSprites(int index)
    {
    }


    private void Update()
    {
        if (data == null)
            return;
        // 대충 유닛 ui 갱신
    }


    public void SetMotionToMove()
    {
        StopAllCoroutines();

        StartCoroutine(MoveMotion_temp());
    }

    public void SetMotionToIdle()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveMotion_temp()
    {
        float elapsedTime = 0;


        // 임시 모션
        while(true)
        {
            rendererRoot.transform.localPosition = new Vector3(0, Mathf.PingPong(elapsedTime, 0.5f), 0);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
}
