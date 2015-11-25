using UnityEngine;
using System.Collections;

public class ModifyLineFFT : MonoBehaviour 
{
    public LineController3D lineController;
    public float heightScalar;

	// Update is called once per frame
	void Update () 
    {   
        for (int index = 0; index < lineController.numFreqRanges; index++)
        {
            GetComponent<LineRenderer>().SetPosition(index, new Vector3((-42.2f + 9.6f * index), -25 + lineController.meanAmplitudeInRange[index] * heightScalar, 0));
        }
	}
}
