using UnityEngine;

namespace MikelW.Inventory
{
    public abstract class Equipment : Item
    {
        [Header("CORE EQUIPMENT INFO")]
        public bool equipped;
        public bool hasInWorldAppearance;

        public GameObject inWorldAppearance;
    }
}