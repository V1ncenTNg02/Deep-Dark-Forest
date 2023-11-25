using ORZ;
using ORZ.Player;
using System.Collections;
using UnityEngine;

public class EnterPuzzleTimeLine : MonoBehaviour
{
    public GameObject Stamina;
    public GameObject Inventory;
    public GameObject DiaryUI;
    public GameObject CountDown;
    public GameObject MainCam;
    public GameObject OtherCam;
    public GameObject Pause;

    public float PreviewTime = 5.0f;

    [Tooltip("The time camera cost to transit from player to puzzle and puzzle back to player")]
    public float transitionTime = 1.0f;
    private bool previewEnd = false;

    private DialogueTrigger dialogue;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = transform.Find("Dialogue").GetComponent<DialogueTrigger>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            previewEnd = false;
            StartCoroutine(Play());
        }
    }

    IEnumerator Play()
    {
        Stamina.SetActive(false);
        Inventory.SetActive(false);
        CountDown.SetActive(false);
        DiaryUI.SetActive(false);
        Pause.GetComponent<Pause>().InEvent = true;
        ObjectGetter.player.GetComponent<PlayerAnimationController>().Idle();
        ObjectGetter.player.GetComponent<PlayerController>().enabled = false;
        ObjectGetter.timeController.FreezeAllEnemies();


        StartCoroutine(SwitchToTorchPuzzleCam());
        yield return new WaitUntil(() => previewEnd);
        dialogue.TriggerDialogue();

        yield return new WaitUntil(() => !ObjectGetter.dialogueManager.playingDialogue);
        Stamina.SetActive(true);
        Inventory.SetActive(true);
        CountDown.SetActive(true);
        DiaryUI.SetActive(true);
        ObjectGetter.player.GetComponent<PlayerController>().enabled = true;
        ObjectGetter.timeController.UnFreezeAllEnemies();

        transform.Find("Dialogue").gameObject.SetActive(false);
        Pause.GetComponent<Pause>().InEvent = false;
        Destroy(this);


    }

    private IEnumerator SwitchToTorchPuzzleCam()
    {
        GameObject mainCamera = MainCam.transform.Find("Main Camera").gameObject;
        GameObject internalCamera = MainCam.transform.Find("Internal Camera").gameObject;

        mainCamera.SetActive(false);
        internalCamera.SetActive(true);
        bool finishTransitToPuzzleTransition = false;
        float t = 0;

        float isNeg = OtherCam.transform.eulerAngles.y < 180 ? 1.0f : -1.0f;
        float rotateY = 360 - OtherCam.transform.eulerAngles.y < 180 ? 
            360 - OtherCam.transform.eulerAngles.y :
            OtherCam.transform.eulerAngles.y;

        while (!finishTransitToPuzzleTransition)
        {
            Vector3 position = Vector3.Lerp(mainCamera.transform.position, OtherCam.transform.position, t);
            float x = Mathf.Lerp(60.0f, OtherCam.transform.eulerAngles.x, t);
            float y = isNeg * Mathf.Lerp(0.0f, rotateY, t);
            t += Time.fixedDeltaTime / transitionTime;
            internalCamera.transform.position = position;
            internalCamera.transform.eulerAngles = new Vector3(x, y, 0);
            if (t >= 1)
            {
                t = 1;
                internalCamera.transform.position = OtherCam.transform.position;
                internalCamera.transform.eulerAngles = OtherCam.transform.eulerAngles;
                finishTransitToPuzzleTransition = true;
            }

            yield return new WaitForFixedUpdate();
        }

        internalCamera.SetActive(false);
        OtherCam.SetActive(true);
        yield return new WaitForSeconds(PreviewTime);

        internalCamera.SetActive(true);
        OtherCam.SetActive(false);
        bool finishBackToPlayerTransition = false;
        while (!finishBackToPlayerTransition)
        {
            Vector3 position = Vector3.Lerp(mainCamera.transform.position, OtherCam.transform.position, t);
            float x = Mathf.Lerp(60.0f, OtherCam.transform.eulerAngles.x, t);
            float y = isNeg * Mathf.Lerp(0.0f, rotateY, t);
            t -= Time.deltaTime / transitionTime;
            internalCamera.transform.position = position;
            internalCamera.transform.eulerAngles = new Vector3(x, y, 0);
            if (t <= 0)
            {
                t = 0;
                internalCamera.transform.position = mainCamera.transform.position;
                internalCamera.transform.eulerAngles = mainCamera.transform.eulerAngles;
                finishBackToPlayerTransition = true;
            }

            yield return null;
        }

        internalCamera.SetActive(false);
        mainCamera.SetActive(true);
        previewEnd = true;
    }
}
