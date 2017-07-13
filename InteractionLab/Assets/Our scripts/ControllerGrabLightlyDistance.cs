using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabLightlyDistance : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    private GameObject objectInHand;
    private Color saveColor;
    private GameObject resetColor;

    // testing for snapping
    public bool snapObject = false;
    private bool LargeColider = false;
    private bool canChangeColor = true;
    protected Rigidbody controllerAttachPoint;
    protected FixedJoint givenJoint;

    private TargetTest t;
    public bool isLearningMode = false;
    private SelfTeaching selfTeaching; 

    public void setObjectSnapping(bool snap)
    {
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
        selfTeaching = GameObject.Find("RightController").GetComponent<SelfTeaching>();
        //if (Menu.teaching) selfTeaching.increaseCounter();
        

        objectInHand = collidingObject;

        if (!snapObject)
        {
            if (Menu.teaching) selfTeaching.setCounter(16);
            Debug.Log("no snapping " + snapObject);
            var joint = AddFixedJoint();
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        }

        else if (snapObject)
        {
            if (Menu.teaching) selfTeaching.setCounter(21);
            Debug.Log("snapping " + snapObject);

            var joint = createJoint(GameObject.Find("Controller (right)"));
            SetSnappedObjectPosition(collidingObject);
            joint.connectedBody = collidingObject.GetComponent<Rigidbody>();
        }
        string name = objectInHand.name;

        if (!isLearningMode)
        {
            t = objectInHand.GetComponent<TargetTest>();
            if (t != null)
            {
                if (name.Equals("TargetObject"))
                {


                    t.getMeasurements().StopGrabTimeMeasure();
                    Debug.Log("stop grab time / start pos time");
                    t.getMeasurements().incrementErrorRate();
                    Debug.Log("increment Error Rate");
                }

                else
                {
                    t.getMeasurements().incrementWrongSelection();
                }
            }
        }

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
        if (Menu.teaching) selfTeaching.increaseCounter();
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
    void Update()
    {
        //Collider vergrößern
        if (!LargeColider) {
            //string size = this.GetComponent<BoxCollider>().size.ToString();
            //Debug.Log("Size Collider befor resizing: " + size);
            this.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.01f, 0.125f);
            this.GetComponent<BoxCollider>().transform.position = new Vector3(0f, -0.06f, 0.02f);
            LargeColider = true;
        }

        if(collidingObject && collidingObject.transform.tag == ("Moveable") && canChangeColor)
        {
            Renderer rend = collidingObject.GetComponent<Renderer>();
            saveColor = rend.material.color;
            rend.material.color = Color.green;
            canChangeColor = false;
            resetColor = collidingObject;
        }
        else if (!canChangeColor && collidingObject==null)
        {
            Renderer rend = resetColor.GetComponent<Renderer>();
            rend.material.color = saveColor;
            canChangeColor = true;
        }




        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject && collidingObject.transform.tag == ("Moveable"))
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

    protected virtual FixedJoint createJoint(GameObject obj)
    {
        Debug.Log("create joint" + obj.transform.name);
        givenJoint = obj.AddComponent<FixedJoint>();
        givenJoint.breakForce = Mathf.Infinity;
        return givenJoint;
    }


    protected virtual void SetSnappedObjectPosition(GameObject obj)
    {
        Debug.Log("Current Object " + obj.transform.name);
        controllerAttachPoint = GameObject.Find("Controller (right)").GetComponent<Rigidbody>();

        obj.transform.rotation = controllerAttachPoint.transform.rotation; //* Quaternion.Euler(obj.transform.localEulerAngles);
        obj.transform.position = controllerAttachPoint.transform.position - (obj.transform.position - obj.transform.position);

    }
}