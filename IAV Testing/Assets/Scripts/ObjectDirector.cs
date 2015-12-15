using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectDirector : MonoBehaviour {

    // Link the FFTInt script to get Bin information
    public FFTInt fftInt;
    // Difference in scale was hard to see so a multiplier makes the difference more noticeable
    public float scaleMultiplier = 10;
    // Makes the movement more noticeable
    public float movementMultiplier = 10;
    // Adds more dynamic colour changes
    public float colourMultiplier = 10;
    // Array of gameobjects
    public GameObject[] objectsInScene;
    // Dictionary of gameobject starting positions
    private Dictionary<GameObject, Vector3> objectsStartingPosition = new Dictionary<GameObject, Vector3>();

    // Cube to test methods
    public GameObject testCube;
    // Will be used for beat detections
    float sum;
    float fMax = 200;

    // For testing purposes to it is easy to change methods during play
    public bool moveMethods = false;
    public bool scaleMethods = true;
    public bool colourMethods = false;
    public bool singleObject = true;

    
    // Enum so colour cna be changed in scene
    public enum PColour {red, green, blue }
    public PColour primaryColour;


    public GameObject[] lights;



    // Use this for initialization
    void Start () {

        // Fill the dictionary
        PopulateStartingPosDictionary(objectsStartingPosition, objectsInScene);
    }
	
	// Update is called once per frame
	void Update () {


        if (colourMethods)
        {
            if (singleObject)
            {
                ChangeColour(testCube, 1, primaryColour);
            }
            else
            {

                ObjectArrayColourChange(objectsInScene, 20, primaryColour);
            }

        }
        if (scaleMethods)
        {
           
            if (singleObject)
            {
                // Single object scale change
                 ChangeSize(testCube, 0);
            }
            else
            {
                // Array of objects scale change
                ObjectArraySizeChange(objectsInScene,0);   
            }
        }

        // Beat detection testing
        //BandVol(100, 1000);
        //Debug.Log(sum);
    }

    void LateUpdate ()
    {
        // If statements used for testing only
        if (moveMethods)
        {
            
            if (singleObject)
            {
                // Single object Movement
                MoveObject(testCube, 0);
            }
            else
            {
                // Array of objects movement
                ObjectArrayMovement(objectsInScene, 0);
            }
        }
    }

    // Change the scale of an object using a particular binp
    public void ChangeSize(GameObject objectInScene, int binNum)
    {
        if (objectInScene != null)
        {
            // Set a starting scale, so the object doesnt grow out of proportion
            Vector3 startingScale = new Vector3(1, 1, 1);
            // Set a new vector to store the values from the bins
            Vector3 scaleChange = new Vector3(fftInt.avgBins[binNum] * scaleMultiplier, fftInt.avgBins[binNum] * scaleMultiplier * 10, fftInt.avgBins[binNum] * scaleMultiplier);
            // Set the localScale of the object in the scene using the startingScale and scaleChange
            objectInScene.transform.localScale = new Vector3(startingScale.x + scaleChange.x, startingScale.y + scaleChange.y, startingScale.z + scaleChange.z);
        }
        else
        {
            // If there is no objects
            Debug.LogError("The gameObject you are trying to scale is null");
        }       
    }
    
    // Function to change scale of all objects in an array
    void ObjectArraySizeChange(GameObject[] objects, int lowestBinNum)
    {
        if (lowestBinNum + objects.Length <= fftInt.avgBins.Length)
        {
            // Iterate through list and move objects
            for (int i = 0; i < objects.Length; i++)
            {
                ChangeSize(objects[i], i);
            }
        }
        // Error message so people understand if they are doing something wrong
        else
        {

            Debug.LogError("Not enough bins to support the highest bin number, The highest number the LowestBinNum can be is:" + (fftInt.avgBins.Length - objects.Length));
        }
    }

    // Move an object around depending on the value of the bin it is attached to
    public void MoveObject(GameObject objectInScene, int binNum)
    {
        if (objectInScene != null && objectsStartingPosition.ContainsKey(objectInScene))
        { 
            // Dictionary used to store the initial position
            Vector3 startingPos = objectsStartingPosition[objectInScene];
            // New position from Bins
            Vector3 newPos = new Vector3(fftInt.avgBins[binNum] * movementMultiplier, fftInt.avgBins[binNum] , fftInt.avgBins[binNum] );
            // Change the position
            objectInScene.transform.position = new Vector3(startingPos.x + newPos.x, startingPos.y + newPos.y, startingPos.z + newPos.z);
        }
        else
        {
            // If there is no objects
            Debug.LogError("The gameObject you are trying to move is null");
        }
    }


    // Function for moving multiple objects using bins
    void ObjectArrayMovement(GameObject[] objects, int lowestBinNum)
    {
        if (lowestBinNum + objects.Length <= fftInt.avgBins.Length)
        { 
        // Iterate through list and move objects
        for(int i = 0; i < objects.Length; i++)
        {
            MoveObject(objects[i], i);
        }
        }
        // Error message so people understand if they are doing something wrong
        else
        {

            Debug.LogError("Not enough bins to support the highest bin number, The highest number the LowestBinNum can be is:" + (fftInt.avgBins.Length - objects.Length));
        }
    }


    // Method for filling the dictionary using an array of objects
    void PopulateStartingPosDictionary(Dictionary<GameObject, Vector3> dictionary, GameObject[] arrayOfObjects)
    {       
        // Run a loop and add the objects and their starting positions into the dictionary
        for (int i = 0; i < arrayOfObjects.Length; i++)
        {
            dictionary.Add(arrayOfObjects[i], arrayOfObjects[i].transform.position);
        }
    }

    // Change the main colour of an object
    void ChangeColour(GameObject objectInScene, int binNum, PColour mainColour)
    {
        // Get the objects renderer
        Renderer objectRenderer = objectInScene.GetComponent<Renderer>();       
        // New colour of the object using bins
        Color newColour = new Color(fftInt.avgBins[binNum], fftInt.avgBins[binNum], fftInt.avgBins[binNum], fftInt.avgBins[binNum]);
        // Switch to check what colour the user wants to use
        switch(mainColour)
        {
                
            case PColour.red:
                newColour.r = newColour.r * colourMultiplier;
                break;

            case PColour.green:
                newColour.g = newColour.g * colourMultiplier;
                break;
            case PColour.blue:
                newColour.b = newColour.b * colourMultiplier;
                break;
            default:
                break;
        }

        // Change the colour of the object
        objectRenderer.material.color =   newColour;
    }

    // Function for changing the colour of a whole array at once
    void ObjectArrayColourChange(GameObject[] objects, int lowestBinNum, PColour mainColour)
    {

        if (lowestBinNum + objects.Length <= fftInt.avgBins.Length)
        {
            // loop through the array and run the ChangeColour method
            for (int i = 0; i < objects.Length; i++)
            {

                ChangeColour(objects[i], i + lowestBinNum, mainColour);
            }
        }
        else
        {

            Debug.LogError("Not enough bins to support the highest bin number, The highest number the LowestBinNum can be is:" + (fftInt.avgBins.Length - objects.Length));
        }
    }


    void lightSway(GameObject light, int binNum)
    {

    }


    //Trying to get beat detection

    //public float BandVol(float fLow, float fHigh)
    //{
    //    fLow = Mathf.Clamp(fLow, 20, fMax);
    //    fHigh = Mathf.Clamp(fHigh, fLow, fMax);
    //    int n1 = Mathf.FloorToInt(fLow * fftInt.sampleSize / fMax);
    //    int n2 = Mathf.FloorToInt(fHigh * fftInt.sampleSize / fMax);
    //    sum = 0;
    //    for (int i = n1; i < n2; i++)
    //    {
    //        sum += fftInt.samples[i];
    //    }
    //    return sum / (n2 - n1);
    //}

}
