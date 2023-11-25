using ORZ;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool EyesOpened;

    private bool isFree = false;
    public bool OnMonster = false;
    public bool OnPlayer = true;

    void Update()
    {
        if (isFree) return;
        transform.parent.position = ObjectGetter.player.transform.position;
    }

    public void SetFree(bool isFree)
    {
        this.isFree = isFree;
    }

}
