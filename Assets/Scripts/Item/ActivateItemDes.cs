using UnityEngine;

namespace ORZ.Item
{
    public class ActivateItemDes : MonoBehaviour
    {
        private bool inbound = false;
        public int targetStory;
        public bool DestoryAfterPickup = false;

        [Tooltip("Use to set the Picked Item in specific index to true. " +
                 "Be able to make sure the diary will only be opened in first pick")]
        public int targetItemPicked = -1;

        void Update()
        {
            if (!DestoryAfterPickup && ObjectGetter.inventoryController.PickedItem[targetItemPicked].IsPicked) return;
            if (inbound && Input.GetButtonDown("Interact"))
            {
                ObjectGetter.diaryController.ActivateItem(targetStory);
                ObjectGetter.diaryController.OpenOrCloseDiary();
                ObjectGetter.diaryController.ChangeToItem(targetStory);
                if (DestoryAfterPickup)
                {
                    Destroy(gameObject);
                }
                else
                {
                    ObjectGetter.inventoryController.PickedItem[targetItemPicked].IsPicked = true;
                }
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
    }

}