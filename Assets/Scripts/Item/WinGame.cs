using ORZ;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    [HideInInspector] public bool[] pickedGem = { false, false, false };
    public int GemPlaced = 0;
    public void CheckWin(){
        if (GemPlaced == 3)
        {
            Debug.Log("You Win!");
            GameObject.Find("GlobalEvent").GetComponent<GameManager>().Win();
            SoundController.Instance.PlaySpecSound("Win");
        }
    }

}
