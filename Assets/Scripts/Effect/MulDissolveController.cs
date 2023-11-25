using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MulDissolveController : MonoBehaviour
{
    public float dissolveDuration = 2.0f;  // Duration for the dissolve effect
    private float startTime;
    private Renderer rend;
    private bool isDissolving = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        //StartDissolve();
    }

    public void StartDissolve()
    {
        startTime = Time.time;
        isDissolving = true;
        StartCoroutine(Dissolving());
    }

    IEnumerator Dissolving(){
        while (isDissolving){
            float process = (Time.time - startTime) / dissolveDuration;
            foreach (var mat in rend.materials)
            {
                mat.SetFloat("_DissolveThreshold", process);
            }
            if (process >= 1.0f)
            {
                isDissolving = false;  // End the dissolve effect once it's complete
                Destroy(gameObject);
            }
            yield return null;
        }
        
    }
}

