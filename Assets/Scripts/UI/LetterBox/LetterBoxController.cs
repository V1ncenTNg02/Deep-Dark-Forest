using System.Collections;
using UnityEngine;

public class LetterBoxController : MonoBehaviour
{
    [SerializeField] private GameObject BlackSides;
    [SerializeField] private float letterBoxWidth = 140.0f;
    [SerializeField] [Range(0.0f, 3.0f)] private float fadeSpeed = 1.0f;

    private GameObject pannelUp;
    private GameObject pannelDown;

    private float t = 0;
    public float T => t;
    void Start()
    {
        pannelUp = BlackSides.transform.Find("PanelUp").gameObject;
        pannelDown = BlackSides.transform.Find("PanelDown").gameObject;
    }

    public void BlackSideFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public void BlackSideFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        while (t > 0)
        {
            float height = Mathf.Lerp(0, letterBoxWidth, t);
            pannelUp.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, height);
            pannelDown.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, height);
            t -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        BlackSides.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        BlackSides.SetActive(true);
        while (t < 1)
        {
            float height = Mathf.Lerp(0, letterBoxWidth, t);
            pannelUp.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, height);
            pannelDown.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, height);
            t += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}
