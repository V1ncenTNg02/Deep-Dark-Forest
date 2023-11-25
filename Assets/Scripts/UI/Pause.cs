using UnityEngine;

public class Pause : MonoBehaviour
{ 
    public GameObject Blur;
    public GameObject Setting;
    public GameObject Instructions;
    public GameObject ControlUI;
    public GameObject UI;
    public GameObject Diary;
    public GameObject Dialogue;
    private bool activate = false;

    public bool InEvent = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenOrDisableObject();
        }
    }

    public void OpenOrDisableObject()
    {
        if (!InEvent)
        {
            UI.SetActive(activate);
            Diary.SetActive(activate);
        }
        Dialogue.SetActive(activate);

        activate = !activate;
        Setting.SetActive(activate);
        Instructions.SetActive(false); 
        ControlUI.SetActive(false);
        
        bool diaryOpening = Diary.transform.Find("DiaryUI").Find("DiaryOpened").gameObject.activeSelf;
        Blur.SetActive(activate || diaryOpening);

        Time.timeScale = activate || diaryOpening ? 0.0f : 1.0f;

        if (activate)
        {
            SoundController.Instance.ChangeVolumeOfAllSounds(0.3f);
        }
        else
        {
            SoundController.Instance.ChangeVolumeOfAllSounds(1.0f);
        }
    }


}
