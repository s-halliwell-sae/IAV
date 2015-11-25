using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/*
	would love to have reference to ffti in eventparams but it causes a bunch of
	serialisation depth warnings in fftEvent, due to unity's serialisation method
	and that's SUPER ugly. Doesn't seem to break anything but is ick.
*/

//data given to subscribers in FFTEvent
[System.Serializable]
public struct FFTEventParams
{
	//current value of the bin/band
	public float value;
	//state of the trigger, is it currently closed or not and was it closed last frame or not
	public bool isClosed, wasClosed;

	public FFTEventTrigger sender;
}

[System.Serializable]
public class FFTEvent : UnityEvent<FFTEventParams>{};

/*
	Is to be added to an FFTInterpreter, it will then fire the OnFFTEvent once the conditions are met.
	Behaves like a noise gate with attack and release limits
*/
[System.Serializable]
public class FFTEventTrigger {

	//percentage based bin to listen to, 0=0hrz, 1=22khrz 
	[Range(0.0f,1.0f)]
	public float startBin = 0, endBin = 1;

	[Range(0.0f,1.0f)]
	public float openValue = 0.5f, closeValue = 0.5f;

	public float attackTime = 0.02f, // bin must be above the open value for this many seconds to open
				releaseTime; // bin must be below the close value for this many seconds to close

	private bool isClosed = true;

	private float timer = 0;

	public FFTEvent OnFFTEvent;

	//called by the FFTInterpreter this is attached to, visitor style.
	//N.B. you should be able to safely add the same FFTEventTrigger to multiple FFTInterpreters
	public void Process(FFTInterpreter ffti)
	{		
		int i = ffti.PercentToBin(startBin), upperLim = ffti.PercentToBin(endBin)+1;

		float val = 0;

		for(;i<upperLim;i++)
		{
			val = Mathf.Max(val, ffti.GetProcessedDataAt(i));
		}

		
		if(isClosed)
		{
			if(val > openValue)
			{
				timer+= Time.deltaTime;
			}
			else
			{
				timer = 0;
			}
			
			if(timer > attackTime)
			{
				FireOpen(val);
			}
		}
		else
		{
			if(val > closeValue)
			{
				FireOpen(val);
			}
			else
			{
				timer += Time.deltaTime;

				if(timer > releaseTime)
				{
					FireClose(val);
				}
				else
				{
					//fire open but don't reset
					Fire (true, false, val);
				}
			}
		}
	}

	private void FireOpen(float val)
	{
		Fire (true, true, val);
	}

	private void FireClose(float val)
	{
		Fire (false, true, val);
	}

	private void Fire(bool open, bool resetTimer, float val)
	{
		if(resetTimer)
			timer = 0;

		FFTEventParams p = new FFTEventParams();
		p.value = val;
		p.wasClosed = isClosed;
		
		isClosed = !open;

		p.isClosed = isClosed;

		OnFFTEvent.Invoke(p);
	}
}
