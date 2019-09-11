using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopListItem : MonoBehaviour
{
    private int _id;
    private float _offset;
    private int _showItemNum;
    private RectTransform _content;

    private RectTransform _rect;
    public RectTransform Rect
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
    public Image Image
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

    private Text _text;
    public Text Text
    {
        get
        {
            if(_text == null)
            {
                _text = GetComponent<Text>();
            }
            return _text;
        }
    }

    private Func<int, LoopListItemModel> _getData;

    public void init(int id, float offset, int num)
    {
        _content = transform.parent.GetComponent<RectTransform>();
        _id = id;
        _offset = offset;
        _showItemNum = num;
    }

    public void AddGetDataListener(Func<int, LoopListItemModel> getData)
    {
        _getData = getData;
    }

    public void OnValueChange()
    {
        int startId, endId;
        UpdateIdRange(out startId, out endId);
        JudgeSelfId(startId, endId);
    }

    private void UpdateIdRange(out int startId, out int endId)
    {
        startId = Mathf.FloorToInt(_content.anchoredPosition.y / (Rect.rect.height + _offset));
        endId = startId + _showItemNum - 1;
    }

    //判断当前id是否在范围内，如果超出则做对应首尾修改
    private void JudgeSelfId(int startId, int endId)
    {
        if(_id < startId)
        {
            ChangeId(endId);
        }
        else if(_id > endId)
        {
            ChangeId(startId);
        }
    }

    private void ChangeId(int id)
    {

    }
}
