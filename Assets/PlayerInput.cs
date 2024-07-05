using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private float cameraBorder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction.x = -1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.x = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.z = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction.z = 1;
        }
        direction = direction.normalized * Time.deltaTime * cameraMoveSpeed;
        Vector3 camPosition = new Vector3(
            Mathf.Clamp(cam.transform.position.x + direction.x, cameraBorder, WorldMap.SQRT_OF_MAP_SIZE + cameraBorder),
            cam.transform.position.y,
            Mathf.Clamp(cam.transform.position.z + direction.z, cameraBorder, WorldMap.SQRT_OF_MAP_SIZE - cameraBorder)
            );
        cam.transform.position = camPosition;
    }
}
