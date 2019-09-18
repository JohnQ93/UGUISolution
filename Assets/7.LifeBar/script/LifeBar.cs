using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    private Transform _target;
    private Vector3 _offset;
    private List<LifeBarData> _data;
    private LifeBarItem _nextBar;
    private LifeBarItem _currentBar;
    private float _ratio;
    private int _currentIndex;
    public void init(Transform target, int lifeMax, List<LifeBarData> data)
    {
        _currentIndex = 0;
        _target = target;
        _offset = GetOffset(target);
        _data = data;
        _nextBar = transform.Find("NextBar").gameObject.AddComponent<LifeBarItem>();
        _currentBar = transform.Find("CurrentBar").gameObject.AddComponent<LifeBarItem>();
        _nextBar.Init();
        _currentBar.Init();

        RectTransform rect = GetComponent<RectTransform>();
        _ratio = rect.rect.width * data.Count / lifeMax; //每一滴血所代表的血条宽度

        SetBarData(_currentIndex, data);
    }

    private Vector3 GetOffset(Transform target)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer == null)
            return Vector3.zero;

        return Vector3.up * (renderer.bounds.max.y + 1f);
    }

    public void ChangeLife(float value)
    {
        float width = _currentBar.ChangeLife(value * _ratio);
        if (width < 0 && ChangeIndex(1))
        {
            Exchange();
            _currentBar.transform.SetAsLastSibling();
            _nextBar.ResetToWidth();
            SetBarData(_currentIndex, _data);
            ChangeLife(width / _ratio);
        }
        else if(width > 0 && ChangeIndex(-1))
        {
            Exchange();
            _currentBar.transform.SetAsLastSibling();
            _currentBar.ResetToZero();
            SetBarData(_currentIndex, _data);
            ChangeLife(width / _ratio);
        }
    }

    private void SetBarData(int index, List<LifeBarData> data)
    {
        if (index < 0 || index >= data.Count)
            return;

        _currentBar.SetData(data[index]);

        if (index + 1 >= data.Count)
        {
            _nextBar.SetData(new LifeBarData(null, color: Color.white));
        }
        else
        {
            _nextBar.SetData(data[index + 1]);
        }
    }

    /// <summary>
    /// 改变当前下标
    /// </summary>
    /// <param name="symbol">1代表扣血，-1代表加血</param>
    /// <returns></returns>
    private bool ChangeIndex(int symbol)
    {
        int index = _currentIndex + symbol;
        if(index >= 0 && index < _data.Count)
        {
            _currentIndex = index;
            return true;
        }

        return false;
    }

    private void Exchange()
    {
        var temp = _nextBar;
        _nextBar = _currentBar;
        _currentBar = temp;
    }

    private void Update()
    {
        if (_target == null)
            return;
        //设置血条显示位置
        transform.position = Camera.main.WorldToScreenPoint(_target.position + _offset);
    }
}

public struct LifeBarData
{
    public Sprite lifeBarSprite;
    public Color lifeBarColor;

    public LifeBarData(Sprite sprite, Color color)
    {
        lifeBarSprite = sprite;
        lifeBarColor = color;
    }
}
