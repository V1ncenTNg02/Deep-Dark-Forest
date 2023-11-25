using UnityEngine;

public class PlayParticles : MonoBehaviour
{
    public float duration = 2.0f;
    private ParticleSystem[] particleSystems;
    // Start is called before the first frame update
    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
        Destroy(gameObject, duration);
    }
}
