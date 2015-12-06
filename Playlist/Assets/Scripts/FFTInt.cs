using UnityEngine;
using System.Collections;

public class FFTInt : MonoBehaviour {

	public int sampleSize = 8192;
	public int numBins;

	private int binNum;
	private float binSize;
	private float tipToNextBin;
	private float binTipScale;

	public float[] samples;
	public float[] bins;

	public AudioSource audioSource;

	//public bool doOnce = false;
	public GameObject bar;
	public GameObject[] bars;

	void Awake()
	{
		audioSource = GameObject.Find ("Audio Source").GetComponent<AudioSource> ();
		binSize = 0.1f;
		numBins = 457;
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

		//binSize = (binCount / binSize)

		for(int i = 0; i < samples.Length-1; i++)
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

		//Square root bin data
		for(int i = 0 ; i < bins.Length; ++i)
		{
			bins[i] = Mathf.Sqrt(bins[i]);
		}

		//How I used it to instantiate some bars
		//if(doOnce == false)
		//{
		//	bars = new GameObject[bins.Length];
		//	Vector3 pos = new Vector3(3,100,3);
			
		//	for(int i = 0 ; i < bins.Length; ++i)
		//	{
		//		GameObject theBar = Instantiate(bar, pos, Quaternion.identity) as GameObject;
		//		bars[i] = theBar;
		//		pos += Vector3.right;
		//	}
		//	doOnce = true;
		//}
		
		//How I used it to move some bars
		//for(int i = 0 ; i < bins.Length; ++i)
		//{
		//	bars[i].GetComponent<Grow>().grow(bins[i]);
		//}

		//Debug line draw
		//Vector3 curPos = new Vector3(0, 200, 0);
		//Vector3 step = Vector3.right;
		//float yscale = bins.Length;
		//for(int i = 0 ; i < bins.Length-1; ++i)
		//{
		//	Debug.DrawLine(curPos + Vector3.up*bins[i]*yscale , curPos + Vector3.up*bins[i+1]*yscale+step, Color.green);
		//	curPos += step;
		//}
	}

}