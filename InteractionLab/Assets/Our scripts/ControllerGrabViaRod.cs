using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabViaRod : MonoBehaviour
{
    public enum AxisType
    {
        XAxis,
        ZAxis
    }
    public float thickness = 0.006f;
    public AxisType facingAxis = AxisType.XAxis;
    public float length = 0.15f;
    public Color color = Color.yellow;


    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    private GameObject objectInHand;
    GameObject holder;
    static GameObject pointer;
    static Material newMaterial;
    private bool StartIsReady = false;
    private bool canChangeColor = true;
    private Color saveColor;
    private GameObject resetColor;



    protected Rigidbody controllerAttachPoint;
    protected FixedJoint givenJoint;


    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void OldStart()
    {
        newMaterial = new Material(Shader.Find("Unlit/TransparentColor"));
        color = new Color(0, 0, 0, 255);
        newMaterial.SetColor("_Color", color);

        holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;
        //holder.transform.localPosition = new Vector3(0, -0.02f, 0);

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = holder.transform;
        pointer.GetComponent<MeshRenderer>().material = newMaterial;

        pointer.GetComponent<BoxCollider>().isTrigger = true;
        pointer.AddComponent<Rigidbody>().isKinematic = false;
        pointer.layer = 2;

        SetPointerTransform(length, thickness);
        StartIsReady = true;

    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())// || !col.Equals(cursor.GetComponent<SphereCollider>()))
        {
            return;
        }
        collidingObject = col.gameObject;
        Debug.Log("Colliding Object " + collidingObject.name);
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

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
     
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
    void Update()
    {

        if (!StartIsReady)
        {
            OldStart();
            //this.GetComponent<BoxCollider>().size = new Vector3(0.005f, 0.005f, 0.3f);
           // this.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0.02f);

            this.GetComponent<BoxCollider>().size = new Vector3(0.006f, 0.006f, 0.05f);
            this.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0.125f);

        }

        if (collidingObject && collidingObject.transform.tag == ("Moveable") && canChangeColor)
        {
            Renderer rend = collidingObject.GetComponent<Renderer>();
            saveColor = rend.material.color;
            rend.material.color = Color.green;
            canChangeColor = false;
            resetColor = collidingObject;
        }
        else if (!canChangeColor && !collidingObject)
        {
            Renderer rend = resetColor.GetComponent<Renderer>();
            rend.material.color = saveColor;
            canChangeColor = true;
        }

        SetPointerTransform(length, thickness);
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
    void SetPointerTransform(float setLength, float setThicknes)
    {
        //if the additional decimal isn't added then the beam position glitches
        float beamPosition = setLength / (2 + 0.00001f);

        if (facingAxis == AxisType.XAxis)
        {
            pointer.transform.localScale = new Vector3(setLength, setThicknes, setThicknes);
            pointer.transform.localPosition = new Vector3(beamPosition, 0f, 0f);
        }
        else
        {
            pointer.transform.localScale = new Vector3(setThicknes, setThicknes, setLength);
            pointer.transform.localPosition = new Vector3(0f, 0f, beamPosition);
        }
    }
}
