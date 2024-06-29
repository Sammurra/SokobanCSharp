using System;
using Newtonsoft.Json;

public static class FileHandler
{
    private static string filePath;
    private static readonly string saveFilePath = "gameState.json";  
    private static readonly string envVar = "GAME_SETUP_PATH";

    static FileHandler()
    {
        Initialize();
    }

    private static void Initialize()
    {
        filePath = Environment.GetEnvironmentVariable(envVar) ?? throw new InvalidOperationException("Environment variable for file path not set.");
    }

    public static dynamic ReadJson()
    {
        try
        {
            string jsonContent = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<dynamic>(jsonContent);
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: {filePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
    }

    public static void SaveJson(dynamic data)
    {
        try
        {
            string jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(saveFilePath, jsonContent);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error writing JSON file: {ex.Message}");
        }
    }
}
