using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Save(object data, string filename)
    {
        string path = Application.persistentDataPath + filename;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Create);

        bf.Serialize(fs, data);
        fs.Close();
    }

    public static object Load(string filename)
    {
        string path = Application.persistentDataPath + filename;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            object data = bf.Deserialize(fs);
            fs.Close();

            return data;
        }
        else return null;
    }
}

[System.Serializable]
public class SettingsData
{
    public float Music, Sound;
    public bool Vibration;

    public SettingsData(float music, float sound, bool vibration)
    {
        Music = music;
        Sound = sound;
        Vibration = vibration;
    }
}

[System.Serializable]
public class GameData
{
    public uint Wins, Loses;
}