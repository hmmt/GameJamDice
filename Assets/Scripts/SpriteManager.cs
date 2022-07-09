using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance { private set; get; }

    [SerializeField] SpriteContainer container;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public SpriteSet GetSpriteSet(int index)
    {
        return container.GetSpriteSet(index);
    }


    public Sprite GetBevaiourIconSprite(int index)
        => container.GetSprite($"BehaviourState_{index}");

    public Sprite GetActingPowerIconSprite(int index)
        => container.GetSprite($"ActingPowerIcon_{index}");
}
