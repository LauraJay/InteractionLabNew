using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RaycastingMethode : MonoBehaviour
{
    public enum AxisType
    {
        XAxis,
        ZAxis
    }
    private SteamVR_TrackedObject trackedObj;
    public Color color;
    public float thickness = 0.004f;
    public AxisType facingAxis = AxisType.XAxis;
    public float length = 100f;
    public bool showCursor = true;
    public RaycastHit hitObject;
    private GameObject pressedController;
    private GameObject temp;
    private bool triggerState;
    public static bool StartIsReady = false;
    private TargetTest t;


    GameObject holder;
    static GameObject pointer;
    static GameObject cursor;
    static Material newMaterial;

    Vector3 cursorScale = new Vector3(0.05f, 0.05f, 0.05f);
    float contactDistance = 0f;
    private static int counter = 0;
    static public Ray raycast;
    Transform contactTarget = null;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
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


    // Use this for initialization
    void OldStart()
    {
        triggerState = false;
        pressedController = new GameObject();
        pressedController.name = "pressedController";

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
            cursor.layer = 2;
        }

        SetPointerTransform(length, thickness);
        StartIsReady = true;
    }

    float GetBeamLength(bool bHit, RaycastHit hit)
    {
        float actualLength = length;

        //reset if beam not hitting or hitting new target
        if (!bHit || (contactTarget && contactTarget != hit.transform))
        {
            contactDistance = 0f;
            contactTarget = null;
        }

        //check if beam has hit a new target
        if (bHit)
        {
            if (hit.distance <= 0)
            {

            }
            contactDistance = hit.distance;
            contactTarget = hit.transform;
        }

        //adjust beam length if something is blocking it
        if (bHit && contactDistance < length)
        {
            actualLength = contactDistance;
        }

        if (actualLength <= 0)
        {
            actualLength = length;
        }

        return actualLength; ;
    }

    void Update()
    {
        if (counter == 0)
        {
            OldStart();
            counter++;
        }
        raycast = new Ray(transform.position, transform.forward);

        bool rayHit = Physics.Raycast(raycast, out hitObject);
        // show pointed at Objects
        // print object that was hit
        if (rayHit && hitObject.transform.tag == "Moveable")
        {
            MeshRenderer cursorRenderer = cursor.GetComponent<MeshRenderer>();
            Color grabbingColor = new Color(0, 255, 0, 1);
            cursorRenderer.material.color = grabbingColor;
            //Debug.Log(hitObject.transform.gameObject.name);
            //Debug.Log("Touched moveable Object" + hitObject.transform.gameObject.name);
        }
        else
        {
            MeshRenderer cursorRenderer = cursor.GetComponent<MeshRenderer>();
            Color grabbingColor = new Color(0, 0, 0, 1);
            cursorRenderer.material.color = grabbingColor;
        }

        float beamLength = GetBeamLength(rayHit, hitObject);
        SetPointerTransform(beamLength, thickness);

        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger) && !triggerState && rayHit && hitObject.transform.tag == "Moveable")
        {
            GrabObject();

        }


        else if (!Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger) && triggerState && hitObject.transform != null)
        {
            ReleaseObject();
            // Debug.Log("should release object");
        }


        triggerState = Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger);
    }


    void GrabObject()
    {
        temp = hitObject.transform.gameObject;
        temp.transform.SetParent(cursor.transform);
        temp.transform.GetComponent<Rigidbody>().isKinematic = true;
        temp.transform.GetComponent<Rigidbody>().useGravity = true;

        Debug.Log("Grabbed Object " + temp.transform.name);
        string name = temp.name;
        if (name.Equals("TargetObject"))
        {
            t = temp.GetComponent<TargetTest>();

            if (t != null)
            {
                t.getMeasurements().StopGrabTimeMeasure();
                Debug.Log("stop grab time / start pos time");
                t.getMeasurements().incrementErrorRate();
                Debug.Log("increment Error Rate");
            }

        }
        else {
            t.getMeasurements().incrementWrongSelection();
        }

    }

    void ReleaseObject()
    {
        if (temp != null)
        {
            Debug.Log("Name of Release Object: " + temp.name);
            temp.transform.SetParent(null);
            Vector3 pos = temp.transform.position;
            Quaternion rot = temp.transform.rotation;
            pressedController = new GameObject();
            temp.transform.rotation = rot;
            temp.transform.position = pos;
            temp.transform.GetComponent<Rigidbody>().isKinematic = false;
            temp.transform.GetComponent<Rigidbody>().useGravity = true;
            Debug.Log("Is Kinematik: " + temp.transform.GetComponent<Rigidbody>().isKinematic + ", use gravity: " + temp.transform.GetComponent<Rigidbody>().useGravity);
        }
    }

    static public void deleteRay()
    {
        newMaterial.color = new Color(0, 0, 0, 0);
        cursor.GetComponent<MeshRenderer>().material = newMaterial;
        pointer.GetComponent<MeshRenderer>().material = newMaterial;
        StartIsReady = false;
    }

    static public void ActivateRay()
    {
        newMaterial.color = new Color(0, 0, 0, 255);
        cursor.GetComponent<MeshRenderer>().material = newMaterial;
        pointer.GetComponent<MeshRenderer>().material = newMaterial;
    }

}
