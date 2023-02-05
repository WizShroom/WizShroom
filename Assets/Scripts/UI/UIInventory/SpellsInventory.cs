using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class SpellsInventory : SingletonMono<SpellsInventory>
{
    public List<SpellSlot> spellSlots;

    private void Start()
    {
        for (int i = 0; i < spellSlots.Count; i++)
        {
            SpellSlot spellSlot = spellSlots[i];
            spellSlot.slotID = i;
        }
    }

    public void AddSpellToFreeSlot(Spell spellToAdd)
    {
        foreach (SpellSlot spellSlot in spellSlots)
        {
            if (spellSlot.AddSpell(spellToAdd))
            {
                break;
            }
        }
    }

    public void CheckMergeability(int giverSlotID, int takerSlotID)
    {
        SpellSlot giverSlot = GetSpellSlotByID(giverSlotID);
        SpellSlot takerSlot = GetSpellSlotByID(takerSlotID);

        if (takerSlot.containedSpell.spell == null)
        {
            MoveSpell(giverSlot, takerSlot);
        }
        else
        {
            MergeSpells(giverSlot, takerSlot);
        }
    }

    public void MoveSpell(SpellSlot giverSlot, SpellSlot takerSlot)
    {
        takerSlot.AddSpell(giverSlot.containedSpell.spell);
        giverSlot.RemoveSpell();
    }

    public void MergeSpells(SpellSlot giverSlot, SpellSlot takerSlot)
    {
        List<SpellEffect> takerEffects = takerSlot.containedSpell.spell.spellEffects;
        List<SpellEffect> giverEffects = giverSlot.containedSpell.spell.spellEffects;

        List<SpellEffect> newSpell = new List<SpellEffect>(takerEffects);
        Spell mergedSpell = null;
        ActionBarSlot takerActionBarSlot = takerSlot.connectedActionBar;

        foreach (SpellEffect giverEffect in giverEffects)
        {
            foreach (SpellEffect takerEffect in takerEffects)
            {
                if (takerEffects.Contains(giverEffect) || giverEffect.incompatibleEffects.Contains(takerEffect))
                {
                    break;
                }
                if (!giverEffect.compatibleEffects.Contains(takerEffect))
                {
                    continue;
                }
                newSpell.Add(giverEffect);
                break;
            }
        }

        string directory = Application.persistentDataPath + "/UserCreatedSpells";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        Spell existingSpellWithEffects = null;
        string[] existingAssetFiles = Directory.GetFiles(directory, "*.json");
        foreach (string file in existingAssetFiles)
        {
            SpellData spellFromFile = JsonUtility.FromJson<SpellData>(File.ReadAllText(file));
            List<SpellEffect> fromFileEffects = new List<SpellEffect>();
            foreach (int spellEffect in spellFromFile.effects)
            {
                fromFileEffects.Add(GameController.Instance.spellEffectDatabase.spellEffects[spellEffect]);
            }
            bool equal = fromFileEffects.OrderBy(x => x.name).SequenceEqual(newSpell.OrderBy(x => x.name));
            if (equal)
            {
                existingSpellWithEffects = ScriptableObject.CreateInstance<Spell>();
                existingSpellWithEffects.spellEffects = fromFileEffects;
                break;
            }
        }

        if (existingSpellWithEffects)
        {
            mergedSpell = existingSpellWithEffects;
        }
        else
        {
            string path = directory + "/test.json";
            FileStream stream = new FileStream(path, FileMode.Create);

            mergedSpell = ScriptableObject.CreateInstance<Spell>();
            Sprite takerSprite = takerSlot.slotImage.sprite;
            mergedSpell.spellEffects = new List<SpellEffect>(newSpell);
            mergedSpell.spellSprite = takerSprite;

            SpellData data = new SpellData(mergedSpell);

            string json = JsonUtility.ToJson(data, true);
            stream.Write(Encoding.ASCII.GetBytes(json), 0, json.Length);
            stream.Close();
            Debug.Log("Game saved to " + path);
        }

        takerSlot.RemoveSpell();
        giverSlot.RemoveSpell();
        takerSlot.AddSpell(mergedSpell);
        if (takerActionBarSlot)
        {
            ConnectToActionBarSlot(takerSlot, takerActionBarSlot);
        }
    }

    public void ConnectToActionBarSlot(SpellSlot giverSlot, ActionBarSlot connectingSlot)
    {
        giverSlot.connectedActionBar = connectingSlot;
        connectingSlot.AddSpell(giverSlot.containedSpell.spell);
    }

    public SpellSlot GetSpellSlotByID(int slotID)
    {
        SpellSlot returnSlot = null;
        foreach (SpellSlot spellSlot in spellSlots)
        {
            if (spellSlot.slotID == slotID)
            {
                returnSlot = spellSlot;
                break;
            }
        }
        return returnSlot;
    }
}