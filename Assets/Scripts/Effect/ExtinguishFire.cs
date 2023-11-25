using UnityEngine;

public class ExtinguishFire : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        Flammable flammable = otherObject.GetComponent<Flammable>();
        if (flammable){
            flammable.Extinguish();
        }
        Transform lightSourceTransform = otherObject.transform.Find("TorchLight(Clone)");
        if (lightSourceTransform != null)
        {
            Destroy(lightSourceTransform.gameObject);
        }    
    }
}
