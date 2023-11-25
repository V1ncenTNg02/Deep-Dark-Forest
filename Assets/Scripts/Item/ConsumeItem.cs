// Author: Cheng Chen

using ORZ.UI;
using UnityEngine;

namespace ORZ.Item
{
    public abstract class ConsumeItem : MonoBehaviour
    {
        private bool inbound = false;

        [Header("ItemController Icon")]
        public Sprite image;

        void Start()
        {
            if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
            {
                sr.sprite = image;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) inbound = true;
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) inbound = false;
        }

        void Update()
        {
            if (inbound && Input.GetButtonDown("Interact"))
            {
                inbound = false;
                if (ObjectGetter.inventoryController.AddItem(this))
                {
                    SoundController.Instance.PlaySpecSound("Pickup");
                    return;
                }
                ObjectGetter.globalEvent.GetComponent<MessageController>().UpdateGM("You cannot carry more");
            }
        }

        public abstract void UseItem();
    }

}
