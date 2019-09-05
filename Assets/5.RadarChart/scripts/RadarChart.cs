using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarChart : Image
{
    [SerializeField]
    private int _pointCount;
    [SerializeField]
    private List<RectTransform> _points;
    [SerializeField]
    private Vector2 _pointSize = new Vector2(10, 10);
    [SerializeField]
    private Sprite _pointSprite;
    [SerializeField]
    private Color _pointColor = Color.white;
    [SerializeField]
    private float[] _handlerRatio;
    [SerializeField]
    private List<RadarChartHandler> _handlers;

    private void Update()
    {
        SetVerticesDirty();    
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        AddVerts(vh);
        AddTriangle(vh);
    }

    private void AddVerts(VertexHelper vh)
    {
        foreach (RadarChartHandler handler in _handlers)
        {
            vh.AddVert(handler.transform.localPosition, color, Vector2.zero);
        }
    }

    private void AddTriangle(VertexHelper vh)
    {
        for (int i = 1; i < _pointCount - 1; i++)
        {
            vh.AddTriangle(0, i + 1, i);
        }
    }

    public void InitPoints()
    {
        ClearPoints();
        _points = new List<RectTransform>();
        SpawnPoints();
        SetPointPos();
    }

    private void ClearPoints()
    {
        if (_points == null)
            return;

        foreach (RectTransform point in _points)
        {
            if(point != null)
            {
                DestroyImmediate(point.gameObject);
            }
        }
    }

    private void SpawnPoints()
    {
        for (int i = 0; i < _pointCount; i++)
        {
            GameObject point = new GameObject("Point" + i);
            point.transform.SetParent(transform);
            _points.Add(point.AddComponent<RectTransform>());
        }
        
    }

    private void SetPointPos()
    {
        float radian = 2 * Mathf.PI / _pointCount;
        float radius = 100;
        float currentRadian = Mathf.PI / 2;
        for (int i = 0; i < _pointCount; i++)
        {
            float x = Mathf.Cos(currentRadian) * radius;
            float y = Mathf.Sin(currentRadian) * radius;
            currentRadian += radian;
            _points[i].anchoredPosition = new Vector2(x, y);
        }
    }

    public void InitHandlers()
    {
        ClearHandler();
        _handlers = new List<RadarChartHandler>();
        SpawnHandlers();
        SetHandlerPos();
    }

    private void ClearHandler()
    {
        if (_handlers == null)
            return;

        foreach (RadarChartHandler handler in _handlers)
        {
            DestroyImmediate(handler.gameObject);
        }
    }

    private void SpawnHandlers()
    {
        RadarChartHandler handlerCom = null;
        for (int i = 0; i < _pointCount; i++)
        {
            GameObject handler = new GameObject("Handler" + i);
            handler.AddComponent<RectTransform>();
            handler.AddComponent<Image>();
            handlerCom = handler.AddComponent<RadarChartHandler>();
            handlerCom.SetParent(transform);
            handlerCom.ChangeSprite(_pointSprite);
            handlerCom.ChangeColor(_pointColor);
            handlerCom.SetSize(_pointSize);

            _handlers.Add(handlerCom);
        }
    }

    private void SetHandlerPos()

    {
        if(_handlerRatio == null || _handlerRatio.Length != _pointCount)
        {
            for (int i = 0; i < _pointCount; i++)
            {
                _handlers[i].SetPos(_points[i].anchoredPosition);
            }
        }
        else
        {
            for (int i = 0; i < _pointCount; i++)
            {
                _handlers[i].SetPos(_points[i].anchoredPosition * _handlerRatio[i]);
            }
        }
    }
}
