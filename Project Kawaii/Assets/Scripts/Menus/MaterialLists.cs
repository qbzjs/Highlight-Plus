using UnityEngine;

namespace MikelW.Profiles
{
    [CreateAssetMenu(menuName = "MikelW/Create Material List")]
    public class MaterialLists : ScriptableObject
    {
        public Material[] materialList;
    }
}