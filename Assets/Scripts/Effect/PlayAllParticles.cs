using ORZ;
using UnityEngine;

public class PlayAllParticles : MonoBehaviour
{
    public GameObject particleSystemPrefab;
    private ParticleSystem[] particleSystems;

    // Update is called once per frame
    public void PlayAllChildParticles()
    {
        GameObject newParticleSystem = Instantiate(particleSystemPrefab, ObjectGetter.player.transform.position, Quaternion.identity);
        newParticleSystem.transform.rotation = ObjectGetter.player.transform.rotation;
        particleSystems = newParticleSystem.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
        Destroy(newParticleSystem, 2f);
    }

    public void LocalPlayAllChildParticles()
    {
        GameObject newParticleSystem = Instantiate(particleSystemPrefab, ObjectGetter.player.transform.position, Quaternion.identity, ObjectGetter.player.transform);
        newParticleSystem.transform.localPosition = Vector3.zero;
        particleSystems = newParticleSystem.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
        Destroy(newParticleSystem, 2f);
    }

    public void CreateObject()
    {
        GameObject newParticleSystem = Instantiate(particleSystemPrefab, ObjectGetter.player.transform.position, Quaternion.identity);
        Destroy(newParticleSystem, 2f);
    }

}
