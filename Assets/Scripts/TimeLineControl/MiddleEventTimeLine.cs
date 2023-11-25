using ORZ;
using ORZ.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MiddleEventTimeLine : MonoBehaviour
{
    [Tooltip("Control GameObjects")]
    public GameObject Camera;
    public GameObject Enemies;
    public GameObject AirWalls;
    public GameObject CountDown;
    public GameObject BlackMask;
    public GameObject DiaryUI;
    public GameObject Inventory;
    public GameObject StaminaBar;
    public GameObject DirectionInstruction;
    public GameObject Pause;

    private DialogueTrigger[] dialogues;

    [Tooltip("Detail Settings")]
    public float DimRate = 2.0f;
    public float PlayerAutoSpeedToCenter = 3.0f;
    public float DemonWillSpawnSpeed = 1.0f;
    
    private bool entireBlack = false;
    private bool playerReachCenter = false;
    private bool demonWillSpawned = false;

    void Start()
    {
        dialogues = transform.Find("Dialogue").GetComponents<DialogueTrigger>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            entireBlack = false;
            playerReachCenter = false;
            demonWillSpawned = false;
            StartCoroutine(Play());
            GetComponent<Collider>().enabled = false;
        }
    }

    IEnumerator Play()
    {
        Debug.Log("Start Center Events");
        CameraController camController = Camera.transform.Find("Main Camera").GetComponent<CameraController>();

        // Dim Scene when enter the center
        BlackMask.SetActive(true);
        BlackMask.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        StartCoroutine(DimScene());
        yield return new WaitUntil(() => entireBlack);

        // Ready processes
        DiaryUI.SetActive(false);
        Inventory.SetActive(false);
        StaminaBar.SetActive(false);
        DirectionInstruction.SetActive(false);
        Pause.GetComponent<Pause>().InEvent = true;

        Destroy(StaminaBar.transform.Find("Shift To Run").gameObject);
        ObjectGetter.player.GetComponent<PlayerController>().enabled = false;

        // Player Walk to center
        ObjectGetter.player.GetComponent<PlayerAnimationController>().Walk();
        StartCoroutine(PlayerToCenter());
        StartCoroutine(BrightScene());

        StartCoroutine(RingBell());
        yield return new WaitUntil(() => playerReachCenter && !entireBlack);

        // Play first dialogue
        BlackMask.SetActive(false);
        dialogues[0].TriggerDialogue();
        yield return new WaitUntil(() => !ObjectGetter.dialogueManager.playingDialogue);

        // Ring the bell and spawn all enemies, camera to DemonWill
        ObjectGetter.player.transform.rotation = new Quaternion(0, 220, 0, 0);
        camController.SetFree(true);
        Camera.GetComponent<Animator>().SetTrigger("TransToDemon");
        yield return new WaitUntil(() => camController.OnMonster);

        // Spawn the Enemies
        Enemies.SetActive(true);
        yield return null;

        ObjectGetter.timeController.FreezeAllEnemies();
        StartCoroutine(SpawnDemonWill());
        yield return new WaitUntil(() => demonWillSpawned);

        // Camera back to player
        Camera.GetComponent<Animator>().SetTrigger("TransToPlayer");
        yield return new WaitUntil(() => camController.OnPlayer);
        Enemies.transform.Find("DemonWill").position += Vector3.down;

        // Play second dialogue
        camController.SetFree(false);
        dialogues[1].TriggerDialogue();
        yield return new WaitUntil(() => !ObjectGetter.dialogueManager.playingDialogue);

        DiaryUI.SetActive(true);
        Inventory.SetActive(true);
        StaminaBar.SetActive(true);
        CountDown.SetActive(true);
        ObjectGetter.player.GetComponent<PlayerController>().enabled = true;
        Destroy(AirWalls);
        ObjectGetter.timeController.UnFreezeAllEnemies();

        SoundController.Instance.RandomPlayEnemySound();
        SoundController.Instance.PlaySpecBGM("BGM1");
        Pause.GetComponent<Pause>().InEvent = false;
        gameObject.SetActive(false);

        Debug.Log("Middle Events End!");
    }

    IEnumerator DimScene()
    {
        Color color = BlackMask.GetComponent<Image>().color;
        while (!entireBlack)
        {
            color += new Color(0, 0, 0, 1) * Time.deltaTime * DimRate;
            BlackMask.GetComponent<Image>().color = color;
            if (color.a >= 1.0f)
            {
                entireBlack = true;
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator BrightScene()
    {
        Color color = BlackMask.GetComponent<Image>().color;
        while (entireBlack)
        {
            color -= new Color(0, 0, 0, 1) * Time.deltaTime * DimRate;
            BlackMask.GetComponent<Image>().color = color;
            if (color.a <= 0.0f)
            {
                entireBlack = false;
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator PlayerToCenter()
    {
        Vector3 start = new Vector3(8.0f, 0, -8.0f);
        ObjectGetter.player.transform.localPosition = start;
        ObjectGetter.player.transform.localEulerAngles = new Vector3(0, -45.0f, 0);
        while (!playerReachCenter)
        {
            ObjectGetter.player.transform.Translate(transform.forward * Time.fixedDeltaTime * PlayerAutoSpeedToCenter);
            if (Vector3.Distance(ObjectGetter.player.transform.localPosition, Vector3.zero) <= 3.0f)
            {
                ObjectGetter.player.GetComponent<PlayerAnimationController>().Idle();
                ObjectGetter.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                playerReachCenter = true;
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator SpawnDemonWill()
    {
        GameObject demonWill = Enemies.transform.Find("DemonWill").gameObject;
        GameObject spawnPlatform = Enemies.transform.Find("SpawnPoint").gameObject;
        Vector3 initialScale = spawnPlatform.transform.localScale;
        bool played = false;
        while (!demonWillSpawned)
        {
            demonWill.transform.Translate(Vector3.up * DemonWillSpawnSpeed * Time.deltaTime);
            float y = demonWill.transform.position.y;
            if (y >= 0 && !played)
            {
                SoundController.Instance.PlaySpecSound("DemonRoar");
                played = true;
            }
            if(y >= 2) { 
                demonWillSpawned = true;
            }
            float t = y < 0 ? Mathf.Lerp(1.0f, 10.0f, 1 - demonWill.transform.position.y / (-4)) : 
                              Mathf.Lerp(0.0f, 10.0f, 1 - y / 2);
            spawnPlatform.transform.localScale = initialScale * t;

            yield return null;
        }
    }

    IEnumerator RingBell()
    {
        int count = 0;
        while (count < 6)
        {
            count++;
            SoundController.Instance.PlaySpecSound("Bell");
            yield return new WaitForSeconds(5.0f);
        }
    }
}
