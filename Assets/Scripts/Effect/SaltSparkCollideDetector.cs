using ORZ.Enemy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaltSparkCollideDetector : MonoBehaviour
{
    private ParticleSystem partSys;
    private List<ParticleSystem.Particle> particles;
    private List<GameObject> targets;

    [Range(0.5f, 10.0f)] [Tooltip("The collision radius of target")]
    [SerializeField] private float detectRadius = 2.0f;
    [Range(1.0f, 3.0f)] [Tooltip("Time to freeze Enemy")] [SerializeField]
    private float freezeTime = 3.0f;

    [Tooltip("The ParticleSystem prefab played when enemy has been frozen")]
    public GameObject freezePS_Prefab;

    // Start is called before the first frame update
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy").ToList();
    }

    void Update()
    {
        partSys = GetComponent<ParticleSystem>();
        ParticleSystem.Particle[] p = new ParticleSystem.Particle[partSys.main.maxParticles];
        int particleCount = partSys.GetParticles(p);
        particles = p.ToList();
        CollideDetector();
    }

    void CollideDetector()
    {
        foreach (var particle in particles)
        {
            foreach (var enemy in targets)
            {
                EnemyController ec = enemy.GetComponent<EnemyController>();
                if (ec == null || ec.IsFreezing || !DetectInBound(particle, enemy)) continue;
                StartCoroutine(ec.Freeze(freezeTime));
                GameObject freeze = Instantiate(freezePS_Prefab, Vector3.zero, Quaternion.identity, enemy.transform);
                freeze.transform.localPosition = Vector3.zero;
                freeze.transform.localScale = Vector3.one * 0.5f;
            }
        }
    }

    private bool DetectInBound(ParticleSystem.Particle particle, GameObject target)
    {
        return (target.transform.position - particle.position).magnitude < detectRadius;
    }
}
