using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("This mouse button is being held down!");
        }

    }
}
