using UnityEngine;
using UnityEngine.UI;

namespace ORZ.Item
{
    public class CeremonyItem : MonoBehaviour
    {
        public Image TargetGem;
        public int ActivateGem;

        private bool inBound = false;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                inBound = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                inBound = false;
            }
        }

        void Update()
        {
            if (inBound && Input.GetButtonDown("Interact"))
            {
                Color color = TargetGem.color;
                TargetGem.color = new Color(color.r, color.g, color.b, 1);
                ObjectGetter.globalEvent.GetComponent<WinGame>().pickedGem[ActivateGem] = true;
                SoundController.Instance.PlaySpecSound("Pickup");
                Destroy(gameObject);
            }
        }

    }
}
