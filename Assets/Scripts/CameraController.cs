using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _dragSpeed = 1.5f;
    private Vector3 _previousMousePosition;
    private bool _isDragging = false;

    void Update()
    {
        CameraMovement();
        CameraZoom();
    }

    public void CameraZoom()
    {
        Camera.main.orthographicSize += -Input.mouseScrollDelta.y * Time.deltaTime * 10;
    }

    public void CameraMovement()
    {
        _dragSpeed = Camera.main.orthographicSize / 2;
        if (Input.GetMouseButtonDown(1))
        {
            _previousMousePosition = Input.mousePosition;
            _isDragging = true;
            return;
        }

        if (Input.GetMouseButtonUp(1))
        {
            _isDragging = false;
        }

        if (_isDragging && Input.GetMouseButton(1))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - _previousMousePosition;

            if (mouseDelta != Vector3.zero)
            {
                Vector3 move = new Vector3(mouseDelta.x * _dragSpeed, mouseDelta.y * _dragSpeed, 0) * Time.deltaTime;
                transform.Translate(-move, Space.World);
                _previousMousePosition = currentMousePosition;
            }
        }
    }

    /*public void Update()
    {
        float xPos = (int) (gameObject.transform.position.x / 0.25) * 0.25f;
        float yPos = (int) (gameObject.transform.position.y / 0.25) * 0.25f;

        gameObject.transform.position = new Vector3(xPos, yPos, gameObject.transform.position.z);
    }*/
}
