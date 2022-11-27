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

    private static List<string> Name = new List<string>
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
        return Name[((int)character)].Split('+')[PlayerPrefs.GetInt("Language")];
    }
}

