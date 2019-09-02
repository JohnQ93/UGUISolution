using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RotationDiagram2D : MonoBehaviour
{
    public Vector2 ItemSize;
    public Sprite[] ItemSprites;
    public float Offset;
    public float ScaleTimesMin;
    public float ScaleTimesMax;

    private List<RotationDiagramItem> _items;
    private List<ItemPosData> _posData;
    void Start()
    {
        _items = new List<RotationDiagramItem>();
        _posData = new List<ItemPosData>();
        CreateItem();
        CalculateData();
        SetItemData();
    }

    private GameObject CreateTemplate()
    {
        GameObject item = new GameObject("Template");
        item.AddComponent<RectTransform>().sizeDelta = ItemSize;
        item.AddComponent<Image>();
        item.AddComponent<RotationDiagramItem>();
        return item;
    }

    private void CreateItem()
    {
        GameObject template = CreateTemplate();
        RotationDiagramItem itemTemp = null;
        foreach (Sprite sprite in ItemSprites)
        {
            itemTemp = Instantiate(template).GetComponent<RotationDiagramItem>();
            //子项要做的事情放到子类脚本中去处理
            itemTemp.SetParent(transform);
            itemTemp.SetSprite(sprite);
            itemTemp.AddMovingListener(Change);
            _items.Add(itemTemp);
        }
        Destroy(template);
    }

    public void Change(float offsetX)
    {
        int symbol = offsetX > 0 ? 1 : -1;
        Change(symbol);
    }

    public void Change(int symbol)
    {
        foreach (RotationDiagramItem item in _items)
        {
            item.ChangePosId(symbol, _items.Count);
        }

        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].SetPosData(_posData[_items[i].PosId]);

        }
    }

    private void CalculateData()
    {
        List<ItemData> itemDatas = new List<ItemData>();

        //椭圆的周长计算
        float length = (ItemSize.x + Offset) * _items.Count;
        //每个sprite之间的比例差值
        float ratioOffset = 1 / (float)_items.Count;

        for (int i = 0; i < _items.Count; i++)
        {
            ItemData itemData = new ItemData();
            itemData.PosId = i;
            itemDatas.Add(itemData);

            _items[i].PosId = i;

            ItemPosData data = new ItemPosData();
            data.X = GetX(ratioOffset * i, length);
            data.ScaleTimes = GetScaleTimes(ratioOffset * i, ScaleTimesMax, ScaleTimesMin);
            _posData.Add(data);
        }

        itemDatas = itemDatas.OrderBy(item => _posData[item.PosId].ScaleTimes).ToList();

        for (int i = 0; i < itemDatas.Count; i++)
        {
            itemDatas[i].OrderId = i + 1;
            _posData[itemDatas[i].PosId].Order = i;
        }
    }

    private void SetItemData()
    {
        for (int i = 0; i < _posData.Count; i++)
        {
            _items[i].SetPosData(_posData[i]);
        }
    }

    private float GetX(float ratio, float length)
    {
        if(ratio < 0 || ratio > 1)
        {
            Debug.LogError("比例必须是在0-1之间的值");
            return 0;
        }
        if(ratio >= 0 && ratio < 0.25f)
        {
            return length * ratio;
        }
        else if(ratio >= 0.25f && ratio < 0.75f)
        {
            return length * (0.5f - ratio);
        }
        else
        {
            return length * (ratio - 1);
        }
    }

    private float GetScaleTimes(float ratio, float max, float min)
    {
        if(ratio < 0 || ratio > 1)
        {
            Debug.LogError("比例必须是在0-1之间的值");
            return 0;
        }

        float factor = (max - min) / 0.5f;
        if(ratio < 0.5f)
        {
            return max - factor * ratio;
        }
        else
        {
            return max - factor * (1 - ratio);
        }
    }
}
public class ItemPosData
{
    public float X;
    public float ScaleTimes;
    public int Order;
}

public class ItemData
{
    public int PosId;
    public int OrderId;
}