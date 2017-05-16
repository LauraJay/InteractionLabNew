using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTest : MonoBehaviour
{

    private GameObject targetArea;
    private GameObject collidingObject;
    private Vector3 startPos;
    private Texture objectTex;
    private Vector3 objectCol;
    private Vector3 objectOutline;

    public Material material;

    // Use this for initialization
    void Start()
    {
        targetArea = GameObject.Find("TargetArea");
        //targetArea.GetComponent("Mesh Collider");
        collidingObject = GameObject.Find("smallCube");
        //collidingObject = GameObject.Find("Apple/Sphere");
        startPos = collidingObject.transform.position;

        objectTex = collidingObject.GetComponent<MeshRenderer>().material.GetTexture("_MainTex");
        objectCol = collidingObject.GetComponent<MeshRenderer>().material.GetVector("_Color");
        material.mainTexture = objectTex;
        material.SetColor("_Color", new Color(objectCol.x, objectCol.y, objectCol.z));
        material.SetColor("_OutlineColor", new Color(1, 1, 1));
        material.SetFloat("_Outline", 0.01f);
        collidingObject.GetComponent<MeshRenderer>().material = material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            collidingObject.transform.localPosition = new Vector3(0.192f, 1.04f, 0.246f);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            collidingObject.transform.localPosition = startPos;
        }


    }


    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("Enter");
        //change outline
        material.SetColor("_OutlineColor", new Color(1, 0, 0));
    }

   // public void OnTriggerStay(Collider other)
   // {
    //    Debug.Log("Stay");
   //     collidingObject.GetComponent<MeshRenderer>().materials.SetValue(testMaterial, 0);
   // }


    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        material.SetColor("_OutlineColor", new Color(1, 1, 1));
    }

}
