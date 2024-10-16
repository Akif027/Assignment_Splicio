
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/NewItemData")]
public class ItemData : ScriptableObject
{
    public List<WeaponItem> weaponItems = new List<WeaponItem>();
    public WeaponItem GetRandomWeapon()
    {
        if (weaponItems.Count == 0)
        {
            Debug.LogError("No weapon items available!");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, weaponItems.Count);  // Select a random index
        return weaponItems[randomIndex];
    }

}

[Serializable]
public class WeaponItem
{
    public string itemName;
    public Sprite itemSprite;

}