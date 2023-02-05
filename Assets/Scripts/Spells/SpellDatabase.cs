using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Database")]
public class SpellDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public Spell[] spells;
    public Dictionary<Spell, int> spellsIDs = new Dictionary<Spell, int>();

    public void OnAfterDeserialize()
    {
        spellsIDs = new Dictionary<Spell, int>();
        for (int i = 0; i < spells.Length; i++)
        {
            spellsIDs.Add(spells[i], i);
        }
    }

    public void OnBeforeSerialize()
    {
        spellsIDs = new Dictionary<Spell, int>();
    }
}