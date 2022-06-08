using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    public static EffectManager Inst { get; private set; }
    public Effect EffectPrefab;
    List<Effect> effects = new List<Effect>();

    void Awake()
    {
        Inst = this;
    }

    public static Effect PlayEffect(Vector3 startPos)
    {
        var effect = FindNoUseEffect();
        effect.Init(startPos);

        return effect;
    }

    static Effect CreateEffect()
    {
        var effect = Instantiate(Inst.EffectPrefab);
        effect.name += Inst.effects.Count;
        effect.transform.parent = Inst.transform;
        Inst.effects.Add(effect);
        return effect;
    }

    static Effect FindNoUseEffect()
    {
        for (int i = 0; i < Inst.effects.Count; i++) {
            if (Inst.effects[i].gameObject.activeSelf) continue;
            return Inst.effects[i];
        }

        return CreateEffect();
    }
}
