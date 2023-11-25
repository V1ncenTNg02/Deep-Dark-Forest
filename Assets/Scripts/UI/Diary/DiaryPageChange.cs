using ORZ;
using ORZ.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiaryPageChange : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    [TextArea(3,10)] public List<string> Contents = new List<string>();
    private int currentPage = 0;

    void Start()
    {
        if (Contents.Count > 0) tmp.text = Contents[0];
    }

    public void NextPage()
    {
        if (currentPage == Contents.Count - 1)
        {
            ObjectGetter.globalEvent.GetComponent<MessageController>().UpdateGM("This is the last page.");
            return;
        }
        currentPage++;
        tmp.text = Contents[currentPage];
    }

    public void LastPage()
    {
        if (currentPage == 0)
        {
            ObjectGetter.globalEvent.GetComponent<MessageController>().UpdateGM("This is the first page.");
            return;
        }
        currentPage--;
        tmp.text = Contents[currentPage];
    }

}
