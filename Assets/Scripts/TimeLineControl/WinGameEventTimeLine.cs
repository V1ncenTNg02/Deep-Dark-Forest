using ORZ;
using ORZ.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinGameEventTimeLine : MonoBehaviour
{
    [Header("Control GameObjects")] public List<GameObject> Gems;
    public GameObject WinCam;
    public GameObject MainCam;
    public GameObject RotateCentral;
    public GameObject UI;
    public GameObject Diary;
    public GameObject Pause;
    public GameObject CountDownClock;
    public GameObject WhiteMask;
    public GameObject WinPanel;
    public GameObject Clock;
    public TextMeshProUGUI tmpWin;
    public Image WinPic;
    public TextMeshProUGUI WinPicDes;

    public GameObject[] Buttons;

    public AudioSource WinGameBGM;

    [Header("Detail Settings")] 
    public float rotateSpeed = 1.0f;
    public float moveUpSpeed = 1.0f;
    public float mergeSpeed = 1.0f;
    public float whiteTime = 5.0f;
    public float waitStartWhiteTime = 5.0f;
    public float transitionTime = 5.0f;

    public float textDuration = 3.0f;

    [TextArea(3,10)] public List<string> WinGameTextsList;
    [TextArea(3, 10)] public List<string> EndingTextsList;

    private bool reachLastTexts = false;
    private bool completeWhite = false;
    private bool startMergeAndRotate = false;
    

#if false
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);
        Win();
    }
#endif

    public void Win()
    {
        reachLastTexts = false;
        completeWhite = false;
        startMergeAndRotate = false;
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        SoundController.Instance.StopEnemySound();
        ObjectGetter.player.GetComponent<PlayerController>().enabled = false;
        UI.SetActive(false);
        Diary.SetActive(false);
        CountDownClock.SetActive(false);
        Pause.GetComponent<Pause>().InEvent = true;
        ObjectGetter.player.GetComponent<PlayerAnimationController>().Idle();

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector3.Distance(enemy.transform.position, ObjectGetter.player.transform.position) <= 2f)
            {
                Vector3 direction =  enemy.transform.position - ObjectGetter.player.transform.position;
                direction.Normalize();

                enemy.transform.position += new Vector3(direction.x * 5, 0, direction.z * 5);
            }
        }

        ObjectGetter.timeController.FreezeAllEnemies();

        // Set all Gems' parent object to rotateCentral
        foreach (var gem in Gems)
        {
            gem.SetActive(true);
            gem.transform.SetParent(RotateCentral.transform);
        }
        Clock.GetComponent<MulDissolveController>().StartDissolve();

        SoundController.Instance.PauseBGM();
        SoundController.Instance.PauseAllSounds();

        SoundController.Instance.PlaySpecSound("WinGame1");
        // Camera gradually move to WinCamera
        StartCoroutine(SwitchToWinCam());

        //Start Rotation & Start Slowly Move up
        StartCoroutine(RotateAndMoveUp());

        yield return new WaitUntil(() => completeWhite);

        UI.SetActive(true);
        Pause.GetComponent<Pause>().InEvent = false;
        Pause.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        ObjectGetter.player.GetComponent<PlayerController>().enabled = true;
        ObjectGetter.player.SetActive(false);
        WinPanel.SetActive(true);

        StartCoroutine(DimWinPanel());
        StartCoroutine(PlayWinTexts());

        yield return new WaitUntil(() => reachLastTexts);
        WinGameBGM.Play();

        StartCoroutine(PlayEndingTexts());
        foreach (var b in Buttons)
        {
            b.SetActive(true);
        }
    }

    IEnumerator RotateAndMoveUp()
    {
        StartCoroutine(White());
        Invoke(nameof(StartMergeAndRotate), 3.0f);
        while (!completeWhite)
        {
            if (RotateCentral.transform.position.y < 4.0f)
            {
                RotateCentral.transform.Translate(startMergeAndRotate ? 
                    Vector3.up * moveUpSpeed * Time.fixedDeltaTime : 
                    Vector3.up * Time.fixedDeltaTime * 0.7f);
            }

            // Get the direction of gem to middle
            if (startMergeAndRotate)
            {
                foreach (var gem in Gems)
                {
                    if (gem.transform.position.magnitude < 0.3f) continue;
                    Vector3 direction = Vector3.zero - gem.transform.localPosition;
                    direction.Normalize();
                    gem.transform.Translate(direction * mergeSpeed * Time.fixedDeltaTime);
                }

                RotateCentral.transform.Rotate(Vector3.up, rotateSpeed);
                rotateSpeed += Time.fixedDeltaTime;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void StartMergeAndRotate()
    {
        startMergeAndRotate = true;
    }

    // White Process
    IEnumerator White()
    {
        yield return new WaitForSeconds(waitStartWhiteTime);
        WhiteMask.SetActive(true);

        float t = 0;
        while (t < 1)
        {
            float alpha = Mathf.Lerp(0.0f, 1.0f, t);
            WhiteMask.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            t += Time.fixedDeltaTime / whiteTime;
            yield return new WaitForFixedUpdate();
        }

        completeWhite = true;
    }


    private IEnumerator SwitchToWinCam()
    {
        GameObject mainCamera = MainCam.transform.Find("Main Camera").gameObject;
        GameObject internalCamera = MainCam.transform.Find("Internal Camera").gameObject;

        mainCamera.SetActive(false);
        internalCamera.SetActive(true);
        bool finishTransitToPuzzleTransition = false;
        float t = 0;

        float isNeg = WinCam.transform.eulerAngles.y < 180 ? 1.0f : -1.0f;
        float rotateY = 360 - WinCam.transform.eulerAngles.y < 180
            ? 360 - WinCam.transform.eulerAngles.y
            : WinCam.transform.eulerAngles.y;

        while (!finishTransitToPuzzleTransition)
        {
            Vector3 position = Vector3.Lerp(mainCamera.transform.position, WinCam.transform.position, t);
            float x = Mathf.Lerp(60.0f, WinCam.transform.eulerAngles.x, t);
            float y = isNeg * Mathf.Lerp(0.0f, rotateY, t);
            t += Time.fixedDeltaTime / transitionTime;
            internalCamera.transform.position = position;
            internalCamera.transform.eulerAngles = new Vector3(x, y, 0);
            if (t >= 1)
            {
                t = 1;
                internalCamera.transform.position = WinCam.transform.position;
                internalCamera.transform.eulerAngles = WinCam.transform.eulerAngles;
                finishTransitToPuzzleTransition = true;
            }

            yield return new WaitForFixedUpdate();
        }

        internalCamera.SetActive(false);
        MainCam.SetActive(false);
        WinCam.SetActive(true);
    }

    IEnumerator DimWinPanel()
    {
        WinPanel.GetComponent<Image>().color = Color.white;
        while (true)
        {
            WinPanel.GetComponent<Image>().color -= new Color(1,1,1,0) * Time.deltaTime;
            if (WinPanel.GetComponent<Image>().color.r < 0.8f)
            {
                break;
            }

            yield return null;
        }
        
    }

    IEnumerator PlayWinTexts()
    {
        float alpha = 0;
        float t = 0;
        foreach (var text in WinGameTextsList)
        {
            alpha = 0.0f;
            t = 0.0f;
            tmpWin.text = text;
            tmpWin.color = new Color(0, 0, 0, 0);
            // Text Fade In
            while (t <= 1.0f)
            {
                alpha = Mathf.Lerp(0.0f, 1.0f, t);
                t += Time.deltaTime;
                tmpWin.color = new Color(0,0,0,alpha);
                yield return null;
            }

            t = 1.0f;
            alpha = 1.0f;

            yield return new WaitForSeconds(textDuration);

            while (t >= 0.0f)
            {
                alpha = Mathf.Lerp(0.0f, 1.0f, t);
                t -= Time.deltaTime;
                tmpWin.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        t = 0.0f;
        alpha = 0.0f;
        while (t <= 1.0f)
        {
            alpha = Mathf.Lerp(0.0f, 1.0f, t);
            t += Time.deltaTime;
            WinPic.color = new Color(80.0f / 255.0f, 30.0f / 255.0f, 30.0f / 255.0f, alpha);
            WinPicDes.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(textDuration);

        t = 1.0f;
        alpha = 1.0f;
        while (t >= 0.0f)
        {
            alpha = Mathf.Lerp(0.0f, 1.0f, t);
            t -= Time.deltaTime;
            WinPic.color = new Color(80.0f / 255.0f, 30.0f / 255.0f, 30.0f / 255.0f, alpha);
            WinPicDes.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        yield return null;

        WinPic.gameObject.SetActive(false);
        WinPicDes.gameObject.SetActive(false);
        reachLastTexts = true;
    }

    IEnumerator PlayEndingTexts()
    {
        float alpha = 0;
        float t = 0;
        foreach (var text in EndingTextsList)
        {
            alpha = 0.0f;
            t = 0.0f;
            tmpWin.text = text;
            tmpWin.color = new Color(0, 0, 0, 0);
            // Text Fade In
            while (t <= 1.0f)
            {
                alpha = Mathf.Lerp(0.0f, 1.0f, t);
                t += Time.deltaTime;
                tmpWin.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            t = 1.0f;
            alpha = 1.0f;

            yield return new WaitForSeconds(textDuration);

            while (t >= 0.0f)
            {
                alpha = Mathf.Lerp(0.0f, 1.0f, t);
                t -= Time.deltaTime;
                tmpWin.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }
    }
}
