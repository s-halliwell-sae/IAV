using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class UiPlayList : MonoBehaviour {

	//Playlist
	public List<string> playList = new List<string> ();
	//Audio format lists
	private List<string> supportedFormats = new List<string> { ".ogg", ".wav" };
	//Playlist UI text list
	public List<Text> playListUIText = new List<Text> ();

	//www stream
	public WWW stream;

	//Audio source and audio clips
	public AudioSource audioSource;
	public AudioClip currentSong;
	public AudioClip nextSong;

	//Path to read audio files from
	public string directoryPath;
	public string directoryFolder;

	//UI
	public Slider scrollBar;
	public GameObject selectedSong;
	public string selectedSongName;

	void Awake()
	{
		//Path to read audio files from
		directoryFolder = "Songs";
		directoryPath = Path.Combine(Application.dataPath, directoryFolder);
		//Find audio source
		audioSource = GameObject.Find ("Audio Source").GetComponent<AudioSource> ();
		//Add audio file names to playlist 
		PopulatePlayList (directoryPath, playList);
		//Set scrollbar max length
		scrollBar.maxValue = playList.Count;
		UpdatePlaylistUI ();
		//GetAudioFile (playList [0]);

		//playList.Insert (1, "Test");
	}

	//Looks at all files @ 'path' and adds them to the 'list' if they are a supported format
	void PopulatePlayList(string path, List<string> list)
	{
		//Get directory info
		DirectoryInfo dirInfo = new DirectoryInfo (path);
		//Store all file info in an array
		FileInfo[] fileInfo = dirInfo.GetFiles ("*");
		//Iterate over each file in file info array
		foreach(FileInfo file in fileInfo)
		{
			//If file has a supported audio extension
			if(CheckAudioFormat(file.Extension))
			{
				//Add filename to song list
				list.Add (file.Name);
			}
		}
	}

	//Returns true if file extension matches a supported format string
	bool CheckAudioFormat(string fileExtension)
	{
		for(int ext = 0; ext < supportedFormats.Count; ext++)
		{
			if(fileExtension == supportedFormats[ext])
			{
				return true;
			}
		}
		return false;
	}
	
	//Streams an audio file to nextSong audio clip 
	void GetAudioFile(string fileName)
	{
		//Cleanup old GetAudioClip memory
		if(nextSong != null)
		{
			stream = null;
			Destroy(nextSong);
		}
		//Create local www path to audio file
		string url = "file://" + Path.Combine (directoryPath, fileName);
		//Start loading audio file
		stream = new WWW(url);
		while (!stream.isDone)
		//Get audio clip from stream
		nextSong = stream.GetAudioClip (false, true);
		//Send stream error to console
		if(stream.error != null)
		{
			Debug.Log(stream.error.ToString());
		}
		//Cleanup
		stream.Dispose ();
	}

	void SetAudio()
	{
		currentSong = nextSong;
		audioSource.clip = currentSong;
		Resources.UnloadUnusedAssets ();
	}

	public void PlayAudio()
	{
		if(selectedSongName != string.Empty)
		{
			GetAudioFile(selectedSongName);
		}
		else
		{
			GetAudioFile(playList[0]);
		}
		SetAudio ();
		audioSource.Play ();
	}

	public void StopAudio()
	{
		audioSource.Stop ();
	}

	// Use this for initialization
//	void Start ()
//	{
//
//	}
	
	// Update is called once per frame
	void Update () 
	{
		//Testing
		if(Input.GetKey(KeyCode.A))
		{
			audioSource.PlayOneShot(currentSong);
		}
		if(Input.GetKey(KeyCode.B))
		{
			SetAudio();
		}
		if(Input.GetKey(KeyCode.C))
		{
			Resources.UnloadUnusedAssets();
		}

	}

	//Displays song list on UI
	public void UpdatePlaylistUI()
	{
		selectedSong = null;
		selectedSongName = string.Empty;

		int sb = (int)(scrollBar.value);

		//Displays songs on the playlist relative to the scrollbar value
		for(int textNum = 0; textNum < playListUIText.Count; textNum++)
		{
			if(textNum+sb <= playList.Count-1)
			{
				playListUIText[textNum].text = playList[textNum+sb];
			}
			else
			{
				playListUIText[textNum].text = string.Empty;
			}
		}
	}

	//Used by DragAndDrop script to move songs and down the list 
	public void MoveSong(string nameA, string nameB)
	{
		int index1 = FindSongIndex(nameA);
		int index2 = FindSongIndex(nameB);

		//Move song up the list
		if(index1 > index2)
		{
			playList.Insert(index2, playList[index1]);
			playList.RemoveAt(index1+1);

		}
		//Move song down the list
		else if(index1 < index2)
		{
			playList.Insert(index2+1, playList[index1]);
			playList.RemoveAt(index1);
		}

		UpdatePlaylistUI ();
	}

	//Returns list index of a "song" string 
	public int FindSongIndex(string name)
	{
		for(int i = 0; i < playList.Count; i++)
		{
			if(playList[i] == name)
			{
				return i;
			}
		}
		Debug.Log ("FindSongIndex, name not found");
		return -1;
	}

//	IEnumerator Wait (float waitTime)
//	{
//		float endTime = Time.realtimeSinceStartup + waitTime;
//		Debug.Log ("waiting?");
//		while (Time.realtimeSinceStartup < endTime) { Debug.Log ("waitinggg?"); }
//		yield return null;
//	}
}
