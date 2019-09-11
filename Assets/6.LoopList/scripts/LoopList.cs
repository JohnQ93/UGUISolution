using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        //获取content节点
        _content = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        //获取最多可显示的子项数量，并实例化
        GameObject item = Resources.Load<GameObject>("LoopListItem");
        _itemHeight = item.GetComponent<RectTransform>().rect.height;
        int num = GetShowItemNum(_itemHeight, _offsetY);
        SpawnItem(num, item);

        SetContentSize();

        transform.GetComponent<ScrollRect>().onValueChanged.AddListener(ValueChanged);
    }

    private void ValueChanged(Vector2 data)
    {
        foreach (LoopListItem item in _items)
        {
            item.OnValueChange();
        }
    }

    private int GetShowItemNum(float itemHeight, float offset)
    {
        float height = GetComponent<RectTransform>().rect.height;

        return Mathf.CeilToInt(height / (itemHeight + offset)) + 1;  //CeilToInt大于等于当前值的整数，额外加一需要多显示一个
    }

    private void SpawnItem(int num, GameObject itemPrefab)
    {
        GameObject temp = null;
        LoopListItem itemTemp = null;
        for (int i = 0; i < num; i++)
        {
            temp = Instantiate(itemPrefab, _content);
            itemTemp = temp.AddComponent<LoopListItem>();
            itemTemp.AddGetDataListener((index) => _models[index]);
            itemTemp.init(i, _offsetY, num);
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
