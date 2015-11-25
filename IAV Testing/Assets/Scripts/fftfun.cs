using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class fftfun : MonoBehaviour {

	//public GameObject pre;
//	public FFTWindow window;
//	public AudioSource audioSource;
//	private float[] fa;
//	public int numSamples;
//	public float[] bins;
//	[Range(0.01f,1.0f)]
//	public float binSize = 1;

	public FFTInterpreter ffteg;
	public int binToWatch;
	public bool isClosed;
	public float openThres, closeThres;
	
	public GameObject[] gos;
	public float scaleMul = 1;
	
	// Use this for initialization
	void Start () 
	{
		ffteg.Init();
	}
	
	
	[ContextMenu("AddEvent")]
	public void AddEmptyEvent()
	{
		ffteg.fftTriggers.Add(new FFTEventTrigger());
	}
	
	void Update()
	{
		ffteg.Sample();
		

	}

	public void OnFFTEvent1(FFTEventParams p)
	{
		gos[0].SetActive(!p.isClosed);
		gos[0].transform.localScale = Vector3.one * p.value * scaleMul * (p.wasClosed ? scaleMul : 1);
	}
	
	public void OnFFTEvent2(FFTEventParams p)
	{
		gos[1].SetActive(!p.isClosed);
		gos[1].transform.localScale = Vector3.one * p.value * scaleMul * (p.wasClosed ? scaleMul : 1);
	}
	
	public void OnFFTEvent3(FFTEventParams p)
	{
		gos[2].SetActive(!p.isClosed);
		gos[2].transform.localScale = Vector3.one * p.value * scaleMul * (p.wasClosed ? scaleMul : 1);
	}
	
	public void OnFFTEvent4(FFTEventParams p)
	{
		gos[3].SetActive(!p.isClosed);
		gos[3].transform.localScale = Vector3.one * p.value * scaleMul * (p.wasClosed ? scaleMul : 1);
	}
	
	public void OnFFTEvent5(FFTEventParams p)
	{
		gos[4].SetActive(!p.isClosed);
		gos[4].transform.localScale = Vector3.one * p.value * scaleMul * (p.wasClosed ? scaleMul : 1);
	}
	
	public void OnFFTEvent6(FFTEventParams p)
	{
		gos[5].SetActive(!p.isClosed);
		gos[5].transform.localScale = Vector3.one * p.value * scaleMul * (p.wasClosed ? scaleMul : 1);
	}
	
	public void OnFFTEvent7(FFTEventParams p)
	{
		gos[6].SetActive(!p.isClosed);
		gos[6].transform.localScale = Vector3.one * p.value * scaleMul * (p.wasClosed ? scaleMul : 1);
	}
	
	public void OnFFTEvent8(FFTEventParams p)
	{
		gos[7].SetActive(!p.isClosed);
		gos[7].transform.localScale = Vector3.one * p.value * scaleMul * (p.wasClosed ? scaleMul : 1);
	}
	
	public void OnFFTEvent9(FFTEventParams p)
	{
		gos[8].SetActive(!p.isClosed);
		gos[8].transform.localScale = Vector3.one * p.value * scaleMul * (p.wasClosed ? scaleMul : 1);
	}
	
	public void OnFFTEvent10(FFTEventParams p)
	{
		gos[9].SetActive(!p.isClosed);
		gos[9].transform.localScale = Vector3.one * p.value * scaleMul * (p.wasClosed ? scaleMul : 1);
	}
}