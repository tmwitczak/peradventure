using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    static string path = Application.persistentDataPath + "/gamedata.atm";
    public static void SaveData(DataCollectorScript dataCollector)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
    
        GameData gameData = new GameData(dataCollector);
        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public static GameData LoadGameData()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData gameData = (GameData)formatter.Deserialize(stream);
            stream.Close();
            return gameData;
        }
        else
        {
            return null;
        }
    }
}
