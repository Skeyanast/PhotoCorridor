using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class SpritesDatabase
{
    private static Dictionary<string, Sprite> _database = new();

    public static Dictionary<string, Sprite> Database => _database;

    public static void LoadAllSprites()
    {
        _database = new Dictionary<string, Sprite>();

        string[] positionFolders = Directory.GetDirectories(Application.dataPath + GlobalStringConstants.CORRIDOR_FOLDER);

        foreach (string folderPath in positionFolders)
        {
            string folderName = Path.GetFileName(folderPath);

            Sprite[] angleSprites = Resources.LoadAll<Sprite>(GlobalStringConstants.CORRIDOR_SPRITES_FOLDER + folderName);

            foreach (Sprite sprite in angleSprites)
            {
                string angleStr = sprite.name.Replace($"{folderName}_angle", "");

                if (int.TryParse(angleStr, out int angleValue))
                {
                    // Ключ в словаре: "posX0_Y0_45"
                    string key = $"{folderName}_{angleValue}";
                    _database[key] = sprite;
                }
            }
        }
    }
}
