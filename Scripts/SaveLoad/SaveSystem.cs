using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveLevels(Indestructable availability)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levels.abc";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelsData data = new LevelsData(availability);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelsData LoadLevels()
    {
        string path = Application.persistentDataPath + "/levels.abc";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelsData data = formatter.Deserialize(stream) as LevelsData;
            stream.Close();

            return data;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
