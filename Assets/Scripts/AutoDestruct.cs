using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    public float time = 2;

    private float elaspedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elaspedTime += Time.deltaTime;

        if (time >= elaspedTime)
            Destroy(gameObject);
    }
}
