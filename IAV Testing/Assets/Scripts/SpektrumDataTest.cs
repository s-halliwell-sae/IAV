using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpektrumDataTest : MonoBehaviour {

    // AudioSource
    public AudioSource testAudio;

  

    //Array to store all of the spectrum data
    public float[] spectrum;

	// Use this for initialization
	void Start () {

        // Get the audiosource component
        testAudio = GetComponent<AudioSource>();


	}
	
	// Update is called once per frame
	void Update () {

        // Set spectrum to the getspectrumData
        // getSpectrumdata(float[] Samples, int channel, FFTwindow window);
        spectrum = testAudio.GetSpectrumData(8192, 0, FFTWindow.BlackmanHarris);
        
        // Create a new int for the loop
        int i = 1;

        // Use a while loop to go through all of the spectrum data of the audiosource
        while (i < 8191)
        {

            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[ i - 1 ]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i ]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
            i++;


        }

   

	}


}



// Compact numbers down to Log Bins
// dont want 800 samples

