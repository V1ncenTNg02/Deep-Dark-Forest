using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlowingController : MonoBehaviour
{
    private Renderer rend;

    [Header("Shader Control")]
    public Color firstStageColor;
    public float firstStagePulseRange;
    public Color secondStageColor;
    public float secondStagePulseRange;
    public Color thirdStageColor;
    public float thirdStagePulseRange;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetColor("_GlowColor", firstStageColor);
        rend.material.SetFloat("_PulseRange", firstStagePulseRange);
    }

    public void Glowing(int level){
        switch (level)
        {
            case 3:
            {
                rend.material.SetColor("_GlowColor", secondStageColor);
                rend.material.SetFloat("_PulseRange", secondStagePulseRange);
                break;
            }
            case 4:
            {
                rend.material.SetColor("_GlowColor", thirdStageColor);
                rend.material.SetFloat("_PulseRange", thirdStagePulseRange);
                break;
            }
        }
        
        
        
    }
 }
