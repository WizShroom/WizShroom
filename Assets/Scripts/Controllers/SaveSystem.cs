using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem : Singleton<SaveSystem>
{
    public void SaveGame(GameController controller)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        Spell spell = ScriptableObject.CreateInstance<Spell>();
        GameData data = new GameData(spell);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Game saved to " + path);
    }
    public GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}

[System.Serializable]
public class GameData
{
    public SpellData spellData;


    public GameData(Spell spell)
    {
        spellData = GatherSpellData(spell);
    }

    public SpellData GatherSpellData(Spell spell)
    {
        return new SpellData(spell);
    }
}