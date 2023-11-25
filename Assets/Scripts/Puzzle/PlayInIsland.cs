using ORZ;
using UnityEngine;

public class PlayInIsland : MonoBehaviour
{
    private Bounds bounds;

    void Start()
    {
        bounds = GetComponent<BoxCollider>().bounds;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bounds.Contains(ObjectGetter.player.transform.position))
        {
            ObjectGetter.timeController.FreezeAllEnemies();
        }
    }
}
