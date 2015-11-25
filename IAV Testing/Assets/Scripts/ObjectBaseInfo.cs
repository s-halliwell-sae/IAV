using UnityEngine;
using System.Collections;



public class ObjectBaseInfo : MonoBehaviour {

    public Vector3 startPosition;
    public Vector3 startScale;
    public Quaternion startRotation;
    public int frequencyLowRange;
    public float frequencyHighRange;
    public bool controlledByDirector;
    public float speed;
    public float lastValue;
    
    public Vector3 startPos;
    public Color startColour;
    public Renderer myRenderer;
    public Light myLight;

	// Use this for initialization
	void Start () {

        
	
	}
	
    public void Initialize()
    {
        myRenderer = gameObject.GetComponentInChildren<Renderer>();
        //startColour = myRenderer.material.color;
        myRenderer.material.color = startColour;
        myLight = gameObject.GetComponentInChildren<Light>();
        myLight.color = startColour;


        startPosition = transform.position;
        startScale = transform.localScale;
        startRotation = transform.rotation;
        controlledByDirector = false;
    }
	// Update is called once per frame
	void Update () {
	
        //if (!controlledByDirector)
        //{
        //    transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * speed);
        //    transform.localScale = Vector3.Lerp(transform.localScale, startScale, Time.deltaTime * speed);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, Time.deltaTime * speed);

        //}

	}
}
