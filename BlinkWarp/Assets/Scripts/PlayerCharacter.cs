using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPersonController.Characters.ThirdPerson;

[RequireComponent (typeof (ThirdPersonController.Characters.ThirdPerson.ThirdPersonCharacter))]

public class PlayerCharacter : MonoBehaviour {
    public GameObject BlinkStartParticle;
    public GameObject BlinkDashParticle;
    public GameObject MainCamera;
    public InteractableSurface stuckTo;
    public float floorJumpOffset; //how much off the interactive floor the player ends up after jumping

    //https://docs.unity3d.com/Manual/Layers.html
    int interactiveLayerMask = 1 << 8;

    public Vector3 contactPoint = new Vector3(0, 0, 0);
    public float blinkRange = 1f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log(Input.GetKeyDown(KeyCode.Space));

        if (stuckTo != null && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 tempNormal = stuckTo.normal;
            Debug.Log(tempNormal + " is the normal jumped from");
            UnstickPlayerOver(stuckTo);
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(tempNormal.x*6.0f, tempNormal.y * 4.0f+12.0f, tempNormal.z * 6.0f);
        }

        InteractableSurface pointedAt = PointingAt();
        if (pointedAt != null)
        {
            pointedAt.onHover();
            if (Input.GetMouseButtonDown(0))
            {
                pointedAt.onClickDown();
            }
        }



        //fall check
        //for testing
        if (transform.position.y < -50)
        {
            transform.position = new Vector3(0, 1, 0);
        }
	}

    //check to see if we're pointing to an interactable surface
    public InteractableSurface PointingAt()
    {
        //http://answers.unity3d.com/questions/181594/how-to-get-mouse-cursor-position-on-plane-and-set.html
        //https://docs.unity3d.com/ScriptReference/Camera.ScreenPointToRay.html
        //https://docs.unity3d.com/ScriptReference/Input-mousePosition.html

        Ray pointingRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit pointingTo = new RaycastHit();

        if (Physics.Raycast(pointingRay, out pointingTo, interactiveLayerMask))
        {

            //if the thing we're pointing at is an interactable surface, set its contact point to return it
            InteractableSurface temp = pointingTo.transform.gameObject.GetComponent<InteractableSurface>();

            if (temp != null)
            {
                contactPoint = pointingTo.point;
                return temp;
            }
        }

        return null;
    }

    //helper method; are we in blink range?
    public bool InBlinkRange()
    {
        return Vector3.SqrMagnitude(contactPoint - transform.position) <= Mathf.Pow(blinkRange, 2);
    }

    //draws colored lines for us
    public void DrawLineFromPlayerTo(Vector3 endPosition, Color color)
    {
        Debug.DrawLine(transform.position, endPosition, color);
    }

    //draws line to interaction point
    public void DrawLineToInteractionPoint(Color color)
    {
        DrawLineFromPlayerTo(contactPoint, color);
    }

    //draws your blink range "circle"
    public void DrawBlinkRangeCircle(int circlePoints)
    {
        Vector3 prevPoint = new Vector3(transform.position.x + blinkRange, transform.position.y, transform.position.z);
        Vector3 currentPoint = new Vector3(0,0,0);
        for (int k = 1; k <= circlePoints; k++)
        {
            currentPoint = new Vector3(transform.position.x + blinkRange * Mathf.Cos(Mathf.PI * 2 * k / circlePoints), transform.position.y, transform.position.z + blinkRange * Mathf.Sin(Mathf.PI * 2 * k / circlePoints));
            Debug.DrawLine(prevPoint,currentPoint,Color.cyan);
            prevPoint = currentPoint;
        }
    }

    //draws your blink range "sphere"
    public void DrawBlinkRangeSphere(int circlePoints, int latitudesPerHemisphere)
    {
        Vector3 prevPoint = new Vector3(0, 0, 0);
        Vector3 currentPoint = new Vector3(0, 0, 0);
        for (int phi = -latitudesPerHemisphere; phi <= latitudesPerHemisphere; phi++)
        {
            prevPoint = new Vector3(transform.position.x + blinkRange * Mathf.Cos(Mathf.PI / 2 * phi/(latitudesPerHemisphere+1)), transform.position.y + blinkRange*Mathf.Sin(Mathf.PI / 2 * phi / (latitudesPerHemisphere + 1)), transform.position.z);
            
            for (int theta = 1; theta <= circlePoints; theta++)
            {
                currentPoint = new Vector3(transform.position.x + blinkRange * Mathf.Cos(Mathf.PI * 2 * theta / circlePoints) * Mathf.Cos(Mathf.PI / 2 * phi / (latitudesPerHemisphere + 1)), transform.position.y + blinkRange * Mathf.Sin(Mathf.PI / 2 * phi / (latitudesPerHemisphere + 1)), transform.position.z + blinkRange * Mathf.Sin(Mathf.PI * 2 * theta / circlePoints) * Mathf.Cos(Mathf.PI / 2 * phi / (latitudesPerHemisphere + 1)));
                Debug.DrawLine(prevPoint, currentPoint, Color.cyan);
                prevPoint = currentPoint;
            }
        }
    }

    //blinks the player to the point they clicked on
    public void WarpToContact()
    {
        Vector3 travelVector = transform.position - contactPoint;
        Quaternion temp = Quaternion.LookRotation(travelVector);
        temp.eulerAngles = new Vector3(temp.eulerAngles.x+90, temp.eulerAngles.y, temp.eulerAngles.z+90);
        Debug.Log(temp.eulerAngles);

        //spawn associated particle effects
        GameObject.Instantiate(BlinkStartParticle, transform.position, Quaternion.identity);
        GameObject.Instantiate(BlinkStartParticle, contactPoint, Quaternion.identity);
        GameObject dashLine = GameObject.Instantiate(BlinkDashParticle, contactPoint + travelVector / 2, temp); 
        //GameObject dashLine = GameObject.Instantiate(BlinkDashParticle, contactPoint+ travelVector/4, temp); //debug
        dashLine.transform.localScale = new Vector3(travelVector.magnitude/2, 1, 1);
        transform.position = contactPoint;
    }

    public void StickPlayer()
    {
        gameObject.GetComponent<Rigidbody>().velocity=new Vector3(0,0,0);
        gameObject.GetComponent<Rigidbody>().isKinematic=true;
    }

    public void StickPlayerTo(InteractableSurface surface)
    {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        stuckTo = surface;
    }

    public void UnstickPlayer()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    public void UnstickPlayerOver(InteractableSurface surface)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.position += surface.normal * floorJumpOffset;
        stuckTo = null;
    }
}
