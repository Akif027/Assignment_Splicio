using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject content;
    public GameObject itemPrefab;

    [Header("Item Data")]
    public ItemData _itemsData;
    public int itemCount = 10;

    private List<GameObject> pooledItems = new List<GameObject>();
    private List<int> deletedItems = new List<int>();  // List to track deleted item indices

    void Start()
    {
        // Load previously deleted items from PlayerPrefs
        deletedItems = PlayerPrefsManager.LoadDeletedItems();

        // Generate the dynamic list of items
        GenerateRandomList();
    }

    // Generates a dynamic list of random items
    public void GenerateRandomList()
    {
        for (int i = 0; i < itemCount; i++)
        {
            GameObject itemObj = GetItemFromPool();
            itemObj.transform.SetParent(content.transform, false);

            Item item = itemObj.GetComponent<Item>();
            WeaponItem weapon = _itemsData.GetRandomWeapon();

            // Setup item properties
            item.index = i;
            item.Icon.sprite = weapon.itemSprite;
            item.Title.text = weapon.itemName;
            item.Cross.onClick.AddListener(() => DeactivateItem(item.index));

            // Deactivate the item if it was previously deleted
            if (deletedItems.Contains(i))
            {
                itemObj.SetActive(false);
            }
        }
    }

    // Fetches an item from the pool or creates a new one if none are available
    private GameObject GetItemFromPool()
    {
        foreach (var pooledItem in pooledItems)
        {
            if (!pooledItem.activeInHierarchy)
            {
                pooledItem.SetActive(true);
                return pooledItem;
            }
        }

        // If no inactive items, instantiate a new one
        GameObject newItem = Instantiate(itemPrefab);
        pooledItems.Add(newItem);
        return newItem;
    }

    // Deactivates the item and stores its index in PlayerPrefs
    public void DeactivateItem(int index)
    {
        if (index >= 0 && index < pooledItems.Count)
        {
            // Animate deactivation
            Tween.MoveAndScale(pooledItems[index], 0.4f, 1000f, 0.3f);

            // Store the index of the deleted item
            if (!deletedItems.Contains(index))
            {
                deletedItems.Add(index);
                PlayerPrefsManager.SaveDeletedItems(deletedItems);  // Save the updated list to PlayerPrefs
            }


        }
    }

    // Clears the list by deactivating all items for reuse later
    public void ClearList()
    {
        foreach (var item in pooledItems)
        {
            item.SetActive(false);
        }
    }

    // Clears the deleted items both locally and in PlayerPrefs
    public void ClearDeletedItems()
    {
        deletedItems.Clear();
        PlayerPrefsManager.ClearDeletedItems();
    }

    // Clears deleted items when the object is disabled
    void OnDisable()
    {
        ClearDeletedItems();
    }
}
