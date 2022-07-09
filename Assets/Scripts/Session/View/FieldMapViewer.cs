using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMapViewer : MonoBehaviour
{
    [SerializeField] Transform[] mapObjects;


    const float speed = 1;

    private float leftBound
    {
        get
        {
            float ratio = Screen.width / (float)Screen.height;
            return -Camera.main.orthographicSize * ratio;
        }
    }
    private float rightBound
    {
        get
        {
            float ratio = Screen.width / (float)Screen.height;
            return Camera.main.orthographicSize * ratio;
        }
    }

    public void SetToMoveState()
    {
        StartCoroutine(MoveRightRoutine());
    }

    public void SetToStopState()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveRightRoutine()
    {

        while(true)
        {
            yield return null;

            foreach(var tr in mapObjects)
            {
                var pos = tr.localPosition;
                pos.x -= Time.deltaTime * speed;

                if (pos.x < leftBound - (rightBound - leftBound) / 2)
                {
                    pos.x = rightBound + (rightBound - leftBound) / 2;
                }

                tr.localPosition = pos;
            }
        }
    }
}
