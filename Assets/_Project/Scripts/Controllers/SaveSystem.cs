using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;

public static class SaveSystem
{
    public static SaveData localData { get; private set; }
    public static int levelCount;

    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, localData);
        stream.Close();
    }

    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/save.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            localData = data;

            if (localData.stars.Count < levelCount)
            {
                localData.stars.Add(0);
            }
            Debug.Log("Carregou save ja existente");
        }
        else
        {
            localData = new SaveData();

            for (int i = 0; i < levelCount; i++)
            {
                localData.stars.Add(0);
            }

            for (int i = 0; i < 4; i++)
            {
                if(i == 0)
                {
                    localData.weaponsUnlocked.Add(true);
                }
                else
                {
                    localData.weaponsUnlocked.Add(false);
                }
            }

            Debug.Log("Carregou save novo");
        }

        return localData;
    }
}
