using UnityEngine;

public class PlayAfterSomeTime : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeToWait = 12f;
    void Start()
    {
        Invoke(nameof(StartDialogueSomeLatency), timeToWait);
    }

    // Update is called once per frame
    void StartDialogueSomeLatency(){
        GetComponent<DialogueTrigger>().TriggerDialogue();
    }
}
