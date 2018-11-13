using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

    public Camera target = null;

    Vector3 originScale;

    float correctionValue = 0.4f;

    void Awake()
    {
        originScale = this.gameObject.transform.localScale;
    }


    void Update()
    {
        transform.eulerAngles = target.transform.eulerAngles;
        transform.localScale = originScale * GetCaculate();
    }



    float GetCaculate()
    {
        float baseValue = Mathf.Abs(this.gameObject.transform.position.z - target.transform.position.z) / target.fieldOfView;
        return Mathf.Min(1.5f, correctionValue + baseValue * 2f);
    }
}
