using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class IAVDirector : MonoBehaviour
{



    public fftfun fftController;
    public LineController3D lineControl;
    private int dataCount;
    public float currentValue;
    public float speed;
    public float scaleMult;
    public List<ObjectBaseInfo> objectList;

    public float width;
    public bool createBarBlock;
    public bool randomColours;
    [Range(0.0f, 1.0f)]
    public float colourMod = 0;
    [Range(0.0f, 1.0f)]
    public float r = 0.5f;
    [Range(0.0f, 1.0f)]
    public float g = 0.5f;
    [Range(0.0f, 1.0f)]
    public float b = 0.5f;

    public bool lerpBars;
    public float lerpBarsSpeed;

    public float distInc;
    public int barQuantity = 0;
    public Vector3 startPos;
    public GameObject barPrefab;



    // Use this for initialization
    void Start()
    {
        if (createBarBlock)
        {
            CreateBars();
        }
        else
        {
            objectList = GameObject.FindObjectsOfType<ObjectBaseInfo>().ToList();
        }


    }

    // Update is called once per frame
    void Update()
    {






        GetProcessedData();


    }

    private void CreateBars()
    {

        colourMod = colourMod / 10;
        int colourIndex = 0;
        bool reverse = false;

        Vector3 spawnPos = startPos;

        barQuantity = fftController.ffteg.GetProcessedDataCount();


        for (int index = 0; index <= barQuantity; index++)
        {

            if (!reverse)
            {
                spawnPos.x -= distInc;
            }
            else
            {
                spawnPos.x += distInc;
            }

            if (index % width == 0)
            {
                spawnPos.z -= distInc;
                // spawnPos.x = startPos.x;
                colourIndex++;
                reverse = !reverse;
            }

            if (randomColours)
            {
                r = Random.Range(0.25f, 0.5f);
                g = Random.Range(0.25f, 0.5f);
                b = Random.Range(0.25f, 0.5f);
            }

            if (colourIndex == 0)
            {
                Mathf.Clamp01(r += colourMod);
                Mathf.Clamp01(g -= colourMod);
                Mathf.Clamp01(b -= colourMod);
            }
            else if (colourIndex == 1)
            {
                Mathf.Clamp01(r -= colourMod);
                Mathf.Clamp01(g += colourMod);
                Mathf.Clamp01(b -= colourMod);
            }
            else if (colourIndex == 2)
            {
                Mathf.Clamp01(r -= colourMod);
                Mathf.Clamp01(g -= colourMod);
                Mathf.Clamp01(b += colourMod);

            }
            else
            {
                colourIndex = 0;
            }


            GameObject currentObject = Instantiate(barPrefab, spawnPos, new Quaternion(transform.rotation.x, transform.rotation.y + Random.Range(-140, 140), transform.rotation.z, transform.rotation.w)) as GameObject;
            ObjectBaseInfo currentOBI = currentObject.GetComponent<ObjectBaseInfo>();
            currentOBI.startColour = new Color(r, g, b, 0.85f);

            //  currentOBI.startScale = new Vector3(currentOBI.startScale.x, 0.25f + (index * percentAmt*2), currentOBI.startScale.z);
            currentOBI.frequencyLowRange = index;
            currentOBI.frequencyHighRange = index;

            currentOBI.Initialize();

        }


        objectList = GameObject.FindObjectsOfType<ObjectBaseInfo>().ToList();

    }



    private void GetProcessedData()
    {

        foreach (ObjectBaseInfo item in objectList)
        {

            item.controlledByDirector = true;
            currentValue = item.lastValue;
            float rawValue = 0;
            if (item.frequencyLowRange < barQuantity)
            {
                rawValue = fftController.ffteg.GetProcessedDataAt(item.frequencyLowRange);
            }

            float newValue = (scaleMult * rawValue);

            if (currentValue < newValue && newValue > 0.01f)
            {
                currentValue = newValue;


            }
            else
            {
                currentValue = Mathf.Lerp(currentValue, 0.01f, Time.deltaTime * speed);
            }

            item.lastValue = currentValue;

            Vector3 lerpScale = new Vector3(item.startScale.x + (currentValue / 3), item.startScale.y + (currentValue), item.startScale.z + (currentValue / 3));
            if (lerpBars)
            {
                item.gameObject.transform.localScale = Vector3.Lerp(item.gameObject.transform.localScale, lerpScale, Time.deltaTime * lerpBarsSpeed);
            }
            else
            {
                item.gameObject.transform.localScale = lerpScale;
            }

            Color newColour = new Color(Mathf.Clamp(currentValue - 1, 0, item.startColour.r), Mathf.Clamp(currentValue - 1, 0, item.startColour.g), Mathf.Clamp(currentValue - 1, 0, item.startColour.b), Mathf.Clamp(currentValue - 1, 0, item.startColour.a));
            item.myRenderer.material.color = newColour;

            Color finalColor = newColour;
            item.myRenderer.material.SetColor("_EmissionColor", finalColor);

            item.myLight.intensity = currentValue - 1;
        }



    }

}
