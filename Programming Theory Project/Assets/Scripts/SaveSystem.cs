using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string filePath = Application.persistentDataPath + "/highscores";

    public static void SaveHighscores (HighscoreArray activeHighscores)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SaveData saveData = new SaveData(activeHighscores);

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(fileStream, saveData);
        }
    }

    public static SaveData LoadHighscores()
    {
        if (File.Exists(filePath))
        {
            // Load SaveData from disk
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                SaveData saveData = formatter.Deserialize(fileStream) as SaveData;

                // Check if the SaveData contains a complete highscore array
                if (saveData.highscores.Length == 5)
                {
                    return saveData;
                } 
            }
        }
         
        // Return an empty SaveData object if no correct file exists
        HighscoreArray empty = ScriptableObject.CreateInstance<HighscoreArray>();

        return new SaveData(empty);  
    }
}
