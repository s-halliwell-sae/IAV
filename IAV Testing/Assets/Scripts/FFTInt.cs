using UnityEngine;
using System.Collections;

public class FFTInt : MonoBehaviour {

	public int sampleSize = 2048;
	public int numBins;
	public int numAvgBins;

	private int binNum;
	private float tipToNextBin;
	private float binTipScale;

	public float[] samples;
	public float[] bins;
	public float[] avgBins;

	public AudioSource audioSource;

	public bool doOnce = false;
	//public GameObject bar;
	//public GameObject[] bars;

	void Awake()
	{
		//audioSource = GameObject.Find ("FFT Source").GetComponent<AudioSource> ();
		numBins = 457;
		numAvgBins = (int) Mathf.Round (numBins / 10);
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(audioSource.isPlaying)
		{
			BinAudioData();
		}
	}

	void BinAudioData()
	{
		//Get spectrum data from audiosource
		samples = audioSource.GetSpectrumData(sampleSize,0,FFTWindow.BlackmanHarris);
		//Initialise bin array
		bins = new float[numBins];
		binTipScale = 1.02f;
		binNum = 0; //Current bin
		tipToNextBin = 1;

		for(int i = 0; i < samples.Length; i++)
		{
			//Fill current bin
			bins[binNum] += samples[i];

			//Goto next bin
			if(i >= tipToNextBin)
			{
				tipToNextBin = tipToNextBin * binTipScale;
				binNum++;
			}
		}

		SquareBinData ();

		AverageBinData();



		//Debug draw

//		Vector3 curPos = new Vector3(0, 200, 0);
//		Vector3 step = Vector3.right;
//		float yscale = bins.Length;
//		for(int i = 0 ; i < bins.Length-1; ++i)
//		{
//			Debug.DrawLine(curPos + Vector3.up*bins[i]*yscale , curPos + Vector3.up*bins[i+1]*yscale+step, Color.green);
//			curPos += step;
//		}
	}

	void SquareBinData()
	{
		//Square root bin data
		for(int i = 0 ; i < bins.Length; ++i)
		{
			bins[i] = Mathf.Sqrt(bins[i]);
		}
	}

	//Compress bin data into average data
	void AverageBinData()
	{
	//Set vars for averaging
	int binCounter = 0;
	int avgBinCount = 10;
	float avg = 0;
	
	//Initialise avg bins
	avgBins = new float[numAvgBins];

	for(int i = 0 ; i < bins.Length; i++)
	{
		avg += bins[i];
		
		if(i == avgBinCount)
		{
			//Average data
			avg = avg / numAvgBins;
			//Put into avg bin
			avgBins[binCounter] = avg;
			//increment avgbins
			avgBinCount = avgBinCount + 10;
			//Reset avg
			avg = 0;
			//incrment counter
			binCounter++;
		}
	}

	}

	public void ClearBins()
	{
		bins = new float[numBins];
		avgBins = new float[numAvgBins];
	}

}
