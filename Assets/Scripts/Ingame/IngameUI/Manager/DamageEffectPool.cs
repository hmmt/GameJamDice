using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectPool : MonoBehaviour
{
    [SerializeField] Transform tfParent;
    [SerializeField] LeanGameObjectPool damageEffectPool;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var effect = SpawnDamageEffect();
            effect.SetDamage(11)
                  .SetText("11")
                  .SetPosition(Vector3.zero)
                  .PlayEffect(() => damageEffectPool.Despawn(effect.gameObject));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            var effect = SpawnDamageEffect();
            effect.SetDamage(5)
                  .SetText("5")
                  .SetPosition(Vector3.zero)
                  .PlayEffect(() => damageEffectPool.Despawn(effect.gameObject));
        }
    }

    public HUDDamageEffect SpawnDamageEffect()
    {
        GameObject prefab = null;
        damageEffectPool.TrySpawn(ref prefab, tfParent ?? transform);
        return prefab.GetComponent<HUDDamageEffect>();
    }

    public void PlayDamageEffect(int damage, Vector3 pos)
    {
        var effect = SpawnDamageEffect();
        effect.SetDamage(damage)
              .SetText(damage.ToString())
              .SetPosition(pos)
              .PlayEffect(() => damageEffectPool.Despawn(effect.gameObject));
    }
}
