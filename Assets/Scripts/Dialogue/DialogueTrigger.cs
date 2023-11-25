using ORZ;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    SphereCollider sphereCollider;

    public void TriggerDialogue()
    {
        StartCoroutine(ObjectGetter.dialogueManager.StartDialogue(dialogue));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue();
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
