using ORZ;
using UnityEngine;

public class Diary : MonoBehaviour
{
    private bool inbound = false;
    void Update()
    {
        if (inbound && Input.GetButtonDown("Interact"))
        {
            ObjectGetter.diaryController.ActivateDiary();
            ObjectGetter.diaryController.OpenOrCloseDiary();
            ObjectGetter.diaryController.ChangeToItem(0);
            Destroy(gameObject);
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
