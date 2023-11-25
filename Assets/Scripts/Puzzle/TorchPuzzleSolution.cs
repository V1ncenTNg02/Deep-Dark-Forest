using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzleSolution : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> solutionTorches = new List<GameObject>();
    public List<GameObject> otherTorches = new List<GameObject>();
    public GameObject sampleTorch;
    public bool solved = false;
    private bool answerCorrect = true;
    private bool gemGenerated = false;
    public GameObject gem_red;
    public GameObject environmentTorch1;
    public GameObject environmentTorch2;

    void Start()
    {
        sampleTorch.GetComponent<Flammable>().LightUp();
    }

    public void DistinguishWrongTorches(){
        foreach (GameObject torch in otherTorches)
        {
            Flammable torchTop = torch.GetComponent<Flammable>();
            torchTop.deActivateFireAndLight();
        }
    }
    
    public void DistinguishRightTorches(){
        foreach (GameObject torch in solutionTorches)
        {
            Flammable torchTop = torch.GetComponent<Flammable>();
            torchTop.deActivateFireAndLight();
        }
    }

    public void CheckAnswer(){
        if (solved == false){
            answerCorrect = true;
            foreach (GameObject solutionTorch in solutionTorches)
            {
                Flammable torchTopScript = solutionTorch.GetComponent<Flammable>();
                if (torchTopScript.firing == false){
                    answerCorrect = false;
                }
            }

            foreach (GameObject otherTorch in otherTorches)
            {
                Flammable torchTopScript = otherTorch.GetComponent<Flammable>();
                if (torchTopScript.firing){
                    answerCorrect = false;
                }
            }
        }
    
        if (!solved && answerCorrect){
            solved = true;
            Debug.Log("Solved");
        }

        if (solved && !gemGenerated){
            gemGenerated = true;
            gem_red.SetActive(true);
            sampleTorch.GetComponent<Flammable>().Extinguish();
            sampleTorch.SetActive(false);
            environmentTorch1.GetComponent<Flammable>().isFlammableNow = true;
            environmentTorch2.GetComponent<Flammable>().isFlammableNow = true;
            environmentTorch1.GetComponent<Flammable>().LightUp();
            environmentTorch2.GetComponent<Flammable>().LightUp();
        }
    }
}
