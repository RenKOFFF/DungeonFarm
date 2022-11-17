using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class GameDataController
{
    public static void Save(object data, string savedFileName)
    {
        var savedFilePath = GetSavedFilePath(savedFileName);

        try
        {
            var jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(savedFilePath, jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Не удалось сохранить данные по пути [{savedFilePath}]. [{e.Message}].");
        }
    }

    public static T Load<T>(string savedFileName)
    {
        var savedFilePath = GetSavedFilePath(savedFileName);

        if (!File.Exists(savedFilePath))
        {
            Debug.LogError($"Не удалось загрузить данные. Не найден файл по пути [{savedFilePath}].");
            return default;
        }

        try
        {
            var jsonData = File.ReadAllText(savedFilePath);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Не удалось загрузить данные по пути [{savedFilePath}]. [{e.Message}].");
            return default;
        }
    }

    private static string GetSavedFilePath(string saveFileName)
        => Application.persistentDataPath + $"/{saveFileName}.json";
}
