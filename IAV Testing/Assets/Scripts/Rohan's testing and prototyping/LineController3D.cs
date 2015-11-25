using UnityEngine;
using System.Collections;

public class LineController3D : MonoBehaviour {

    public fftfun fftController;
    private int dataCount;
    public float currentValue;
    public int numFreqRanges = 1;
    public int[] numInFreqRange;
    public float[] meanAmplitudeInRange;
    public int multScalar;

	// Use this for initialization
	void Start () 
    {
        if (numFreqRanges <= 0)
        {
            numFreqRanges = 1;
        }

        numInFreqRange = new int[numFreqRanges];
        meanAmplitudeInRange = new float[numFreqRanges];
        GetProcessedData();
        InitialiseArrays ();
	}
	
	// Update is called once per frame
	void Update () 
    {
        GetProcessedData();
        InitialiseArrays();
        GetMeanAmplitudes ();
	}

    private void GetProcessedData()
    {
        dataCount = fftController.ffteg.GetProcessedDataCount();
        Debug.Log(dataCount);
    }

    private void InitialiseArrays ()
    {
        int numRemainders = dataCount % numFreqRanges;

        for (int index = 0; index < numFreqRanges; index++)
        {
            numInFreqRange[index] = Mathf.FloorToInt(dataCount / numFreqRanges);

            if (numRemainders > 0)
            {
                numInFreqRange[index]++;
                numRemainders--;
            }
        }
    }

    private void GetMeanAmplitudes ()
    {
        int currentSampleIndex = 0;

        for (int index = 0; index < numFreqRanges; index++)
        {
            float currentSum = 0;

            for (int numInRangeIndex = 0; numInRangeIndex < numInFreqRange[index]; numInRangeIndex++)
            {
                currentSum += fftController.ffteg.GetProcessedDataAt(currentSampleIndex + numInRangeIndex);
            }

            meanAmplitudeInRange[index] = (currentSum / numInFreqRange[index]) * multScalar;
            currentSampleIndex += numInFreqRange[index];
        }
    }
}
