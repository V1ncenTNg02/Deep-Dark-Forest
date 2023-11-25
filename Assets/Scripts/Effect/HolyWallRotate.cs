using UnityEngine;

public class HolyWallRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,1,0), rotateSpeed);
    }
}
