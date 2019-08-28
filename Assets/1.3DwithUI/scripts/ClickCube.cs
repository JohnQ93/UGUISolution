using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCube : MonoBehaviour, IPointerClickHandler
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
    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeColor();
    }
}
