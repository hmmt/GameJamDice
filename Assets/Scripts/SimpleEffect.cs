using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEffect : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;


    private float _elapsed = 0;
    void OnEnable()
    {
        StartCoroutine(EffectRoutine());
    }

    IEnumerator EffectRoutine()
    {
        float f = 0;
        while (f < 1)
        {
            spriteRenderer.color = new Color(1, 1, 1, f);
            yield return null;
            f += Time.deltaTime * 2;
        }

        spriteRenderer.color = Color.white;
        f = 1;
        yield return new WaitForSeconds(0.5f);

        while(f > 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, f);
            yield return null;
            f -= Time.deltaTime * 2;
        }


        Destroy(gameObject);
    }
}
