using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Acts as our way of terminating particles and stuff that need to destroy themselves a certain amount of time after spawning.
public class TimedEffect : MonoBehaviour {
    public float killTime = 1.0f; //the time it takes before this self-terminates.
    float timeSinceAwake = 0.0f;

	// Update is called once per frame
	void Update () {
        timeSinceAwake += Time.deltaTime;
        if (timeSinceAwake > killTime)
        {
            Destroy(gameObject);
        }
	}
}
