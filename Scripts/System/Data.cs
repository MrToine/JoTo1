using Godot;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

public class Data
{
    // Chemin du fichier de sauvegarde
    private static string savePath = getSavePath();

    public static string getSavePath() {
        string documentsPath = OS.GetSystemDir(OS.SystemDir.Documents);
        string gameSavePath = $"{documentsPath}/JoTo1/save";

        // Créer le dossier de sauvegarde s'il n'existe pas
        if (!Directory.Exists(gameSavePath)) {
            Directory.CreateDirectory(gameSavePath);
        }

        return $"{gameSavePath}/save.json";
    }

    public static void SaveGame(GameData gameData) {
        var options = new JsonSerializerOptions {
            WriteIndented = true
        };
        options.Converters.Add(new Vector3JsonConverter());
        string json = JsonSerializer.Serialize(gameData, options);
        using var file = Godot.FileAccess.Open(savePath, Godot.FileAccess.ModeFlags.Write);
        file.StoreString(json);
    }

    public static GameData LoadGame() {
        if (!Godot.FileAccess.FileExists(savePath)) {
            GD.PrintErr("Aucune sauvegarde trouvée.");
            return null;
        }

        using var file = Godot.FileAccess.Open(savePath, Godot.FileAccess.ModeFlags.Read);
        string json = file.GetAsText();
        
        var options = new JsonSerializerOptions();
        options.Converters.Add(new Vector3JsonConverter());
        var gameData = JsonSerializer.Deserialize<GameData>(json, options);
        
        return gameData;
    }
}