using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void Update()
    {
        float xPos = (int) (gameObject.transform.position.x / 0.25) * 0.25f;
        float yPos = (int) (gameObject.transform.position.y / 0.25) * 0.25f;

        gameObject.transform.position = new Vector3(xPos, yPos, gameObject.transform.position.z);
    }
}
