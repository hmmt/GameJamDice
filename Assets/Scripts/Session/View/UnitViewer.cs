using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitViewer : MonoBehaviour
{
    [SerializeField] GameObject rendererRoot;
    
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
