using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveManager
{
    public static void SavePlayerData (PlayerStatus playerStatus)
    {
        PlayerData playerData = new PlayerData(playerStatus);
        string datapath = Application.persistentDataPath + "/player.save";
        FileStream fileStream = new FileStream(datapath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fileStream, playerData);
        fileStream.Close();
    }

    public static PlayerData LoadPlayerdata()
    {
        string datapath = Application.persistentDataPath + "/player.save";

        if (File.Exists(datapath))
        {
            FileStream fileStream = new FileStream(datapath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            PlayerData playerData = (PlayerData) bf.Deserialize(fileStream);
            fileStream.Close ();
            return playerData;
        }
        else
        {
            Debug.LogError("No se encontró el archivo guardado");
            return null;
        }
    }
}
