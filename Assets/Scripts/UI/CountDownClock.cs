using ORZ.Enemy.DemonWill;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountDownClock : MonoBehaviour
{
    int currentTime = 0;
    private int countSignal = 0;
    [SerializeField] int startTime = 300;

    [SerializeField] TextMeshProUGUI countDownText;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startTime;
        SetTime();
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            CountDown();
        }
    }

    void CountDown()
    {
        if (!countDownText.IsActive()) return;
        currentTime -= 1;
        countSignal += 1;
        if (currentTime <= 0)
        {
            currentTime = 0;
        }

        if (countSignal == 60)
        {
            GameObject demonWill = GameObject.Find("DemonWill");
            if (demonWill != null)
            {
                demonWill.GetComponent<DWController>().Accelerate();
                SoundController.Instance.PlaySpecSound("Crazy");
            }
            countSignal = 0;
        }

        if (currentTime == 60)
        {
            SoundController.Instance.PlaySpecBGM("BGM3");
            SoundController.Instance.transform.Find("Clock").GetComponent<AudioSource>().Play();
        }
        if (currentTime <= 60)
        {
            countDownText.color = Color.red;
        }
        SetTime();
    }

    void SetTime()
    {
        TimeSpan ts = TimeSpan.FromSeconds(currentTime);
        countDownText.text = $"{ts.Minutes}:{ts.Seconds:D2}";
    }
}
