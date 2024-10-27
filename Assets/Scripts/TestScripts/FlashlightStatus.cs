using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightStatus : MonoBehaviour
{
	[SerializeField] Light flashLight;
	[SerializeField] AudioSource lightOnSound, lightOffSound;
	bool flashlightStatus;
    
    void Start()
    {
	    flashlightStatus = false;
	    flashLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
	    if(Input.GetKeyDown(KeyCode.F))
	    {
	    	flashlightStatus = !flashlightStatus;
	    	
	    	if(flashlightStatus)
	    	{
	    		flashLight.enabled = true;
	    		lightOnSound.Play();
	    	}
	    	else
	    	{
	    		flashLight.enabled = false;
	    		lightOffSound.Play();
	    	}
	    }
    }
}
