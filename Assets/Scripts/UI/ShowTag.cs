using UnityEngine;

public class ShowTag : MonoBehaviour
{
    [SerializeField] private GameObject tmp; 
    // Start is called before the first frame update
    void Start()
    {
        tmp.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tmp.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tmp.SetActive(false);
        }
    }
}
