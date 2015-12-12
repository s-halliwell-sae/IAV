using UnityEngine;
using System.Collections;

public class ObjectDirector : MonoBehaviour {

    // Link the FFTInt script to get Bin information
    public FFTInt fftInt;
    // Difference in scale was hard to see so a multiplier makes the difference more noticeable
    public float scaleMultiplier = 10;
    // Array of gameobjects
    public GameObject[] objectsInScene;

    public float fMax = 200;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        ObjectArraySizeChange(objectsInScene);

       
	}

    // Change the scale of an object using a particular bin
    public void ChangeSize(GameObject objectInScene, int i)
    {
        // Set a starting scale, so the object doesnt grow out of proportion
        Vector3 startingScale =  new Vector3(1, 1, 1);
       // Set a new vector to store the values from the bins
        Vector3 scaleChange = new Vector3(fftInt.avgBins[i] * scaleMultiplier, fftInt.avgBins[i] * scaleMultiplier * 10, fftInt.avgBins[i] * scaleMultiplier);
        // Set the localScale of the object in the scene using the startingScale and scaleChange
        objectInScene.transform.localScale = new Vector3 (startingScale.x + scaleChange.x, startingScale.y + scaleChange.y, startingScale.z + scaleChange.z);
        
    }

    // Function to change scale of all objects in an array
    void ObjectArraySizeChange(GameObject[] objects)
    {
        // Loop over array and set a gameobject and bin number
        for(int i = 0; i < objects.Length; i++)
        {
            // Array Error Here
            // Does not cause scene to crash
            ChangeSize(objects[i].gameObject, i);
        }
    }





    // Trying to get beat detection 
    
    //public float BandVol(float fLow, float fHigh)
    //{
    //    fLow = Mathf.Clamp(fLow, 20, fMax);
    //    fHigh = Mathf.Clamp(fHigh, fLow, fMax);
    //    int n1 = Mathf.FloorToInt(fLow * fftInt.sampleSize / fMax);
    //    int n2 = Mathf.FloorToInt(fHigh * fftInt.sampleSize / fMax);
    //    float sum = 0;
    //    for(int i = n1; i < n2; i++)
    //    {
    //        sum += fftInt.samples[i];        
    //    }
    //    return sum / (n2 - n1);
    //}

}
