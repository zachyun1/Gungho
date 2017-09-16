﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour {

    public float speed = 10.0f;
    public KeyCode keyRight = KeyCode.RightArrow;
    public KeyCode keyLeft = KeyCode.LeftArrow;
    public KeyCode keyUp = KeyCode.UpArrow;
    public KeyCode keyDown = KeyCode.DownArrow;

    public KeyCode keyD = KeyCode.D;
    public KeyCode keyA = KeyCode.A;
    public KeyCode keyW = KeyCode.W;
    public KeyCode keyS = KeyCode.S;

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(keyRight) || Input.GetKey(keyD))
        {
            transform.Translate(new Vector3(Time.deltaTime * speed, 0.0f, 0.0f));
        }
        if (Input.GetKey(keyLeft) || Input.GetKey(keyA))
        {
            transform.Translate(new Vector3(Time.deltaTime * -speed, 0.0f, 0.0f));
        }
        if (Input.GetKey(keyUp) || Input.GetKey(keyW))
        {
            transform.Translate(new Vector3(0.0f, Time.deltaTime * speed, 0.0f));
        }
        if (Input.GetKey(keyDown) || Input.GetKey(keyS))
        {
            transform.Translate(new Vector3(0.0f, Time.deltaTime * -speed, 0.0f));
        }
    }
}
