using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUp: MonoBehaviour
{
    private TorchPuzzleSolution torchPuzzleSolution;

    void Start(){
        torchPuzzleSolution = GameObject.Find("TorchPuzzle").GetComponent<TorchPuzzleSolution>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireDetector"))
        {
            Flammable flammable = GetComponent<Flammable>();
            if (flammable && flammable.firing == false)
            {
                flammable.LightUp();
                // Extinguish the torches in another group
                if (gameObject.CompareTag("SolutionTorch") && !torchPuzzleSolution.solved){
                    torchPuzzleSolution.CheckAnswer();
                    torchPuzzleSolution.DistinguishWrongTorches();
                }
                else if (gameObject.CompareTag("OtherTorch") && !torchPuzzleSolution.solved){
                    torchPuzzleSolution.DistinguishRightTorches();
                }
            }
        }

    }
}

    

