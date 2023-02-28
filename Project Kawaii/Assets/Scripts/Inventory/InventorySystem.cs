using UnityEngine;

namespace MikelW.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        public int inventorySize = 20;
        public int maxInventorySize = 40;
        public Item[] items;


        public GameObject[] inventoryDrags;

        private void Awake()
        {
            items = new Item[inventorySize];
        }

    }
}