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
                _image = transform.Find("Icon").GetComponent<Image>();
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
                _text = transform.Find("Describe").GetComponent<Text>();
            }
            return _text;
        }
    }

    private Func<int, LoopListItemModel> _getData;
    private LoopListItemModel _model;

    public void init(int id, float offset, int num)
    {
        _id = -1;
        _content = transform.parent.GetComponent<RectTransform>();
        _offset = offset;
        _showItemNum = num;

        ChangeId(id);
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
        int offset = 0; //同时有多个子项超出范围时，要针对每一项做偏差
        if(_id < startId)
        {
            offset = startId - _id - 1;
            ChangeId(endId - offset);
        }
        else if(_id > endId)
        {
            offset = _id - endId - 1;
            ChangeId(startId + offset);
        }
    }

    private void ChangeId(int id)
    {
        if(_id != id && JudgeIdValid(id))
        {
            Debug.Log(id);
            _id = id;
            _model = _getData(id);
            Image.sprite = _model.Icon;
            Text.text = _model.Describe;
            SetPos();
        }
    }
    private bool JudgeIdValid(int id)
    {
        return !_getData(id).Equals(new LoopListItemModel());
    }

    private void SetPos()
    {
        Rect.anchoredPosition = new Vector2(0, -(Rect.rect.height * _id + _offset));
    }
}
