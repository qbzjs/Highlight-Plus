using UnityEngine;
using MikelW.Skills;

namespace MikelW.Inventory
{
    public abstract class Tool : Item
    {
        [Header("CORE TOOL INFO")]
        public GameObject inWorldAppearance;
        public Skill affectedSkill;
    }
}