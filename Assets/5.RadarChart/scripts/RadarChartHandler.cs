using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadarChartHandler : MonoBehaviour, IDragHandler
{
    private RectTransform _rect;
    private RectTransform Rect
    {
        get
        {
            if(_rect == null)
            {
                _rect = GetComponent<RectTransform>();
            }
            return _rect;
        }
    }

    private Image _image;
    private Image Image
    {
        get
        {
            if(_image == null)
            {
                _image = GetComponent<Image>();
            }
            return _image;
        }
    }
    public void SetParent(Transform parent)
    {
        Rect.SetParent(parent);
    }

    public void ChangeSprite(Sprite sprite)
    {
        Image.sprite = sprite;
    }

    public void ChangeColor(Color color)
    {
        Image.color = color;
    }

    public void SetPos(Vector2 pos)
    {
        Rect.anchoredPosition = pos;
    }

    public void SetSize(Vector2 size)
    {
        Rect.sizeDelta = size;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Rect.anchoredPosition += eventData.delta / GetScale();
    }

    private float GetScale()
    {
        //lossyScale是当前rect的所有父物体缩放的累计值
        return Rect.lossyScale.x;
    }
}
