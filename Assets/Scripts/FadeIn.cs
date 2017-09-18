using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {

    SpriteRenderer render;
    float value;

	// Use this for initialization
	void Start () {
        render = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Color temp = render.color;
        temp.a += Time.deltaTime / 2;
        render.color = temp;
	}
}