using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(new Vector3(0f,0f,0f), Vector3.right + Vector3.up, Time.deltaTime * 10f);
    }
}
