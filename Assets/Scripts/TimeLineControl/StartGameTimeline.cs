using ORZ;
using ORZ.Player;
using System.Collections;
using UnityEngine;

public class StartGameTimeline : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Diary;
    public GameObject Map;
    public GameObject Inventory;
    public GameObject StaminaBar;
    public GameObject DiaryClose;
    public GameObject BlackMask;
    public GameObject DirectionInstruction;
    public GameObject DiaryUI;

    void Start()
    {
        StartCoroutine(Play());
        // Eyes Open Effect
        BlackMask.SetActive(true);
        
    }

    IEnumerator Play()
    {
        yield return null;

        Inventory.SetActive(false);
        StaminaBar.SetActive(false);
        Camera.GetComponent<Animator>().SetTrigger("StartGame");
        BlackMask.SetActive(false);
        Map.SetActive(false);
        // Some ready during opening eyes
        DialogueTrigger[] dialogueTriggers = GetComponents<DialogueTrigger>();
        ObjectGetter.player.GetComponent<PlayerController>().enabled = false;
        DisableDiary();
        yield return new WaitUntil(() => Camera.GetComponent<CameraController>().EyesOpened);

        // Opening Dialogue Start
        dialogueTriggers[0].TriggerDialogue();
        yield return new WaitUntil(() => !ObjectGetter.dialogueManager.playingDialogue);

        // Let Diary able to pick up
        EnableDiary();
        
        yield return new WaitUntil(() => DiaryClose.activeSelf);

        // Start second Dialogue when diary closed
        yield return null;
        DiaryUI.SetActive(false);
        dialogueTriggers[1].TriggerDialogue();
        yield return new WaitUntil(() => !ObjectGetter.dialogueManager.playingDialogue);

        // Player is able to move now, suppose to pick up the torch (but they don't have to)
        Inventory.SetActive(true);
        StaminaBar.SetActive(true);
        DirectionInstruction.SetActive(true);
        DiaryUI.SetActive(true);
        Map.SetActive(true);
        ObjectGetter.player.GetComponent<PlayerController>().enabled = true;

        // Camera move up and zoom out
        Camera.GetComponent<Animator>().SetTrigger("EnterRoad");
        gameObject.SetActive(false);

        Debug.Log("StartGame Events End!");
    }

    void DisableDiary()
    {
        Diary.GetComponent<Diary>().enabled = false;
        DiaryClose.SetActive(false);
        Diary.transform.GetChild(0).gameObject.SetActive(false);
        Diary.transform.GetChild(1).gameObject.SetActive(false);
    }

    void EnableDiary()
    {
        Diary.GetComponent<Diary>().enabled = true;
        Diary.transform.GetChild(0).gameObject.SetActive(true);
        Diary.transform.GetChild(1).gameObject.SetActive(true);
    }
}
