using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click3D : MonoBehaviour
{
    private int _index = 0;
    void ChangeColor()
    {
        if (_index == 0)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        }
        else
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.cyan);
        }
        _index = _index == 0 ? 1 : 0;
    }

    private void OnMouseDown()
    {
        ChangeColor();
    }
}
