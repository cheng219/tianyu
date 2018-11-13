using UnityEngine;
using System.Collections;

public class LightMapsLog : MonoBehaviour {

	
	void Update () {

        if (Input.GetKey(KeyCode.F1))
        {
            Debug.Log("LightmapSettings.lightmapsMode = " + LightmapSettings.lightmapsMode);
            Debug.Log("LightmapSettings.lightProbes.count = " + LightmapSettings.lightProbes.count);
            Debug.Log("QualitySettings.desiredColorSpace = " + QualitySettings.desiredColorSpace);
            foreach (LightmapData item in LightmapSettings.lightmaps)
            {
                Debug.Log("lightmapFar = " + item.lightmapFar.name + " , lightmapNear = " + item.lightmapNear.name);
            }
        }
	}
}
