
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{


    public static void saveData(CurrentData current)
    {
        Debug.Log("Saving Data");
        BinaryFormatter formatter = new BinaryFormatter();
        //Test12345
        string path = Application.persistentDataPath + "/Test12345";

        FileStream stream = new FileStream(path, FileMode.Create);


        GameData data = new GameData(current);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static GameData LoadGameData()
    {

        Debug.Log("Loading...");
        GameData dataToReturn;
        string path = Application.persistentDataPath + "/Test12345";

        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            dataToReturn = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return dataToReturn;

        }
        else
        {
            // Debug.LogError("Save file not found in" + path);
            Debug.Log("no files found");
            return null;
        }
    }
}
