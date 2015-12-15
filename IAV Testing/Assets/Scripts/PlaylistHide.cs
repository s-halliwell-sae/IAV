using UnityEngine;
using System.Collections;

public class PlaylistHide : MonoBehaviour {

    public GameObject playlist;
    bool activeBool = false;

	// Use this for initialization
	void Start () {

        playlist.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update () {


        if(Input.GetKeyDown(KeyCode.Space))
        {
            activeBool = !activeBool;
            
        }

        if(activeBool)
        {

            playlist.SetActive(true);

        }
        else
        {

            playlist.SetActive(false);

        }

	
	}
}
