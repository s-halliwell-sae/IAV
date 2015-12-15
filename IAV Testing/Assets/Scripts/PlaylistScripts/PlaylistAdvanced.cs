using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaylistAdvanced : MonoBehaviour {

	public GameObject panel;
	public AudioSource audioSource;
	public Toggle toggle;
	public Button setDefault;

	public Slider volume;
	public Slider panning;

	void Awake()
	{
		//audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource> ();
		volume.value = audioSource.volume;
		panning.value = audioSource.panStereo;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(toggle.isOn)
		{
			panel.SetActive(true);
		}
		else
		{
			panel.SetActive(false);
		}

		if(audioSource.volume != volume.value)
		{
			UpdateVolume (volume.value);
		}

		if(audioSource.panStereo != panning.value)
		{
			UpdatePanning (panning.value);
		}
	}

	void UpdateVolume(float value)
	{
		audioSource.volume = value;
	}

	void UpdatePanning(float value)
	{
		audioSource.panStereo = value;
	}

	public void SetDefault()
	{
		float vol = 0.5f;
		float pan = 0f;

		UpdateVolume (0.5f);
		volume.value = vol;
		UpdatePanning (0f);
		panning.value = pan;
	}
}
