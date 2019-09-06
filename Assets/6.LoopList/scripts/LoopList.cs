﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopList : MonoBehaviour
{
    public float _offsetY;
    private float _itemHeight;
    private RectTransform _content;

    private List<LoopListItem> _items;
    private List<LoopListItemModel> _models;
    void Start()
    {
        _items = new List<LoopListItem>();
        _models = new List<LoopListItemModel>();
        //获取列表数据
        GetModel();

        _content = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        GameObject item = Resources.Load<GameObject>("LoopListItem");
        _itemHeight = item.GetComponent<RectTransform>().rect.height;
        int num = GetShowItemNum(_itemHeight, _offsetY);
        SpawnIten(num, item);
        SetContentSize();
    }

    private int GetShowItemNum(float itemHeight, float offset)
    {
        float height = GetComponent<RectTransform>().rect.height;

        return Mathf.CeilToInt(height / (itemHeight + offset)) + 1;  //CeilToInt大于等于当前值的整数，额外加一需要多显示一个
    }

    private void SpawnIten(int num, GameObject itemPrefab)
    {
        GameObject temp = null;
        LoopListItem itemTemp = null;
        for (int i = 0; i < num; i++)
        {
            temp = Instantiate(itemPrefab, _content);
            itemTemp = temp.AddComponent<LoopListItem>();
            _items.Add(itemTemp);
        }
    }

    //从外部获取数据暂存
    private void GetModel()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Icon");
        foreach (Sprite icon in sprites)
        {
            _models.Add(new LoopListItemModel(icon, icon.name));
        }
    }

    //根据子项数量设置Content的高度
    private void SetContentSize()
    {
        float y = _models.Count * _itemHeight + (_models.Count - 1) * _offsetY;
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, y);
    }
}
