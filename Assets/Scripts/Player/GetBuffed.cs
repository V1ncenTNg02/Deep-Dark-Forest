using ORZ.Player;
using UnityEngine;

public class GetBuffed : MonoBehaviour
{
    public float buffTime = 5f;
    private GameObject player;
    private PlayerController playerController;

    private float originSpeed;
    private float originRunSpeed;
    private GameObject TrailEffect;
    private ParticleSystem TrailEffectParticleSystem;
    [SerializeField] private float speedFactor = 10f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        originSpeed = playerController.getSpeed();
        originRunSpeed = playerController.getRunSpeed();
        TrailEffect = player.transform.Find("SpeedUpTrail").gameObject;
        TrailEffectParticleSystem = TrailEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame

    public void startBuff()
    {
        playerController.setSpeed(originSpeed * speedFactor);
        playerController.setRunSpeed(originRunSpeed * speedFactor);
        CancelInvoke(nameof(StopBuffLater));
        Invoke(nameof(StopBuffLater), buffTime);
        TrailEffectParticleSystem.Play();
    }

    void StopBuffLater()
    {
        stopBuff();
    }
    public void stopBuff()
    {
        playerController.setSpeed(originSpeed);
        playerController.setRunSpeed(originRunSpeed);
        TrailEffectParticleSystem.Stop();
    }
}
