using ORZ;
using ORZ.Item;
using UnityEngine;

public class Torch : ConsumeItem
{
    public GameObject detector;
    public GameObject fire;

    private bool canUse = true;
    public override void UseItem()
    {
        if (!canUse) return;
        GameObject detect = Instantiate(detector, ObjectGetter.player.transform.position, Quaternion.identity);
        GameObject newFire = Instantiate(fire, 
            ObjectGetter.player.transform.position + Vector3.up + ObjectGetter.player.transform.forward * 0.4f, 
            Quaternion.identity);
        newFire.GetComponent<ParticleSystem>().Play();
        SoundController.Instance.PlaySpecSound("Fire");
        canUse = false;
        Invoke(nameof(CanFire), 1.0f);
        Destroy(detect, 1.0f);
        Destroy(newFire, 1.0f);
    }

    void CanFire()
    {
        canUse = true;
    }
}
