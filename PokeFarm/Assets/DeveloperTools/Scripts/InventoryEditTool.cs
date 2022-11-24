using System.Collections.Generic;
using UnityEngine;

namespace DeveloperTools.Scripts
{
    [CreateAssetMenu(menuName = "DeveloperTools/InventoryEditTool")]
    public class InventoryEditTool : ScriptableObject
    {
        public List<ItemSlot> slots;
    }
}
