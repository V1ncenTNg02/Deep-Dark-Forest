using ORZ;
using ORZ.Player;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private GameObject _targetDoor;
    public GameObject MagicCircle;
    private bool _canTeleport = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("tag").gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canTeleport = true;
            transform.Find("tag").gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canTeleport = false;
            transform.Find("tag").gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (_canTeleport)
        {
            if (Input.GetButtonDown("Interact"))
            {
                ObjectGetter.player.GetComponent<PlayerController>().CanMove = false;
                ObjectGetter.player.GetComponent<PlayerAnimationController>().Idle();
                MagicCircle.SetActive(true);
                SoundController.Instance.PlaySpecSound("Teleport");
                Invoke(nameof(Tele), 1.5f);
            }
        }
    }

    void Tele()
    {
        Vector3 targetPos = _targetDoor.transform.position;
        ObjectGetter.player.transform.position = targetPos + new Vector3(0f, 0.09f, 0f);
        ObjectGetter.player.GetComponent<PlayerController>().CanMove = true;
        MagicCircle.SetActive(false);
    }
}
