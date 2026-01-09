using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LigaBotonera.Extensions;
public static class TempDataExtensions
{
    public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
    {
        tempData[key] = JsonSerializer.Serialize(value);
    }

    public static T Get<T>(this ITempDataDictionary tempData, string key)
    {
        tempData.TryGetValue(key, out object o);
        return o == null ? default : JsonSerializer.Deserialize<T>((string)o);
    }
}