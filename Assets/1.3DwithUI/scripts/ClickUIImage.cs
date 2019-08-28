using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickUIImage : MonoBehaviour, IPointerClickHandler
{
    private int _index = 0;

    void ChangeColor()
    {
        if (_index == 0)
        {
            GetComponent<Image>().color = Color.blue;
        }
        else
        {
            GetComponent<Image>().color = Color.black;
        }
        _index = _index == 0 ? 1 : 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeColor();
        ExecuteAll(eventData);
    }

    public void ExecuteAll(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results)
        {
            if(result.gameObject != gameObject)
            {
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
