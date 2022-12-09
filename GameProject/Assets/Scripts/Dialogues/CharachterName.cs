using System.Collections.Generic;
using UnityEngine;
public class CharacterName
{
    public enum Character
    {
        None = -1,
        Rectangle = 0,
        Circle = 1,
        Cat = 2,
        Druid = 3
    }

    private static readonly List<string> names = new()
    {
        "Rectangle+Прямокутник",
        "Circle+Коло",
        "Cat+Кіт",
        "Druid+Друїд"
    };

    public static string GetLocalizedCharachterName(Character character)
    {
        if((int)character == -1)
        {
            return null;
        }        
        return names[((int)character)].Split('+')[PlayerPrefs.GetInt("Language")];
    }
}

