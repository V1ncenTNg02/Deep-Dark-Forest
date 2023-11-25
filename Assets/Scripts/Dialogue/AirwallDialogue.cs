using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirwallDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    private DialogueTrigger dialogueTrigger;
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")){
            dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            dialogueTrigger.TriggerDialogue();
        }
    }
}
