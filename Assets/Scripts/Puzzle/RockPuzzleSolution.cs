using UnityEngine;

public class RockPuzzleSolution : MonoBehaviour
{
    public int numRockSolved = 0;
    public bool solved = false;
    public GameObject Gem_blue;
    private DissolveController dissolveController;
    public GameObject environmentTorch1;
    public GameObject environmentTorch2;
    // Start is called before the first frame update
    void Start(){
        dissolveController = GameObject.FindGameObjectWithTag("MagicRock").GetComponent<DissolveController>();
    }

    public void CheckifSolved(){
        if(numRockSolved == 3 && solved == false){
            solved = true;
            Gem_blue.SetActive(true);
            dissolveController.StartDissolve();
            environmentTorch1.GetComponent<Flammable>().isFlammableNow = true;
            environmentTorch2.GetComponent<Flammable>().isFlammableNow = true;
            environmentTorch1.GetComponent<Flammable>().LightUp();
            environmentTorch2.GetComponent<Flammable>().LightUp();
        }
    }

}
