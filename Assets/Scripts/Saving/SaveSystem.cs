using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{

    public static void SaveProgress(PlayerStats playerData, Rendering renderer, QuestHolder quests)
    {
        Debug.Log("GameSaved");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/gameData.cow";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(playerData, renderer, quests);

        formatter.Serialize(stream, data);

        stream.Close();
    }


    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/gameData.cow";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;

            //Debug.Log("Data Loaded From " + path);
            stream.Close();

            return data;

        }
        else
        {
            //Debug.LogError("Save File Not Found In "+path);
            Debug.Log("Save File Not Found In "+path);
            return null;
        }
    }


    public static void ClearData()//(PlayerStats playerData, Rendering renderer)
    {
        /*
        string path = Application.persistentDataPath + "/gameData.cow";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete(true);
        Directory.CreateDirectory(path);
        */
        string path = Application.persistentDataPath + "/gameData.cow";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    
}
