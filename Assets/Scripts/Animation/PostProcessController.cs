using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessController : MonoBehaviour
{
    private PostProcessVolume ppv;

    [SerializeField] private float ev;
    public float EV
    {
        get => ev;
        set
        {
            ev = value;
            SetEV();
        }
    }

    void Start()
    {
        ppv = GetComponent<PostProcessVolume>();
    }

    public void SetEV()
    {
        ppv.profile.GetSetting<ColorGrading>().postExposure.value = ev;
    }

    public void StartEvent(string eve) => StartCoroutine(eve);
    public void StopEvent(string eve) => StopCoroutine(eve);
    public void StopAllEvent() => StopAllCoroutines();

    private IEnumerator EventEyesOpenStart()
    {
        while (true)
        {
            SetEV();
            yield return null;
        }
    }
}