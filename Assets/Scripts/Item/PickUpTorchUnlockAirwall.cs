using UnityEngine;

public class PickUpTorchUnlockAirwall : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject airwall;
    private GameObject player;
    private bool inbound = false;

    void Start(){
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) inbound = true;
        }

    void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) inbound = false;
        }

    void Update()
        {
            if (inbound && Input.GetButtonDown("Interact"))
            {
                Destroy(airwall);
                Debug.Log("airwall unlocked");
                Destroy(gameObject);
            }
        }

    // void OnTriggerStay(Collider other){
    //     if (other.gameObject.tag == "Player"){
    //         if (Input.GetButtonDown("Interact")){
    //             airwall.SetActive(false);
    //             Debug.Log("airwall unlocked");
    //         }
    //     }
    // }
}
