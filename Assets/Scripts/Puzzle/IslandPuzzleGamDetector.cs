using UnityEngine;

public class IslandPuzzleGamDetector : MonoBehaviour
{
    public GameObject environmentTorch1;
    public GameObject environmentTorch2;

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player"))
        {
            environmentTorch1.GetComponent<Flammable>().isFlammableNow = true;
            environmentTorch2.GetComponent<Flammable>().isFlammableNow = true;
            environmentTorch1.GetComponent<Flammable>().LightUp();
            environmentTorch2.GetComponent<Flammable>().LightUp();
            Destroy(gameObject);
        }
    }
}
