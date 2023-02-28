using UnityEngine;
using UnityEngine.UI;

namespace MikelW.Inventory
{
    /// <summary>
    /// Root class of all objects used for inventory system
    /// </summary>
    public abstract class Item : MonoBehaviour
    {
        [Header("CORE ITEM INFO")]
        public int goldValue = 5;
        public string itemName = "Unnamed";
        public string itemType = "Void";
        public string itemDescription = "This item hasn't been properly implemented, overwrite with correct info.";
        public bool stackable = false;
        public int stackLimit = 0;
        public int amount = 1;

        public Image icon;

        public virtual void Use()
        {
            Debug.LogError("Item 'Use' function unimplemented in: " + transform.name + " please overwrite and implement functionality");
        }
    }
}