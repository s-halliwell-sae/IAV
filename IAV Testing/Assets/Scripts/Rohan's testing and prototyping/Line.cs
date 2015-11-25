using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {

    public LineController3D lineController;
    public int lineTypeIndex;
    public float moveSpeedScalar;
    public float maxInDirection;
    private float movedCurrent;
    public float inputValue;
    public AudioSource source;
    public float clipLength;

	// Use this for initialization
	void Start () 
    {
        clipLength = source.clip.length;
	}
	
	// Update is called once per frame
	void Update () 
    {
        GetValue();
        Movement();
	}

    private void Movement ()
    {
        //transform.localPosition -= new Vector3(0, moveSpeedScalar * inputValue, 0);
        GetComponent<Rigidbody>().velocity = new Vector3(moveSpeedScalar * inputValue, 0, 0);

        //GetComponent<Rigidbody>().AddForce(transform.right * moveSpeedScalar * inputValue);

        if (!source.isPlaying)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        //transform.Translate(new Vector3(0, 0, 0));

        //transform.Translate(transform.forward * moveSpeedScalar * inputValue);
        //movedCurrent += inputValue;
        //if (movedCurrent >= maxInDirection)
        //{
        //    //transform.Rotate(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //    movedCurrent = 0;
        //}
    }

    private void GetValue ()
    {
        inputValue = lineController.meanAmplitudeInRange[lineTypeIndex];
    }
}
