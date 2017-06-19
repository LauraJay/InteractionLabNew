using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndirectRaycast : MonoBehaviour
{
    public enum AxisType
    {
        XAxis,
        ZAxis
    }
    public float thickness = 0.004f;
    public AxisType facingAxis = AxisType.XAxis;
    public float length = 100f;


    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    private GameObject objectInHand;
    GameObject holder;
    static GameObject pointer;
    static GameObject cursor;
    static Material newMaterial;
    public Color color;
    Vector3 cursorScale = new Vector3(0.05f, 0.05f, 0.05f);
    private bool showCursor = true;
    private bool StartIsReady = false;
    private float step;

    // testing for snapping
    public bool snapObject = false;
    protected Rigidbody controllerAttachPoint;
    protected FixedJoint givenJoint;


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

    private void OldStart() {
        step = 0.01f;
        newMaterial = new Material(Shader.Find("Unlit/TransparentColor"));
        color = new Color(0, 0, 0, 255);
        newMaterial.SetColor("_Color", color);

        holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = holder.transform;
        pointer.GetComponent<MeshRenderer>().material = newMaterial;

        pointer.GetComponent<BoxCollider>().isTrigger = true;
        pointer.AddComponent<Rigidbody>().isKinematic = true;
        pointer.layer = 2;

        if (showCursor)
        {
            cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cursor.transform.parent = holder.transform;
            cursor.GetComponent<MeshRenderer>().material = newMaterial;
            cursor.transform.localScale = cursorScale;
            cursor.GetComponent<SphereCollider>().isTrigger = true;
            cursor.AddComponent<Rigidbody>().isKinematic = true;
            cursor.GetComponent<Rigidbody>().useGravity = false;

            cursor.layer = 2;
        }

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
        Debug.Log("Colliding Object" + collidingObject.name);
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

        if (!snapObject)
        {
            Debug.Log("no snapping " + snapObject);
            var joint = AddFixedJoint();
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        }

        else if (snapObject)
        {
            Debug.Log("snapping " + snapObject);

            var joint = createJoint(GameObject.Find("Controller (right)"));
            SetSnappedObjectPosition(collidingObject);
            joint.connectedBody = collidingObject.GetComponent<Rigidbody>();
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
        }
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad) || Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        // if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
            print("Pressing Touchpad");

            if (touchpad.y > 0.3f)
            {
                if (length < 100 * step)
                {
                    print("Moving Up");
                }
                length += step;
            }

            else if (touchpad.y < -0.3f)
            {
                if (length > step) { 
                length -= step;
                }
                print("Moving Down");
            }

  

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
            if (showCursor)
            {
                cursor.transform.localPosition = new Vector3(setLength - cursor.transform.localScale.x, 0f, 0f);
            }
        }
        else
        {
            pointer.transform.localScale = new Vector3(setThicknes, setThicknes, setLength);
            pointer.transform.localPosition = new Vector3(0f, 0f, beamPosition);

            if (showCursor)
            {
                cursor.transform.localPosition = new Vector3(0f, 0f, setLength - cursor.transform.localScale.z);
            }
        }
    }
}
