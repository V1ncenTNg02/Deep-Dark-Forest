using ORZ.UI.Inventory;
using ORZ.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ORZ
{
    public static class ObjectGetter
    {
        public static GameObject player;
        public static List<GameObject> items;
        public static InventoryController inventoryController;
        public static DiaryController diaryController;
        public static GameObject globalEvent;
        public static GameManager gameManager;
        public static DialogueManager dialogueManager;
        public static TimeController timeController;


        public static void Initialize()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            items = GameObject.FindGameObjectsWithTag("ItemUI").ToList();
            items.Sort(Utils.CompareByName);
            inventoryController = GameObject.Find("UI").GetComponent<InventoryController>();
            diaryController = GameObject.Find("Diary").GetComponent<DiaryController>();
            globalEvent = GameObject.Find("GlobalEvent");
            if (globalEvent != null)
            {
                gameManager = globalEvent.GetComponent<GameManager>();
                dialogueManager = globalEvent.GetComponent<DialogueManager>();
                timeController = globalEvent.GetComponent<TimeController>();
            }
            else
            {
                Debug.Log("GlobalEvent Not Found!");
            }
            if (player == null) Debug.Log("Player Not Found!");
            if (items.Count == 0) Debug.Log("Items not Found!");
            if (inventoryController == null) Debug.Log("Inventory Controller not Found!");
            if (diaryController == null) Debug.Log("Diary Controller not Found!");
            if (gameManager == null) Debug.Log("GameManager Not Found!");
        }
    }

}
