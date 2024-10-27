using UnityEngine;
using System.Collections;

public class LightSwitch : MonoBehaviour {

    public bool onSwitch;
    public float canSwitch;
    public bool lightStatus;
    public GameObject theLight;
    public AudioSource audLight;

	void OnTriggerEnter(Collider other)
    {
        onSwitch = true;
    }

    void OnTriggerExit(Collider other)
    {
        onSwitch = false;
    }

    void Update()
    {
        if(theLight.active == true)
        {
            lightStatus = true;
        }
        else
        {
            lightStatus = false;
        }

        if (onSwitch)
        {
            if (lightStatus)
            {
                if (Input.GetKeyDown(KeyCode.E) && Time.time > canSwitch)
                {
                    theLight.active = false;
                    audLight.Play();
                    canSwitch = Time.time + 1.2f;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E) && Time.time > canSwitch)
                {
                    theLight.active = true;
                    audLight.Play();
                    canSwitch = Time.time + 0.6f;
                }
            }
        }
    }

    void OnGUI()
    {
        if (onSwitch)
        {
            if (lightStatus)
            {
                GUI.Box(new Rect(0, 0, 200, 20), "Press E to close the light");

            }
            else
            {
                GUI.Box(new Rect(0, 0, 200, 20), "Press E to open the light");
            }
        }
    }
}