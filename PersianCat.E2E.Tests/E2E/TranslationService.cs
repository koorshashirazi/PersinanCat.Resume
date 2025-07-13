using System.Text.Json;

namespace PersianCat.E2E.Tests.E2E;

public class TranslationService
{
    private readonly Dictionary<string, Dictionary<string, string>> _translations;

    public TranslationService()
    {
        _translations = [];
        var solutionDir = PathHelper.FindSlnDirectoryPath();
        if (solutionDir is null)
        {
            return;
        }
        var resourcesDir = Path.Combine(solutionDir, "PersianCat.Resume", "wwwroot", "Resources");
        foreach (var file in Directory.GetFiles(resourcesDir, "*.json"))
        {
            var lang = Path.GetFileNameWithoutExtension(file);
            if (lang is null)
            {
                continue;
            }
            var content = File.ReadAllText(file);
            var json = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
            if (json is not null)
            {
                _translations.Add(lang, json);
            }
        }
    }

    public string GetTranslation(string language, string key)
    {
        return _translations[language][key];
    }
}
