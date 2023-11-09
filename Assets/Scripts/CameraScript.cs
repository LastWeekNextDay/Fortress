using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private const float DRIFT = 0.001f;
    private const float DRIFT_SHIFT = 0.005f;

    private Camera _camera;
    private LocalWorld _localWorld;
    private float _cameraDrift = DRIFT;
    private float _cameraDriftShift = DRIFT_SHIFT;
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_cameraDrift != DRIFT_SHIFT)
            {
                _cameraDrift = DRIFT_SHIFT;
            }
        } 
        else 
        {
            if (_cameraDrift != DRIFT)
            {
                _cameraDrift = DRIFT;
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (transform.position.y < _localWorld.TileCountY)
                transform.position += new Vector3(0, _cameraDrift * _camera.orthographicSize, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (transform.position.x > 0)
                transform.position += new Vector3(-_cameraDrift * _camera.orthographicSize, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (transform.position.y > 0)
                transform.position += new Vector3(0, -_cameraDrift * _camera.orthographicSize, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (transform.position.x < _localWorld.TileCountX)
                transform.position += new Vector3(_cameraDrift * _camera.orthographicSize, 0, 0);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (_camera.orthographicSize > 0.5f)
                _camera.orthographicSize -= 0.25f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (_camera.orthographicSize < 30f)
                _camera.orthographicSize += 0.25f;
        }
        if (Input.mousePosition.x >= Screen.width - 1)
        {
            if (_cameraDrift != DRIFT_SHIFT)
            {
                _cameraDrift = DRIFT_SHIFT;
            }
            if (transform.position.x < _localWorld.TileCountX)
                transform.position += new Vector3(_cameraDrift * _camera.orthographicSize, 0, 0);
        } else if (Input.mousePosition.x >= Screen.width - 50)
        {
            if (_cameraDrift != DRIFT)
            {
                _cameraDrift = DRIFT;
            }
            if (transform.position.x < _localWorld.TileCountX)
                transform.position += new Vector3(_cameraDrift * _camera.orthographicSize, 0, 0);
        }
        if (Input.mousePosition.x <= 1)
        {
            if (_cameraDrift != DRIFT_SHIFT)
            {
                _cameraDrift = DRIFT_SHIFT;
            }
            if (transform.position.x > 0)
                transform.position += new Vector3(-_cameraDrift * _camera.orthographicSize, 0, 0);
        } else if (Input.mousePosition.x <= 50)
        {
            if (_cameraDrift != DRIFT)
            {
                _cameraDrift = DRIFT;
            }
            if (transform.position.x > 0)
                transform.position += new Vector3(-_cameraDrift * _camera.orthographicSize, 0, 0);
        }
        if (Input.mousePosition.y >= Screen.height - 1)
        {
            if (_cameraDrift != DRIFT_SHIFT)
            {
                _cameraDrift = DRIFT_SHIFT;
            }
            if (transform.position.y < _localWorld.TileCountY)
                transform.position += new Vector3(0, _cameraDrift * _camera.orthographicSize, 0);
        } else if (Input.mousePosition.y >= Screen.height - 50)
        {
            if (_cameraDrift != DRIFT)
            {
                _cameraDrift = DRIFT;
            }
            if (transform.position.y < _localWorld.TileCountY)
                transform.position += new Vector3(0, _cameraDrift * _camera.orthographicSize, 0);
        }
        if (Input.mousePosition.y <= 1)
        {
            if (_cameraDrift != DRIFT_SHIFT)
            {
                _cameraDrift = DRIFT_SHIFT;
            }
            if (transform.position.y > 0)
                transform.position += new Vector3(0, -_cameraDrift * _camera.orthographicSize, 0);
        } else if (Input.mousePosition.y <= 50)
        {
            if (_cameraDrift != DRIFT)
            {
                _cameraDrift = DRIFT;
            }
            if (transform.position.y > 0)
                transform.position += new Vector3(0, -_cameraDrift * _camera.orthographicSize, 0);
        }
    }
}
