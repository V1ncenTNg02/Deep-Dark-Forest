using ORZ;
using UnityEngine;

public class PlayerEnterPuzzle : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!ObjectGetter.timeController.IsFreezing && other.CompareTag("Player"))
        {
            ObjectGetter.timeController.FreezeAllEnemies();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit!");
            ObjectGetter.timeController.UnFreezeAllEnemies();
        }
    }
}
