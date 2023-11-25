using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour
{
    public float fadeInDuration = 2.0f; // Duration for the fade-in in seconds
    public float imageStayDuration = 2.0f; 
    private Image imageComponent;
    private float elapsed = 0f;
    public bool safeModeOn =false;

    //public GameObject YouDie;

    void Start()
    {
        if (!safeModeOn){
            imageComponent = GetComponent<Image>();
            if (imageComponent == null)
            {
                Debug.LogError("No Image component found on this GameObject.");
                return;
            }

            Color initialColor = imageComponent.color;
            imageComponent.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1.0f);

            StartCoroutine(FadeIn());
        }
        else{
            gameObject.SetActive(false);
        }
    }

    public void SetSafeMode()
    {
        safeModeOn = !safeModeOn;
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSecondsRealtime(imageStayDuration);
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsed / fadeInDuration);

            Color currentColor = imageComponent.color;
            imageComponent.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1.0f - normalizedTime);

            yield return null;
        }
        //YouDie.SetActive(true);
        gameObject.SetActive(false);
    }
}
