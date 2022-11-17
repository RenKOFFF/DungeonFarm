using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class GameDataController
{
    public enum DataCategory
    {
        Containers,
    }

    public static void Save(object data, DataCategory dataCategory, string savedFileName)
    {
        var directoryPath = AddToPersistentDataPath(dataCategory.ToString());

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var savedFilePath = GetSavedFilePath(dataCategory, savedFileName);

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

    public static T Load<T>(DataCategory dataCategory, string savedFileName)
    {
        var savedFilePath = GetSavedFilePath(dataCategory, savedFileName);

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

    private static string GetSavedFilePath(DataCategory dataCategory, string saveFileName)
        => AddToPersistentDataPath($"{dataCategory}/{saveFileName}.json");

    private static string AddToPersistentDataPath(string pathPart)
        => $"{Application.persistentDataPath}/{pathPart}";
}
