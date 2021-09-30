using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour
{
    [SerializeField] float cameraMoveSpeed = 5f;
    [SerializeField] float minXDistanceFromBorder = 150f;
    [SerializeField] float minYDistanceFromBorder = 150f;


    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = new Vector3(0, 0, 0);

        // Hovering Left
        if (Input.mousePosition.x < minXDistanceFromBorder)
            moveVector.x = -cameraMoveSpeed;
        // Hovering Right
        else if (Input.mousePosition.x > Screen.width - minXDistanceFromBorder)
            moveVector.x = cameraMoveSpeed;

        // Hovering Up
        if (Input.mousePosition.y > Screen.height - minYDistanceFromBorder)
            moveVector.y = cameraMoveSpeed;
        else if (Input.mousePosition.y < minYDistanceFromBorder)
            moveVector.y = -cameraMoveSpeed;

        transform.position += moveVector * Time.deltaTime;
    }
}
