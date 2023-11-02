using System.Collections.Generic;
using TechTalk.SpecFlow;
namespace SpecflowSelenium.Utils

public static Dictionary<string, string> ToDictionary(Table table)
{
    var dictionary = new Dictionary<string, string>();
    foreach (var column in table.Columns)
    {
        dictionary.Add(column[0], column[1]);
    }
    return dictionary;
}

