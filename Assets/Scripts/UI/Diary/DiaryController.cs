using ORZ;
using ORZ.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class DiaryController : MonoBehaviour
{
    [SerializeField] private List<GameObject> ItemContents;
    public GameObject DiaryOpen;
    public GameObject DiaryClose;
    public GameObject Circle;
    public GameObject Diary;
    public GameObject Blur;
    public bool IsOpen = false;
    public bool IsActivate = false;

    void Start()
    {
        foreach (var item in ItemContents)
        {
            item.SetActive(false);
        }
        ItemContents[0].SetActive(true);
        Circle.transform.localPosition = ItemContents[0].transform.parent.localPosition;
        DiaryOpen.SetActive(false);
        DiaryClose.SetActive(true);
        Diary.SetActive(IsActivate);
    }

    void Update()
    {
        if (IsActivate && Input.GetButtonDown("OpenDiary"))
        {
            OpenOrCloseDiary();
        }
    }

    public void OpenOrCloseDiary()
    {
        ObjectGetter.player.GetComponent<PlayerAnimationController>().Idle();
        ObjectGetter.player.GetComponent<PlayerSoundController>().StopAudio();
        Blur.SetActive(!Blur.activeSelf);
        ChangeDiaryIcon(!IsOpen);
        SoundController.Instance.PlaySpecSound(IsOpen ? "Open" : "Close");
    }

    public void CloseDiary()
    {
        ObjectGetter.player.GetComponent<PlayerAnimationController>().Idle();
        ObjectGetter.player.GetComponent<PlayerSoundController>().StopAudio();
        ChangeDiaryIcon(false);
    }

    public void ChangeDiaryIcon(bool b)
    {
        DiaryClose.SetActive(!b);
        DiaryOpen.SetActive(b);
        IsOpen = b;
        Time.timeScale = Math.Abs(Time.timeScale - 1.0f) < 0.001f ? 0.0f : 1.0f;
    }
    public void ChangeToItem(int index)
    {
        int i = 0;
        while (i < ItemContents.Count)
        {
            ItemContents[i].SetActive(i == index);
            if (i == index)
            {
                Circle.transform.localPosition = ItemContents[i].transform.parent.localPosition;
            }
            i++;
        }
    }

    public void ActivateItem(int index)
    {
        ItemContents[index].transform.Find("Content").gameObject.SetActive(true);
        ItemContents[index].transform.Find("Hide").gameObject.SetActive(false);
        ItemContents[index].transform.parent.Find("Icon").Find("Icon").GetComponent<Image>().color = Color.white;
        ItemContents[index].transform.parent.Find("Icon").Find("Hide").gameObject.SetActive(false);
    }

    public void ActivateDiary()
    {
        Diary.SetActive(true);
        IsActivate = true;
    }
}
