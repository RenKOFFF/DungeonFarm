#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public enum SaveSlot
{
    Slot1,
    Slot2,
    Slot3,
}

public enum DataCategory
{
    Containers,
    Time,
    WorldData,
}

public static class GameDataController
{
    public static readonly Dictionary<string, Item> AllItems;

    private static SaveSlot CurrentSaveSlot { get; set; } = SaveSlot.Slot1;

    private static string PersistentDataPath => Application.persistentDataPath;

    private const char PathSeparator = '/';

    static GameDataController()
    {
        AllItems = Resources
            .LoadAll<Item>("Items")
            .ToDictionary(i => i.Name);
    }

    /// <summary>
    /// Вызывается при выборе игроком доступного сохранения.
    /// </summary>
    public static void SetSaveSlot(SaveSlot slot)
    {
        CurrentSaveSlot = slot;
    }

    public static void Save(object data, DataCategory dataCategory, string savedFileName)
    {
        var saveDirectoryPath = GetSaveDirectoryPath(dataCategory);
        var saveFilePath = GetSavedFilePath(dataCategory, savedFileName);

        if (!Directory.Exists(saveDirectoryPath))
            Directory.CreateDirectory(saveDirectoryPath);

        try
        {
            var jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(saveFilePath, jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Не удалось сохранить данные по пути [{saveFilePath}]. [{e.Message}].");
        }
    }

    /// <returns>default(T), если не удалось загрузить данные</returns>
    public static T? Load<T>(DataCategory dataCategory, string savedFileName)
    {
        var savedFilePath = GetSavedFilePath(dataCategory, savedFileName);

        if (!File.Exists(savedFilePath))
            return default;

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
        => Load<T>(dataCategory, savedFileName) ?? new T();

    private static string GetSaveDirectoryPath(DataCategory dataCategory)
    {
        var pathParts = new[]
        {
            CurrentSaveSlot.ToString(),
            dataCategory.ToString(),
        };

        return AddToPersistentDataPath(string.Join(PathSeparator, pathParts));
    }

    private static string GetSavedFilePath(DataCategory dataCategory, string saveFileName)
        => $"{GetSaveDirectoryPath(dataCategory)}{PathSeparator}{saveFileName}.json";

    private static string AddToPersistentDataPath(string pathPart)
        => $"{PersistentDataPath}{PathSeparator}{pathPart}";
}
