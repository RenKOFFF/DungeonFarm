using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public enum DataCategory
{
    Containers,
}

public static class GameDataController
{
    public static Dictionary<string, Item> AllItems;

    static GameDataController()
    {
        AllItems = Resources
            .LoadAll<Item>("Items")
            .ToDictionary(i => i.Name);
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

    /// <returns>default(T), если не удалось загрузить данные</returns>
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

    /// <returns>new T(), если не удалось загрузить данные</returns>
    public static T LoadWithInitializationIfEmpty<T>(DataCategory dataCategory, string savedFileName) where T : new()
    {
        var result = Load<T>(dataCategory, savedFileName);
        return result.Equals(default)
            ? new T()
            : result;
    }

    private static string GetSavedFilePath(DataCategory dataCategory, string saveFileName)
        => AddToPersistentDataPath($"{dataCategory}/{saveFileName}.json");

    private static string AddToPersistentDataPath(string pathPart)
        => $"{Application.persistentDataPath}/{pathPart}";
}
