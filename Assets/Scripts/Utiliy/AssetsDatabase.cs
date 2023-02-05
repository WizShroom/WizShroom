using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/AssetsDatabase")]
public class AssetsDatabase : ScriptableObject, ISerializationCallbackReceiver
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

    public void OnAfterDeserialize()
    {
        /*         spritesIDs = new Dictionary<Sprite, int>();
                for (int i = 0; i < sprites.Length; i++)
                {
                    spritesIDs.Add(sprites[i], i);
                }

                spellEffectsIDs = new Dictionary<SpellEffect, int>();
                for (int i = 0; i < spellEffects.Length; i++)
                {
                    spellEffectsIDs.Add(spellEffects[i], i);
                    Debug.Log(spellEffects[i].spellEffectLevel + " " + i);

                }

                spellsIDs = new Dictionary<Spell, int>();
                for (int i = 0; i < spells.Length; i++)
                {
                    spellsIDs.Add(spells[i], i);
                }

                projectilesIDs = new Dictionary<GameObject, int>();
                for (int i = 0; i < projectiles.Length; i++)
                {
                    projectilesIDs.Add(projectiles[i], i);
                }

                audiosIDs = new Dictionary<AudioClip, int>();
                for (int i = 0; i < audios.Length; i++)
                {
                    audiosIDs.Add(audios[i], i);
                } */

    }

    public void OnBeforeSerialize()
    {
        /*       spritesIDs = new Dictionary<Sprite, int>();
              spellEffectsIDs = new Dictionary<SpellEffect, int>();
              spellsIDs = new Dictionary<Spell, int>();
              projectilesIDs = new Dictionary<GameObject, int>();
              audiosIDs = new Dictionary<AudioClip, int>(); */
    }
}