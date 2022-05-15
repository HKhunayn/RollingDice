
using UnityEngine;

public class camera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cam;
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        cam.position = player.transform.position;
    }
}
