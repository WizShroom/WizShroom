using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/AssetsDatabase")]
public class AssetsDatabase : ScriptableObject
{
    public List<Sprite> sprites;
    public Dictionary<Sprite, int> spritesIDs = new Dictionary<Sprite, int>();

    public List<SpellEffect> spellEffects;
    public Dictionary<SpellEffect, int> spellEffectsIDs = new Dictionary<SpellEffect, int>();

    public List<Spell> spells;
    public Dictionary<Spell, int> spellsIDs = new Dictionary<Spell, int>();

    public List<GameObject> projectiles;
    public Dictionary<GameObject, int> projectilesIDs = new Dictionary<GameObject, int>();

    public List<AudioClip> audios;
    public Dictionary<AudioClip, int> audiosIDs = new Dictionary<AudioClip, int>();
}