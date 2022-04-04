using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    { 
        if (Mathf.Abs(player.transform.position.x) < 15.5 && Mathf.Abs(player.transform.position.z) < 15.5)
        transform.position = player.transform.position + offset;
    }
}
