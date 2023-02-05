using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpellEffects/Database")]
public class SpellEffectDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public SpellEffect[] spellEffects;
    public Dictionary<SpellEffect, int> spellEffectsIDs = new Dictionary<SpellEffect, int>();

    public void OnAfterDeserialize()
    {
        spellEffectsIDs = new Dictionary<SpellEffect, int>();
        for (int i = 0; i < spellEffects.Length; i++)
        {
            spellEffectsIDs.Add(spellEffects[i], i);
        }
    }

    public void OnBeforeSerialize()
    {
        spellEffectsIDs = new Dictionary<SpellEffect, int>();
    }
}