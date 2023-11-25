using System;
using TMPro;
using UnityEngine;

public class ShowNameTag : MonoBehaviour
{
    private GameObject nameTag;
    public String floatName = "Press \"E\" ";

    // Start is called before the first frame update
    void Awake()
    {
        nameTag = transform.Find("nameTag").gameObject;
        nameTag.GetComponent<TextMeshPro>().text = floatName;
        nameTag.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nameTag.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nameTag.SetActive(false);
        }
    }
}
