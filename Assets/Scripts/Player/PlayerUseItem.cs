using ORZ.Item;
using UnityEngine;
using UnityEngine.UI;

namespace ORZ.Player
{
    public class PlayerUseItem : MonoBehaviour
    {
        private int selectItem = 0;
        private int lastSelectItem = 0;
        public GameObject startImage;

        void Start()
        {
            if (startImage.TryGetComponent<Image>(out Image image))
            {
                image.color = Color.red;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<PlayerController>().isFreezing) return;
            
            SelectItem();
            
            if (Input.GetButtonDown("Throw"))
            {
                Transform item = ObjectGetter.items[selectItem].transform.Find("item");
                if (item != null)
                {
                    item.gameObject.GetComponent<ConsumeItem>().UseItem();
                    if (item.GetComponent<Torch>() == null && item.GetComponent<CeremonyItem>() == null) 
                    {
                        Destroy(item.gameObject);
                    }
                }
            }

        }

        void SelectItem()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeItem(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeItem(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeItem(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeItem(3);
        }

        void ChangeItem(int index)
        {
            if (index != selectItem)
            {
                lastSelectItem = selectItem;
                selectItem = index;
                ObjectGetter.items[lastSelectItem].GetComponent<Image>().color = Color.white;
                ObjectGetter.items[selectItem].GetComponent<Image>().color = Color.red;
                SoundController.Instance.PlaySpecUISound("ChangeItem");
            }
        }
    }

}
