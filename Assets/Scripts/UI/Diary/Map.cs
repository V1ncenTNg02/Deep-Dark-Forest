using ORZ;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            OpenMap();
        }
    }

    public void OpenMap()
    {
        ObjectGetter.diaryController.OpenOrCloseDiary();
        ObjectGetter.diaryController.ChangeToItem(1);
    }
}
