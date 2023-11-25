using System.Collections;
using UnityEngine;

public class Flammable : MonoBehaviour
{
    const float FLAME_FOREVER = 0;
    public bool isFlammableNow = true;
    public GameObject flame;
    public float flameTime = FLAME_FOREVER;
    public bool firing = false;
    public GameObject PointLight;

    public void Start()
    {
        flame.SetActive(false);
        PointLight.SetActive(false);
    }

    public void LightUp()
    {
        switch (isFlammableNow && !firing)
        {
            case true when  flameTime == FLAME_FOREVER:
                activateFireAndLight();
                break;
            case true when flameTime != FLAME_FOREVER:
                StartCoroutine(StartAndStopFiring());
                break;
        }
    }

    public void Extinguish(){
        if (firing){
            flame.GetComponent<ParticleSystem>().Stop();
            firing = false;
        }
    }

    public void activateFireAndLight()
    {
        flame.SetActive(true);
        PointLight.SetActive(true);
        flame.GetComponent<ParticleSystem>().Play();
        firing = true;
        GetComponent<AudioSource>().Play();
    }

    public void deActivateFireAndLight()
    {
        flame.GetComponent<ParticleSystem>().Stop();
        flame.SetActive(false);
        PointLight.SetActive(false);
        firing = false;
        GetComponent<AudioSource>().Stop();
    }

    IEnumerator StartAndStopFiring(){
        activateFireAndLight();
        yield return new WaitForSeconds(flameTime);
        deActivateFireAndLight();
    }

}
