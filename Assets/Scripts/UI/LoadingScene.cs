using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingCircle;
    public List<Sprite> PicInstructions;
    public List<string> TextInstructions;

    public TextMeshProUGUI tmp;
    public Image image;
    public TextMeshProUGUI tmpProgress;

    public float CircleSpinSpeed;

    public float CircleFadeTime;
    public float TextFadeTime;
    public float TextRemainTime;
    public float PicFadeTime;
    public float PicRemainTime;

    private bool fadingInText = false;
    private bool fadingInPic = false;

    private AsyncOperation asyncLoad;

    public void LoadGame()
    {
        StartCoroutine(LoadGameScene());
    }


    IEnumerator LoadGameScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;

        Coroutine coroutineText = StartCoroutine(ChangeText());
        Coroutine coroutinePic = StartCoroutine(ChangePic());
        StartCoroutine(FadeInCircle());

        float process = 0.0f;
        tmpProgress.text = "0.0%";

        while (asyncLoad.progress < 0.9f || process < 100.0f)
        {
            LoadingCircle.transform.Rotate(Vector3.forward, Time.deltaTime * CircleSpinSpeed);
            process += Random.Range(0.0f, 1.0f);
            if (process > 100.0f)
            {
                process = 100.0f;
            }
            tmpProgress.text = $"{Mathf.Round(process)}%";
            yield return null;
        }

        tmpProgress.gameObject.SetActive(false);
        StartCoroutine(FadeoutCircle());
        StopCoroutine(coroutineText);
        tmp.text = "Press \"Space\" to Continue";
        tmp.color = Color.white;

        bool pressSpace = false;

        while (!pressSpace)
        {
            pressSpace = Input.GetKeyDown(KeyCode.Space);
            yield return null;
        }

        yield return null;
        asyncLoad.allowSceneActivation = true;
        StopAllCoroutines();
    }

    IEnumerator FadeInCircle()
    {
        LoadingCircle.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        float t = 0;
        while (t < 1)
        {
            float alpha = Mathf.Lerp(0, 1, t);
            t += Time.deltaTime / CircleFadeTime;
            LoadingCircle.GetComponent<Image>().color = new Color(1, 1, 1, alpha); 
            yield return null;
        }
    }

    IEnumerator FadeoutCircle()
    {
        LoadingCircle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        float t = 1;
        while (t > 0)
        {
            float alpha = Mathf.Lerp(0, 1, t);
            t -= Time.deltaTime / CircleFadeTime;
            LoadingCircle.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }

    IEnumerator ChangeText()
    {
        int i = 0;
        int l = TextInstructions.Count;
        tmp.color = new Color(1, 1, 1, 0);
        tmp.text = TextInstructions[i];

        while (true)
        {
            // Fade In Text
            StartCoroutine(FadeInText());
            yield return new WaitWhile(() => fadingInText);
            yield return null;
            if (l == 1)
            {
                yield break;
            }

            yield return new WaitForSeconds(TextRemainTime);
            
            // Fade Out Text
            StartCoroutine(FadeOutText());
            yield return new WaitUntil(() => fadingInText);
            yield return null;

            i = (i + 1) % l;
            tmp.text = TextInstructions[i];

            yield return null;
        }
    }

    IEnumerator FadeInText()
    {
        fadingInText = true;
        tmp.color = new Color(1, 1, 1, 0);
        float t = 0;
        while (t < 1)
        {
            float alpha = Mathf.Lerp(0, 1, t);
            t += Time.deltaTime / TextFadeTime;
            tmp.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        fadingInText = false;
    }

    IEnumerator FadeOutText()
    {
        fadingInText = false;
        tmp.color = new Color(1, 1, 1, 1);
        float t = 1;
        while (t > 0)
        {
            float alpha = Mathf.Lerp(0, 1, t);
            t -= Time.deltaTime / TextFadeTime;
            tmp.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        fadingInText = true;
    }

    IEnumerator ChangePic()
    {
        int i = 0;
        int l = PicInstructions.Count;
        image.color = new Color(1, 1, 1, 0);
        image.sprite = PicInstructions[i];

        while (true)
        {
            // Fade In Picture
            StartCoroutine(FadeInPic());
            yield return new WaitWhile(() => fadingInPic);
            yield return null;
            if (l == 1)
            {
                yield break;
            }

            yield return new WaitForSeconds(PicRemainTime);

            // Fade Out Picture
            StartCoroutine(FadeOutPic());
            yield return new WaitUntil(() => fadingInPic);
            yield return null;

            i = (i + 1) % l;
            image.sprite = PicInstructions[i];

            yield return null;
        }



    }

    IEnumerator FadeInPic()
    {
        fadingInPic = true;
        image.color = new Color(1, 1, 1, 0);
        float t = 0;
        while (t < 1)
        {
            float alpha = Mathf.Lerp(0, 1, t);
            t += Time.deltaTime / PicFadeTime;
            image.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        fadingInPic = false;
    }

    IEnumerator FadeOutPic()
    {
        fadingInPic = false;
        image.color = new Color(1, 1, 1, 1);
        float t = 1;
        while (t > 0)
        {
            float alpha = Mathf.Lerp(0, 1, t);
            t -= Time.deltaTime / PicFadeTime;
            image.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        fadingInPic = true;
    }
}
