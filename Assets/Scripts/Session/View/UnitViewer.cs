using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitViewer : MonoBehaviour
{
    [SerializeField] GameObject rendererRoot;

    public UnitStatusData data { private set; get; }

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
        // ���� ���� ui ����
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


        // �ӽ� ���
        while(true)
        {
            rendererRoot.transform.localPosition = new Vector3(0, Mathf.PingPong(elapsedTime, 0.5f), 0);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
}
