using Bgb_DataAccessLibrary.Contracts.IServices.IResources;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace BgB_TeachingAssistant.Services.Resources
{
    public class ResourceDictionaryLoader : IResourceDictionaryLoader
    {
        private readonly ResourceDictionary _mergedDictionary;

        public ResourceDictionaryLoader(string resourcesPath)
        {
            _mergedDictionary = LoadDictionaries(resourcesPath);
            // Example: Preload the type
            _ = typeof(BgB_TeachingAssistant.Helpers.CustomProperties);

        }
        private ResourceDictionary LoadDictionaries(string resourcesPath)
        {
            var mergedDictionary = new ResourceDictionary();
            var xamlFiles = Directory.GetFiles(resourcesPath, "*.xaml", SearchOption.AllDirectories)
                .Where(file => !file.Contains("UserControls") &&
                               !file.Contains("ExcludeMoreFileNamesHere"));

            Console.WriteLine("Starting to load ResourceDictionaries...");

            foreach (var xamlFile in xamlFiles)
            {
                try
                {
                    Console.WriteLine($"Attempting to load: {xamlFile}");
                    var dictionary = new ResourceDictionary
                    {
                        Source = new Uri(xamlFile, UriKind.RelativeOrAbsolute)
                    };

                    mergedDictionary.MergedDictionaries.Add(dictionary);

                    Console.WriteLine($"Loaded dictionary from {xamlFile} with {dictionary.Keys.Count} keys:");
                    foreach (var key in dictionary.Keys)
                    {
                        Console.WriteLine($"Key: {key}, ValueType: {dictionary[key]?.GetType()}");
                    }
                }
                catch (XamlParseException ex) when (ex.InnerException?.Message.Contains("CustomProperties") == true)
                {
                    Console.WriteLine($"Skipped loading {xamlFile} due to unresolved 'CustomProperties': {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load ResourceDictionary from {xamlFile}: {ex.Message}");
                }
            }

            Console.WriteLine($"MergedDictionaries contains {mergedDictionary.MergedDictionaries.Count} child dictionaries.");
            Console.WriteLine("Completed loading ResourceDictionaries.");

            return mergedDictionary;
        }
        public IReadOnlyDictionary<string, object> GetMergedResources()
        {
            var resources = new Dictionary<string, object>();

            // Log keys directly in the _mergedDictionary (if any)
            Console.WriteLine($"Keys in parent merged dictionary: {_mergedDictionary.Keys.Count}");
            foreach (var key in _mergedDictionary.Keys)
            {
                if (key != null)
                {
                    string keyString = key.ToString();
                    resources[keyString] = _mergedDictionary[key];
                    Console.WriteLine($"Added key from parent dictionary: {keyString}");
                }
            }

            // Log keys from child dictionaries
            Console.WriteLine($"Iterating over {_mergedDictionary.MergedDictionaries.Count} merged dictionaries...");
            foreach (var childDictionary in _mergedDictionary.MergedDictionaries)
            {
                foreach (var key in childDictionary.Keys)
                {
                    if (key != null)
                    {
                        string keyString = key.ToString();
                        if (!resources.ContainsKey(keyString))
                        {
                            resources[keyString] = childDictionary[key];
                            Console.WriteLine($"Added key from child dictionary: {keyString}");
                        }
                        else
                        {
                            Console.WriteLine($"Duplicate key ignored: {keyString}");
                        }
                    }
                }
            }

            Console.WriteLine($"Total resources aggregated: {resources.Count}");
            return resources;
        }
    }
}
