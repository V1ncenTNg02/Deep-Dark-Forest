using ORZ.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ORZ.UI.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [Serializable]
        public class ItemPickedInfo
        {
            [Tooltip("The Item Name")]
            public string Name;
            [Tooltip("To memorize whether the item has picked up before")]
            public bool IsPicked;
        }
        public List<ItemPickedInfo> PickedItem;

        public bool AddItem(ConsumeItem item)
        {
            String name = "item";
            
            foreach (GameObject go in ObjectGetter.items)
            {
                if (go.transform.Find(name) == null)
                {
                    item.name = name;
                    item.transform.SetParent(go.transform);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = Vector3.one * 0.8f;
                    item.transform.localRotation = Quaternion.identity;
                    Debug.Log("Item Picked");
                    return true;
                }
            }

            return false;

        }
    }

}
