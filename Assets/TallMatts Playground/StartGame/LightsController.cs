using UnityEngine;
using System.Collections;

public class LightsController : MonoBehaviour {

    public Light light;
    public Light spotLight;
    public float intensity = 0f;

	void Start () {
        RenderSettings.ambientLight = Color.black;
        RenderSettings.ambientIntensity = intensity;
        
        light.intensity = intensity;

        Invoke("RaiseLights", 3);
    }
	
	void RaiseLights () {
        if (intensity == 0)
        {
            RenderSettings.ambientLight = Color.white;
        }
        if (intensity < 1)
        {
            RenderSettings.ambientIntensity = intensity;
            light.intensity = intensity;
            spotLight.intensity = 2 - (intensity * 2);
            intensity += 0.025f;
            Invoke("RaiseLights", 0.025f);
        }
    }
}
