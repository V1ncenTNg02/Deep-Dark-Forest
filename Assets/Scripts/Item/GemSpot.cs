using ORZ;
using UnityEngine;

public class GemSpot : MonoBehaviour
{
    public int TargetGem;
    private bool inBound = false;
    private bool placed = false;

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
        if (placed) return;
        if (inBound && Input.GetButtonDown("Interact"))
        {
            if (ObjectGetter.globalEvent.GetComponent<WinGame>().pickedGem[TargetGem])
            {
                transform.Find("Gem").gameObject.SetActive(true);
                transform.Find("Light").gameObject.SetActive(true);
                SoundController.Instance.PlaySpecSound("PlaceGem");
                Destroy(transform.Find("nameTag").gameObject);
                Destroy(GetComponent<ShowNameTag>());
                placed = true;
                ObjectGetter.globalEvent.GetComponent<WinGame>().GemPlaced++;
                ObjectGetter.globalEvent.GetComponent<WinGame>().CheckWin();

            }
        }
    }
}
