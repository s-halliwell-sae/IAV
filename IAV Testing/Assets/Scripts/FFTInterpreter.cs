using UnityEngine;
using System.Collections.Generic;


/*
	Handles listening to the GetSpectrumData from an audiosource, the number of samples that should be taken,
	converting data to bins and reinterprets data from its linear format to logarithmic. This is the format 
	you would be more accustomed to, similar to what an equaliser shows.
	
	FFTTrigers are added here so events can be fired when certain bands/bins reach a certain intensity.
	See FFTTriggers for more info.
*/
[System.Serializable]
public class FFTInterpreter
{
	/*
		samples requested from GetSpectrumData, higher number means more bins with higher accuracy are possible 
		but is also more expensive. In basic observation in the profiler they are linearly more expensive, 
		that is to say, 128 takes twice as much time to fully process (both internally to unity and us) than 64.
	*/
	public enum SampleCount 
	{
		_64 = 64, _128 = 128, _256 = 256, _512 = 512, _1024 = 1024, _2048 = 2048, _4096 = 4096, _8192 = 8192
	}
	
	/*
		A bin is the result of processing a number of samples across the spectrum. This controls if those samples 
		should all be added, which tends to skew towards the higher bins being larger by the nature of spectrum 
		being linear while sound is log. Or if we should simply keep the max value found amount the samples 
		destined for the bin in question, which doesn't seem bias but does result in smaller numbers alround. This
		can be accounted for with the processmode and the data process mode.
	*/
	public enum BinAccumMode
	{
		Max,
		Add,
	}
	
	/*
		After all samples in a bin have gone through the accum mode, do they need another layer of processing
		for example if added them all together you may then want them averaged by the number of sammples in the 
		bin to bring that number back down.
	*/
	public enum BinProcessMode
	{
		None,
		AverageSamples,
		MultiplyBySamples,
		MultiplyByBinNumber
	}
	
	/*
		Used after moving data from bins to processedData. The results from get spectrum data are all between 0-1 
		but tend to be very low, these can make the data easier to look at in debug and easier to reason with 
		when determining values for FFTTriggers to fire on.
	*/
	public enum DataProcessMode
	{
		Raw,
		Inverse,	//not recommended as values get very close to 0
		OneMinus,
		OneMinusInverse,
		Squared,
		Cubed,
		SquareRoot,
		CubeRoot	//currently the most sensitive way to present data
	}
	
	//fftwindow given to getspectrumdata
	public FFTWindow window;
	
	//source we listen to
	public AudioSource audioSource;
	
	public SampleCount sampleCount;
	public BinAccumMode accumMode;
	public BinProcessMode binProcessMode;
	public DataProcessMode dataProcessingMode;
	
	//internal samples cache
	private float[] samples;
	
	private float[] bins;
	[Range(0.01f,1.0f)]
	/*
		smaller value scales down the size of individual bins, resulting in more bins/bands.
		
		If you only care about is there lots of bass or lots of treble then 1 is fine, if you 
		wanted to try to find if the kick drum is being hit versus the bass guitar then a lower 
		number would be required. 
		
		N.B. Total number of bins is also controlled by the samples being requested
	*/
	public float binSize = 1;
	private float nextBinTipScale;
	public bool debugDraw = false;

	public List<FFTEventTrigger> fftTriggers;
	
	//internal state shared between functions
	int curBin = 0;
	float tipToNextBin = 1;
	int samplesInBin = 0;

	public void Init()
	{
		samples = new float[(int)sampleCount];
		//this method is approximate
		int binCount = Mathf.CeilToInt(Mathf.Log((float)samples.Length,2));
		binCount = (int)(binCount / binSize) + 2;//make sure we have the space at the end to not run over
		bins = new float[binCount];
		nextBinTipScale = Mathf.Pow(2,binSize);
	}
	
	
	public void Sample()
	{
		audioSource.GetSpectrumData(samples,0, window);
		
		for(int i = 0 ; i < bins.Length; ++i)
		{
			bins[i] = 0;
		}
		
		
		curBin = 0;
		tipToNextBin = 1;
		samplesInBin = 0;
		
		for(int i = 0; i < samples.Length-1; i++)
		{
			samplesInBin++;
			//if(curBin < bins.Length)
			switch(accumMode)
			{
			case BinAccumMode.Add:
				bins[curBin] += samples[i];
				break;
			case BinAccumMode.Max:
				bins[curBin] = Mathf.Max(bins[curBin],samples[i]);
				break;
			default:
				Debug.LogError("unhandled mode");
				break;
			}
			
			if( i >= tipToNextBin)
			{
				TipToNextBin();
			}
		}
		
		//did the last bin get processed?
		if(curBin < bins.Length)
			TipToNextBin();
		
//		var tmp = processedData;
//		processedData = prevProcessedData;
//		prevProcessedData = tmp;
		
		for(int i = 0 ; i < bins.Length; ++i)
		{
			switch(dataProcessingMode)
			{
			case DataProcessMode.Raw:
				break;
			case DataProcessMode.Inverse:
				bins[i] = 1.0f / bins[i];
				break;
			case DataProcessMode.OneMinus:
				bins[i] = 1 - bins[i];
				break;
			case DataProcessMode.OneMinusInverse:
				bins[i] = 1.0f / (1 - bins[i]);
				break;
			case DataProcessMode.Squared:
				bins[i] = bins[i]*bins[i];
				break;
			case DataProcessMode.Cubed:
				bins[i] = bins[i]*bins[i]*bins[i];
				break;
			case DataProcessMode.SquareRoot:
				bins[i] = Mathf.Sqrt(bins[i]);
				break;
			case DataProcessMode.CubeRoot:
				bins[i] = Mathf.Pow(bins[i],1.0f/3.0f);
				break;
				
			}
			//Mathf.Pow (2,Mathf.Exp(1+bins[i]))/2.0f;
		}
		
		
		if(debugDraw)
			DrawDebugVis();

		for(int i = 0 ; i < fftTriggers.Count; i++)
		{
			fftTriggers[i].Process(this);
		}
		
	}
	private void TipToNextBin()
	{
		tipToNextBin = tipToNextBin * nextBinTipScale;
		
		switch(binProcessMode)
		{
		case BinProcessMode.AverageSamples:
			bins[curBin] = bins[curBin] / samplesInBin;
			break;
		case BinProcessMode.MultiplyBySamples:
			bins[curBin] = bins[curBin] * samplesInBin;
			break;
		case BinProcessMode.MultiplyByBinNumber:
			bins[curBin] = bins[curBin] * curBin;
			break;
		}
		curBin++;
		samplesInBin = 0;
	}
	
	public void DrawDebugVis()
	{
		Vector3 curPos = Vector3.zero;
		Vector3 step = Vector3.right;
		float yscale = bins.Length;
		for(int i = 0 ; i < bins.Length-1; ++i)
		{
			Debug.DrawLine(curPos + Vector3.up*bins[i]*yscale , curPos + Vector3.up*bins[i+1]*yscale+step, Color.green);
			curPos += step;
		}
	}

	public int PercentToBin(float f)
	{
		return (int)((bins.Length - 1) * f);
	}
	
	public int GetProcessedDataCount()
	{
		return bins.Length;
	}
	
	public float GetProcessedDataAt(int index)
	{
		return bins[index];
	}
}