using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class RotationDiagramItem : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public int PosId;

    private float _offsetX;

    private Action<float> _moveAction;

    private float _animTime = 1;

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
    /// <summary>
    /// 设置自身父物体
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    /// <summary>
    /// 设置自身精灵图片
    /// </summary>
    /// <param name="sprite"></param>
    public void SetSprite(Sprite sprite)
    {
        Image.sprite = sprite;
    }

    /// <summary>
    /// 对某个子项进行位置信息赋值，以确定其显示位置X轴、缩放、层级
    /// </summary>
    /// <param name="data">包含X轴、缩放和层级的数据</param>
    public void SetPosData(ItemPosData data)
    {
        Rect.DOAnchorPos(Vector2.right * data.X, _animTime);
        Rect.DOScale(data.ScaleTimes, _animTime);
        //Rect.anchoredPosition = Vector2.right * data.X;
        //Rect.localScale = Vector3.one * data.ScaleTimes;
        //transform.SetSiblingIndex(data.Order);
        StartCoroutine(Wait(data)); //延迟改变层级
    }

    IEnumerator Wait(ItemPosData data)
    {
        yield return new WaitForSeconds(_animTime * 0.3f);
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

    /// <summary>
    /// 响应drag拖动时每个子项的PosId
    /// </summary>
    /// <param name="symbol">x轴向拖动的正负符号，-1为向左， 1为向右</param>
    /// <param name="totalNum">整个循环的子项总数</param>
    public void ChangePosId(int symbol, int totalNum)
    {
        int id = PosId;
        id += symbol;
        if (id < 0)
        {
            //第0项往左移时，需要变为最后一项
            id += totalNum;
        }
        //末尾项向右移时，需要变为第0项，于是整体取余刚好可以满足效果
        PosId = id % totalNum;
    }
}
