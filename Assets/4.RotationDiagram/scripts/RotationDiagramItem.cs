using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotationDiagramItem : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public int PosId;

    private float _offsetX;

    private Action<float> _moveAction;

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
    private RectTransform _rect;
    private RectTransform Rect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();
            }
            return _rect;
        }
    }
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void SetSprite(Sprite sprite)
    {
        Image.sprite = sprite;
    }

    public void SetPosData(ItemPosData data)
    {
        Rect.anchoredPosition = Vector2.right * data.X;
        Rect.localScale = Vector3.one * data.ScaleTimes;
        transform.SetSiblingIndex(data.Order);
    }

    public void AddMovingListener(Action<float> action)
    {
        _moveAction = action;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _offsetX += eventData.delta.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _moveAction(_offsetX);
        _offsetX = 0;
    }

    public void ChangePosId(int symbol, int totalNum)
    {
        int id = PosId;
        id += symbol;
        if (id < 0)
        {
            id += totalNum;
        }
        PosId = id % totalNum;
    }
}
