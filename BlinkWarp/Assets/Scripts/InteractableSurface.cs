using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableSurface : MonoBehaviour {

    protected GameObject player;
    protected MaterialPropertyBlock materialProperties;
    public Vector3 normal;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	// Use this for initialization
	void Start () {
        
        materialProperties = new MaterialPropertyBlock();
        gameObject.GetComponent<Renderer>().GetPropertyBlock(materialProperties);


        //gameObject.GetComponent<Renderer>().material.mainTexture.wrapMode = TextureWrapMode.Repeat;
        //gets the parent wall object's "normal" direction, which is already in global directional coordinates
        //might need updating once we have walls in multiple directions on one wall gameobject or "air surfaces" without a parent gameobject
        normal = gameObject.transform.parent.up;

    }

    // Update is called once per frame
    //http://answers.unity3d.com/questions/529814/how-to-have-2-different-objects-at-the-same-place.html
    //Shader-related stuff; updates the player's position in the "blink range" shader. Decided to put into the same script I was already using instead of a separate script outright.
    //https://docs.unity3d.com/ScriptReference/MaterialPropertyBlock.html
    public void Update () {
        if (player != null)
        {
            materialProperties.SetVector("_PlayerPosition", player.transform.position);
            gameObject.GetComponent<Renderer>().SetPropertyBlock(materialProperties);
        }
	}
    abstract public void onHover();

    abstract public void onClickDown();
    abstract public void onClickHold();
    abstract public void onClickUp();
   
    abstract public void onKeyDown();
    abstract public void onKeyHold();
    abstract public void onKeyUp();
}
