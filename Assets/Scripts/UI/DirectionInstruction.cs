using ORZ;
using UnityEngine;
using UnityEngine.UI;

public class DirectionInstruction : MonoBehaviour
{
    public Vector3 Target;

    void Start()
    {
        GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction3 = (Target - ObjectGetter.player.transform.position);
        Vector2 direction = new Vector2(direction3.x, direction3.z);

        if (direction.magnitude < 10.0f)
        {
            GetComponent<Image>().enabled = false;
            return;
        }
        else
        {
            GetComponent<Image>().enabled = true;
        }

        direction.Normalize();
        transform.localPosition = new Vector3(Screen.width * direction.x / 2, Screen.height * direction.y / 2, 0);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0, 0, -angle);

    }
}
