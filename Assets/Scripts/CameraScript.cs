using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Camera _camera;
    private LocalWorld _localWorld;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        _localWorld = GameObject.Find("LocalWorld").GetComponent<LocalWorld>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (transform.position.y < _localWorld.TileCountY)
                transform.position += new Vector3(0, 0.1f * _camera.orthographicSize, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (transform.position.x > 0)
                transform.position += new Vector3(-0.1f * _camera.orthographicSize, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (transform.position.y > 0)
                transform.position += new Vector3(0, -0.1f * _camera.orthographicSize, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (transform.position.x < _localWorld.TileCountX)
                transform.position += new Vector3(0.1f * _camera.orthographicSize, 0, 0);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (_camera.orthographicSize > 0.5f)
                _camera.orthographicSize -= 0.25f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (_camera.orthographicSize < 20f)
                _camera.orthographicSize += 0.25f;
        }
    }
}
