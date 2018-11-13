using UnityEngine;
using System.Collections;

public class GemInlaySlotStateUI : MonoBehaviour {

    public GameObject openGo;
    public GameObject lockGo;
    public UILabel labOpenDes;

    public void SetData(string _des,bool isOpen)
    {
        if (labOpenDes != null)
        {
            labOpenDes.enabled = !isOpen;
            labOpenDes.text = _des;
        }
        if (openGo != null) openGo.SetActive(isOpen);
        if (lockGo != null) lockGo.SetActive(!isOpen);
    }
}
