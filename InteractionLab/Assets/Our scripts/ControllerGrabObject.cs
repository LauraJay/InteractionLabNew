using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    private GameObject objectInHand;



    // testing for snapping
    public bool snapObject = true;
    public Transform rightHandleSnap; 
    protected Rigidbody controllerAttachPoint;
   // protected Joint ControllerAttachPoint;



    private void setObjectSnapping( bool snap ) {

        snapObject = snap;
    }

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }


    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
      //  collidingObject = null;
        //if (!snapObject) { 
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    //}
        //else if(snapObject) {
        //    SetSnappedObjectPosition(collidingObject);
        //}
        //snapping 
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
       
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        objectInHand = null;
    }

    // Update is called once per frame
    void Update () {
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject && collidingObject.transform.tag == ("Moveable") )
            {
                Debug.Log("Grabbed Object: " + collidingObject.transform.name);
                GrabObject(); 
            }
        }
        if (Controller.GetHairTriggerUp())
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
    }

    protected virtual void SetSnappedObjectPosition(GameObject obj)
    {
        Debug.Log("Current Object " + obj.transform.name);
        controllerAttachPoint.position = Controller.transform.pos  + new Vector3(100,0,0);
        controllerAttachPoint.rotation = Controller.transform.rot;


        if (obj.transform == null)
        {
            obj.transform.position = controllerAttachPoint.transform.position;
        }
        else
        {
            obj.transform.rotation = controllerAttachPoint.transform.rotation * Quaternion.Euler(obj.transform.localEulerAngles);
            obj.transform.position = controllerAttachPoint.transform.position - (obj.transform.position - obj.transform.position);
        }
    }
}
