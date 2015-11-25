using UnityEngine;
using System.Collections;

public class RotateLines : MonoBehaviour {

    public LineController3D lineController;
    public float speedScalar;
    public float meanOfMeans;
    public bool isCamera;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        for (int index = 0; index < lineController.numFreqRanges; index++)
        {
            meanOfMeans += lineController.meanAmplitudeInRange[index];
        }
        meanOfMeans /= lineController.numFreqRanges;

        if (isCamera)
        {
            transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * meanOfMeans * speedScalar);
           // transform.Rotate(Vector3.right, /*Input.GetAxis("Vertical") */ meanOfMeans * speedScalar);
        }
        else
        {
            transform.Rotate(transform.up, meanOfMeans * speedScalar);
        }
	}
}
