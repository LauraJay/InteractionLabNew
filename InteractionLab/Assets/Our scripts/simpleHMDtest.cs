using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleHMDtest : MonoBehaviour {

    bool StartReady = false;
    private GameObject HMDEye;
    private GameObject cube;

    // Use this for initialization
    void TestStart () {
        HMDEye = GameObject.Find("Camera (ears)");

        cube =  GameObject.CreatePrimitive(PrimitiveType.Cube);


      //   cube.transform.localPosition = Vector3.zero;

        cube.transform.position = HMDEye.transform.position + new Vector3 ( 0.0f, 0.0f, 5.0f);
     //   cube.transform.rotation = HMDEye.transform.rotation;
        cube.transform.SetParent(HMDEye.transform);
    
        StartReady = true;
    }
	
	// Update is called once per frame
	void Update () {
        while (!StartReady)
        {
            TestStart();
        }
	}
}
