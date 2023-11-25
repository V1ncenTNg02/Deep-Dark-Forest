using System.Collections;
using UnityEngine;

public class RockPuzzlePushRock : MonoBehaviour
{

    private RockPuzzleSolution rockPuzzleSolution;
    public Flammable torch;
    private float rotationSpeed = 30f;
    private bool solved = false;

    void Start()
    {
        rockPuzzleSolution = transform.parent.GetComponent<RockPuzzleSolution>();
    }

    void OnTriggerStay(Collider otherCollider)
    {
        GameObject otherObject = otherCollider.gameObject;
        if (otherObject.CompareTag("RockPuzzlePattern"))
        {
            // Avoid the object directly abstract to the target
            if (Vector3.Distance(otherObject.transform.position, transform.position) >= 1.0f || solved) return;
            Debug.Log("Trigger");
            CheckCorrectness(otherObject);
        }
    }

    void CheckCorrectness(GameObject pattern)
    {
        if (name == pattern.name){
            solved = true;
            transform.position = pattern.transform.position + new Vector3(0, transform.position.y + 0.05f, 0);
            transform.rotation = new Quaternion(0, pattern.transform.rotation.y,0,0);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            StartCoroutine(RotateContinuously());
            rockPuzzleSolution.numRockSolved += 1;
            rockPuzzleSolution.CheckifSolved();
            LightUpTorches();
        }
    }
    IEnumerator RotateContinuously()
    {
        while (true)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;

            // Apply rotation to the GameObject along the Y-axis
            transform.Rotate(0, rotationAmount, 0);

            yield return null; // Wait for the next frame
        }
    }

    void LightUpTorches()
    {
        torch.LightUp();
        this.enabled = false;
    }
}
