using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class SpritesDatabase
{
    private static Dictionary<string, Sprite> _database = new();

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

    public static bool ContainsSprite(Vector2 position, float angle)
    {
        string key = CreateDatabaseKey(position, angle);

        return _database.ContainsKey(key);
    }

    public static bool TryGetSprite(Vector2 position, float angle, out Sprite value)
    {
        string key = CreateDatabaseKey(position, angle);

        return _database.TryGetValue(key, out value);
    }

    private static string CreateDatabaseKey(Vector2 position, float angle)
    {
        Func<float, int> processNumber = Mathf.RoundToInt;

        return $"posX{processNumber(position.x)}_Y{processNumber(position.y)}_{processNumber(angle)}";
    }
}
