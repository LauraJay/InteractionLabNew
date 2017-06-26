using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RaycastingMethodeHMD : MonoBehaviour
{
    public enum AxisType
    {
        XAxis,
        ZAxis
    }
   // private SteamVR_TrackedObject trackedObj;
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
    private GameObject HMDEye;

    GameObject holder;
    static GameObject pointer;
    static GameObject cursor;
    static Material newMaterial;

    Vector3 cursorScale = new Vector3(0.05f, 0.05f, 0.05f);
    float contactDistance = 0f;
    private static int counter = 0;
    static public Ray raycast;
    Transform contactTarget = null;
    int ControllerID =3;
    

    //private SteamVR_Controller.Device Controller
    //{
    //    get { return SteamVR_Controller.Input((int)trackedObj.index); }
    //}
   


    //void Awake()
    //{
    //    trackedObj = GetComponent<SteamVR_TrackedObject>();
    //}

    void SetPointerTransform(float setLength, float setThicknes)
    {
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
        HMDEye = GameObject.Find("Camera (eye)");
       var CameraObject = SteamVR_Render.Top();
        triggerState = false;
        pressedController = new GameObject();
        pressedController.name = "pressedController";

        newMaterial = new Material(Shader.Find("Unlit/TransparentColor"));
        color = new Color(0, 0, 0, 255);
        newMaterial.SetColor("_Color", color);

        holder = new GameObject();
        holder.transform.parent = HMDEye.transform;
        holder.transform.localPosition = Vector3.zero;
        //holder.transform.position = HMDEye.transform.position;
        //holder.transform.rotation = HMDEye.transform.rotation;

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = HMDEye.transform.position;
        //cube.transform.rotation = HMDEye.transform.rotation;
        cube.transform.localScale = new Vector3(.1f, .1f, .1f);


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
        var CameraObject = SteamVR_Render.Top();
        Debug.Log("Camera local Position x=" + CameraObject.transform.localPosition.x + "  and y=" + CameraObject.transform.localPosition.y + " and z=" + CameraObject.transform.localPosition.z);
        //Debug.Log("Camera Rotation   x = " + CameraObject.transform.rotation.x + "  and y = " + CameraObject.transform.rotation.y + " and z = " + CameraObject.transform.rotation.z);
        //         Debug.Log("Camera Rotoation x=" + (CameraObject.transform.rotation.x - HMDEye.transform.rotation.x) + "  and y=" +( CameraObject.transform.rotation.y - HMDEye.transform.rotation.y) + " and z=" + (CameraObject.transform.rotation.z - HMDEye.transform.rotation.z));
        Debug.Log("hmd local Position x=" + HMDEye.transform.localPosition.x + "  and y=" + HMDEye.transform.localPosition.y + " and z=" + HMDEye.transform.localPosition.z);

        Debug.Log("diff Position x=" + (CameraObject.transform.localPosition.x - HMDEye.transform.localPosition.x) + "  and y=" + (CameraObject.transform.localPosition.y - HMDEye.transform.localPosition.y) + " and z=" + (CameraObject.transform.localPosition.z - HMDEye.transform.localPosition.z));
        //holder.transform.position = HMDEye.transform.position;
        //holder.transform.rotation = HMDEye.transform.rotation;

        raycast = new Ray(transform.position, transform.forward);
        //raycast = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.forward);
        //raycast = new Ray(CameraObject.transform.position, CameraObject.transform.forward * length);
        //raycast = new Ray(new Vector3(CameraObject.transform.position.x, CameraObject.transform.position.y+0.015f, CameraObject.transform.position.z), CameraObject.transform.forward * length);
        bool rayHit = Physics.Raycast(raycast, out hitObject);
        

        if (rayHit && hitObject.transform.tag == "Moveable")
        {
            MeshRenderer cursorRenderer = cursor.GetComponent<MeshRenderer>();
            Color grabbingColor = new Color(0, 255, 0, 1);
            cursorRenderer.material.color = grabbingColor;
            Debug.Log(hitObject.transform.gameObject.name);
            Debug.Log("Touched moveable Object" + hitObject.transform.gameObject.name);
        }
        else
        {
            MeshRenderer cursorRenderer = cursor.GetComponent<MeshRenderer>();
            Color grabbingColor = new Color(0, 0, 0, 1);
            cursorRenderer.material.color = grabbingColor;
        }

        float beamLength = GetBeamLength(rayHit, hitObject);
        SetPointerTransform(beamLength, thickness);

            if (SteamVR_Controller.Input(ControllerID).GetPress(SteamVR_Controller.ButtonMask.Trigger) && !triggerState && rayHit && hitObject.transform.tag == "Moveable")
             {
                 GrabObject();
             }


             else if (!SteamVR_Controller.Input(ControllerID).GetPress(SteamVR_Controller.ButtonMask.Trigger) && triggerState && hitObject.transform != null)
             {
                 ReleaseObject();
             }


             triggerState = SteamVR_Controller.Input(ControllerID).GetPress(SteamVR_Controller.ButtonMask.Trigger);   
    }


    void GrabObject()
    {
        temp = hitObject.transform.gameObject;
        temp.transform.SetParent(cursor.transform);
        temp.transform.GetComponent<Rigidbody>().isKinematic = true;
    }

    void ReleaseObject()
    {
        temp.transform.SetParent(null);
        Vector3 pos = temp.transform.position;
        Quaternion rot = temp.transform.rotation;
        pressedController = new GameObject();
        temp.transform.rotation = rot;
        temp.transform.position = pos;
        temp.transform.GetComponent<Rigidbody>().isKinematic = false;
        temp.transform.GetComponent<Rigidbody>().useGravity = true;
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
