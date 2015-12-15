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
    // Lists to store the objects that will be changing
    public List<GameObject> scaleObjects = new List<GameObject>();
    public List<GameObject> moveObjects = new List<GameObject>();
    public List<GameObject> colourObjects = new List<GameObject>();
    public List<GameObject> lights = new List<GameObject>();
    // Dictionary of gameobject starting positions
    private Dictionary<GameObject, Vector3> objectsStartingPosition = new Dictionary<GameObject, Vector3>();
    // Dictionary to store the lights
    private Dictionary<GameObject, Light> LightGameObjects = new Dictionary<GameObject, Light>();
    // Cube to test methods
    public GameObject testCube;
    public GameObject testLight;
    // Will be used for beat detections
    float sum;
    float fMax = 200;
    // For testing purposes to it is easy to change methods during play
    public bool moveMethods = false;
    public bool scaleMethods = true;
    public bool colourMethods = false;
    public bool singleObject = true;
    public bool songPlaying = false;
    // Enum so colour cna be changed in scene
    public enum PColour {red, green, blue }
    public PColour primaryColour;
    // Use this for initialization
    void Start () {
        // Populate the scale and movement lists
        PopulateGOListWithTag(scaleObjects, "Scale");
        PopulateGOListWithTag(moveObjects, "Move");
        // Fill the dictionary to store initial positions of objects
        PopulateStartingPosDictionary(objectsStartingPosition, moveObjects);
        //PopulateLightDictionary(LightGameObjects, lights);
    }
	
	// Update is called once per frame
	void Update () {
        // Check if there is a song playing
        if(fftInt.samples.Length > 0)
        {
            songPlaying = true;
        }
        else { songPlaying = false; }

        if (songPlaying)
        {
            //LightColour(testLight, 1);
            //LightArrayColourChange(lights, 0, PColour.blue);
            // Methods used to change the colour of objects
            if (colourMethods)
            {
                if (singleObject)
                {
                    ChangeColour(testCube, 0, primaryColour);
                }
                else
                {
                    MultipleObjectColourChange(scaleObjects, 0, primaryColour);
                    MultipleObjectColourChange(moveObjects, 0, primaryColour);
                }
            }
            if (scaleMethods)
            {
                // Methods used to scale objects
                if (singleObject)
                {
                    // Single object scale change
                    ChangeSize(testCube, 0);
                }
                else
                {
                    // Array of objects scale change
                    MultipleObjectScaleChange(scaleObjects, 0);
                }
            }
        }
        // Beat detection testing
        //BandVol(100, 1000);
        //Debug.Log(sum);
    }

    void LateUpdate ()
    {
        if(songPlaying)
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
                    MultipleObjectMovement(moveObjects, 0);
            }
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
            Vector3 scaleChange = new Vector3(fftInt.avgBins[binNum] * scaleMultiplier, fftInt.avgBins[binNum] * scaleMultiplier, fftInt.avgBins[binNum] * scaleMultiplier);
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
    void MultipleObjectScaleChange(List<GameObject> objects, int lowestBinNum)
    {
        if (lowestBinNum + objects.Count <= fftInt.avgBins.Length)
        {
            // Iterate through list and move objects
            for (int i = 0; i < objects.Count; i++)
            {
                ChangeSize(objects[i], i);
            }
        }
        // Error message so people understand if they are doing something wrong
        else
        {
            Debug.LogError("Not enough bins to support the highest bin number, The highest number the LowestBinNum can be is:" + (fftInt.avgBins.Length - objects.Count));
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
    void MultipleObjectMovement(List<GameObject> objects, int lowestBinNum)
    {
        if (lowestBinNum + objects.Count <= fftInt.avgBins.Length)
        { 
        // Iterate through list and move objects
        for(int i = 0; i < objects.Count; i++)
        {
            MoveObject(objects[i], i);
        }
        }
        // Error message so people understand if they are doing something wrong
        else
        {
            Debug.LogError("Not enough bins to support the highest bin number, The highest number the LowestBinNum can be is:" + (fftInt.avgBins.Length - objects.Count));
        }
    }

    // Method for filling the dictionary using an array of objects
    void PopulateStartingPosDictionary(Dictionary<GameObject, Vector3> dictionary, List<GameObject> listOfObjects)
    {
        if (listOfObjects.Count > 0)
        {
            // Run a loop and add the objects and their starting positions into the dictionary
            for (int i = 0; i < listOfObjects.Count; i++)
            {
                dictionary.Add(listOfObjects[i], listOfObjects[i].transform.position);
            }
        }
        else
        {
            Debug.LogError("ErrorWithStartingDictionary");
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
    void MultipleObjectColourChange(List<GameObject> objects, int lowestBinNum, PColour mainColour)
    {      
        // Set oarticular bins to colours so there are multiple colours in the scene at once
        if (lowestBinNum + objects.Count <= fftInt.avgBins.Length)
        {
            // loop through the array and run the ChasngeColour method
            for (int i = 0; i < objects.Count; i++)
            {
                if (i < 15)
                {
                    ChangeColour(objects[i], i + lowestBinNum, PColour.blue);
                }
                else if (i < 30)
                {
                    ChangeColour(objects[i], i + lowestBinNum, PColour.red);
                }
                else if (i > 30)
                {
                    ChangeColour(objects[i], i + lowestBinNum, PColour.green);
                }
                
            }   
        }
        else
        {  
            Debug.LogError("Not enough bins to support the highest bin number, The highest number the LowestBinNum can be is:" + (fftInt.avgBins.Length - objects.Count));
        }
    }

    // Populating the light dictionary
    void PopulateLightDictionary(Dictionary<GameObject, Light> dictionary, List<GameObject> listOfObjects)
    {
        // Loop through a list of objects, get the object, and the light attached to the object
        for (int i = 0; i < listOfObjects.Count; i++)
        {

            dictionary.Add(listOfObjects[i], listOfObjects[i].GetComponent<Light>());
        }
    }

    // Method to change the colour of a light
    void LightColour(GameObject light, int binNum, PColour colour)
    {
        // Check there is a light
        if (light != null)
        {   // Set a light vairables to the components
            Light objectLight = light.GetComponent<Light>();
            // Set a new colour
            Color newColour = new Color();
            // Use switch to change the colour using the Pcolour enum
            switch (colour)
            {
                case PColour.red:
                    newColour = Color.red;
                    break;
                case PColour.green:
                    newColour = Color.green;
                    break;
                case PColour.blue:
                    newColour = Color.blue;
                    break;
                default:
                    break;
            }
            // Change the colour
            objectLight.color = newColour;
        }
    }

    // Changing lights in an array
    void MultipleLightObjectChange(List<GameObject> lights, int lowestBinNum, PColour colour)
    {
        if (lowestBinNum + lights.Count <= fftInt.avgBins.Length)
        {
            // loop through the array and run the ChangeColour method
            for (int i = 0; i < lights.Count; i++)
            {
                
                LightColour(lights[i], i + lowestBinNum, colour ); 
            }
        }
        else
        {
            Debug.LogError("Not enough bins to support the highest bin number, The highest number the LowestBinNum can be is:" + (fftInt.avgBins.Length - lights.Count));
        }

    }

    // Method for populating gameObject lists using tags
    void PopulateGOListWithTag(List<GameObject> list, string tag)
    {
        // Check the tag, and change the loop accordingly
        // Needs to be made more efficient
        if (tag == "Scale")
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Scale"))
            {
                list.Add(item);
            }
        }
        else if (tag == "Move")
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Move"))
            {
                list.Add(item);
            }
        }
        else if (tag == "Colour")
        {
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Colour"))
            {
                list.Add(item);
            }
        }
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
