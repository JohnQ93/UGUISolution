using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeftMouseClick : MonoBehaviour
{
    private int _index = 0;
    private GraphicRaycaster _graphic;
    // Start is called before the first frame update
    void Start()
    {
        _graphic = FindObjectOfType<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isOverUI())
        {
            ChangeColor();
        }

        if (Input.GetMouseButtonUp(1) && EventSystem.current.IsPointerOverGameObject())  //表示鼠标停在任一物体上返回true
        {
            ChangeColor();
        }

    }

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

    public bool isOverUI()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.pressPosition = Input.mousePosition;
        data.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        _graphic.Raycast(data, results);

        return results.Count > 0;  //返回true则表示当前点击到了UI，false则表示未点击到UI
    }
}
