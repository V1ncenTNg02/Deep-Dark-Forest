using ORZ;
using ORZ.Enemy;
using ORZ.Player;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool IsFreezing { get; set; } = false;

    public void FreezeGame()
    {
        FreezeAllEnemies();
        FreezePlayer();
    }

    public void UnFreezeGame()
    {
        UnFreezeAllEnemies();
        UnFreezePlayer();
    }

    public void FreezeAllEnemies()
    {
        IsFreezing = true;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 0) return;
        StopDeFreezeAllEnemies(enemies);
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().LongTimeFreeze();
        }
    }

    public void UnFreezeAllEnemies()
    {
        Debug.Log("UnFreeze!");
        IsFreezing = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 0) return;
        foreach (GameObject enemy in enemies)
        {
            StartCoroutine(enemy.GetComponent<EnemyController>().DeFreeze(0f));
        }
    }

    public void StopDeFreezeAllEnemies(GameObject[] enemies)
    {
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().StopDeFreeze();
        }
    }

    public void FreezePlayer()
    {
        ObjectGetter.player.GetComponent<PlayerController>().isFreezing = true;
    }

    public void UnFreezePlayer()
    {
        ObjectGetter.player.GetComponent<PlayerController>().isFreezing = false;
    }
}
