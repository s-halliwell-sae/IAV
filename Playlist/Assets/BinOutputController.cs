using UnityEngine;
using System.Collections;

public class BinOutputController : MonoBehaviour
{

    public FFTInt fftController;    //This will need to be replaced with the correct sample array source
    //Number of raw smaples from the FFT
    private int dataCount;
    //Number of outputs
    public int numFreqRanges = 1;
    //Number of samples per output array
    public int[] numInFreqRange;
    //The mean amplitude of the samples within each range
    public float[] meanAmplitudeInRange;
    //The value used to scale the output values
    public int multScalar = 1;

    //The percentage of samples removed from the output arrays
    public int highFreqTrimPercent;
    public int lowFreqTrimPercent;
    //The calculated number of samples removed.
    private int lowFreqTrimNum;
    private int highFreqTrimNum;

	// Use this for initialization
	void Start () 
    {
        //Breaks the runtime if the trim percentage is greater than 100%
        if (lowFreqTrimPercent + highFreqTrimPercent >= 100)
        {
            Debug.LogError("Trim percentages are greater than 100%! Nothing would be displayed.");
            Debug.Break();
        }

        //Prevents dividing by zero
        if (numFreqRanges <= 0)
        {
            numFreqRanges = 1;
        }

        //Initialises the arrays
        numInFreqRange = new int[numFreqRanges];
        meanAmplitudeInRange = new float[numFreqRanges];
	}
	
	// Update is called once per frame
	void Update () 
    {
        PopulateCountArray ();
        GetMeanAmplitudes ();
	}

    /// <summary>
    /// Populates the count array by giving each array element the same number of samples, 
    /// then distributing the remainders; favouring the lower frequencies when allocating remainders.
    /// </summary>
    private void PopulateCountArray ()
    {
        //Gets the number of raw samples from the FFT array source
        dataCount = fftController.samples.Length;    //This will need to be replaced with the correct sample array source

        //Sets the number of samples that are excluded
        lowFreqTrimNum = dataCount * lowFreqTrimPercent / 100;
        Debug.Log("low "+lowFreqTrimNum);
        highFreqTrimNum = dataCount * highFreqTrimPercent / 100;
        Debug.Log("high "+highFreqTrimNum);

        dataCount -= (lowFreqTrimNum + highFreqTrimNum);
        

        //Gets the number of remainders
        int numRemainders = dataCount % numFreqRanges;

        //Gives each array element the same number of samples, with the remainders being added (one by one) until they are all used.
        for (int index = 0; index < numFreqRanges; index++)
        {
            //Gets the number that can be evenly distributed
            numInFreqRange[index] = Mathf.FloorToInt(dataCount / numFreqRanges);

            //If there are still remainders, add one to the current array element
            if (numRemainders > 0)
            {
                numInFreqRange[index]++;
                numRemainders--;
            }
        }
    }

    /// <summary>
    /// Gets the mean (average) of all samples within each range. Fills the ouput array.
    /// </summary>
    private void GetMeanAmplitudes ()
    {
        //Sets the initial offset for the index to the lowFreqTrimNum, 
        //forcing the required number of low-frequency samples to be ignored.
        int currentSampleIndex = lowFreqTrimNum;

        //Loops over each frequency range.
        for (int index = 0; index < numFreqRanges; index++)
        {
            //The sum for this frequency range, defaults at zero.
            float currentSum = 0;

            //Loops the number of times specified in the relevant numInFreqRange element.
            for (int numInRangeIndex = 0; numInRangeIndex < numInFreqRange[index]; numInRangeIndex++)
            {
                //Adds the amplitude of the current index to the sum.
                currentSum += fftController.samples[currentSampleIndex + numInRangeIndex];     //This will need to be replaced with the correct sample array source
            }

            //Gets the mean and reassigns the required array element.
            meanAmplitudeInRange[index] = (currentSum / numInFreqRange[index]) * multScalar;
            //Increments the index offset to allow the extraction of the next set of data.
            currentSampleIndex += numInFreqRange[index];

            //The highFreqTrimNum doesn't have any further use as it has already scaled the number of samples being extracted.
        }
    }
}
