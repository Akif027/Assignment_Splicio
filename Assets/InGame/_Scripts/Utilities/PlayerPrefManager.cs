using System.Collections.Generic;
using UnityEngine;

// this script will consist of all the commom player prefs methods
public static class PlayerPrefsManager
{
    private const string DeletedItemsKey = "DeletedItems";

    // Save the list of deleted items as a comma-separated string in PlayerPrefs
    public static void SaveDeletedItems(List<int> deletedItems)
    {
        string deletedItemsString = string.Join(",", deletedItems);
        PlayerPrefs.SetString(DeletedItemsKey, deletedItemsString);
        PlayerPrefs.Save();
    }

    // Load the list of deleted items from PlayerPrefs
    public static List<int> LoadDeletedItems()
    {
        List<int> deletedItems = new List<int>();

        string savedData = PlayerPrefs.GetString(DeletedItemsKey, string.Empty);
        if (!string.IsNullOrEmpty(savedData))
        {
            string[] deletedItemStrings = savedData.Split(',');
            foreach (var str in deletedItemStrings)
            {
                if (int.TryParse(str, out int index))
                {
                    deletedItems.Add(index);
                }
            }
        }

        return deletedItems;
    }

    // Clear the deleted items from PlayerPrefs (for reset or testing purposes)
    public static void ClearDeletedItems()
    {
        PlayerPrefs.DeleteKey(DeletedItemsKey);
        PlayerPrefs.Save();
    }
}
