using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class CircleImage : Image
{
    /// <summary>
    /// 圆形由多少块三角形组成
    /// </summary>
    [SerializeField]
    private int segements = 100;
    /// <summary>
    /// 显示部分占圆形的百分比
    /// </summary>
    [SerializeField]
    private float showPercent = 1;

    private readonly Color32 GRAY_COLOR = new Color32(60, 60, 60, 255);

    private List<Vector3> _vertexList;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        _vertexList = new List<Vector3>();

        AddVertex(vh, segements);

        AddTriangle(vh, segements);
    }

    private void AddVertex(VertexHelper vh, int segements)
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        int realSegements = (int)(segements * showPercent);

        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
        float uvWidth = uv.z - uv.x;
        float uvHeight = uv.w - uv.y;
        Vector2 uvCenter = new Vector2(uvWidth * 0.5f, uvHeight * 0.5f);
        Vector2 convertRatio = new Vector2(uvWidth / width, uvHeight / height);

        //每一块三角面的对应弧度值
        float radian = (2 * Mathf.PI) / segements;
        float radius = width * 0.5f;

        Vector2 originPos = new Vector2((0.5f - rectTransform.pivot.x) * width, (0.5f - rectTransform.pivot.y) * height);

        //画出圆心点
        Color32 tempColor = GetOriginColor();
        UIVertex origin = GetUIVertex(tempColor, originPos, Vector2.zero, uvCenter, convertRatio);
        vh.AddVert(origin);

        //画出圆形边缘上的顶点
        int vertexCount = realSegements + 1;
        float curRadian = 0;
        for (int i = 0; i < segements + 1; i++)
        {
            float x = Mathf.Cos(curRadian) * radius;
            float y = Mathf.Sin(curRadian) * radius;
            curRadian += radian;

            if (i < vertexCount)
            {
                tempColor = color;
            }
            else
            {
                tempColor = GRAY_COLOR;
            }
            Vector2 uvPos = new Vector2(x, y);
            Vector2 pos = uvPos + originPos;
            UIVertex vertexTemp = GetUIVertex(tempColor, pos, uvPos, uvCenter, convertRatio);
            vh.AddVert(vertexTemp);
            _vertexList.Add(pos);
        }
    }

    private void AddTriangle(VertexHelper vh, int segements)
    {
        //画出三角形面片
        int id = 1;
        for (int i = 0; i < segements; i++)
        {
            vh.AddTriangle(id, 0, id + 1);
            id++;
        }
    }

    private UIVertex GetUIVertex(Color32 color, Vector3 pos, Vector2 uvPos, Vector2 uvCenter, Vector2 uvScale)
    {
        UIVertex vertex = new UIVertex();
        vertex.color = color;
        vertex.position = pos;
        vertex.uv0 = new Vector2(uvPos.x * uvScale.x + uvCenter.x, uvPos.y * uvScale.y + uvCenter.y);
        return vertex;
    }

    private Color32 GetOriginColor()
    {
        Color32 colorTemp = (Color.white - GRAY_COLOR) * showPercent;
        return new Color32(
            (byte)(GRAY_COLOR.r + colorTemp.r),
            (byte)(GRAY_COLOR.g + colorTemp.g),
            (byte)(GRAY_COLOR.b + colorTemp.b),
            255);
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint);
        return isValid(localPoint, _vertexList);
    }

    private bool isValid(Vector2 localPoint, List<Vector3> vertexList)
    {
        int num = GetCrossPointNum(localPoint, vertexList);
        return num % 2 == 1;  //奇数返回true， 偶数返回false
    }

    private int GetCrossPointNum(Vector2 localPoint, List<Vector3> vertexList)
    {
        int count = 0;
        Vector3 vert1 = Vector3.zero;
        Vector3 vert2 = Vector3.zero;

        int vertCount = vertexList.Count;
        for (int i = 0; i < vertCount; i++)
        {
            vert1 = vertexList[i];
            vert2 = vertexList[(i + 1) % vertCount];

            if(IsYAxisInRange(localPoint, vert1, vert2))
            {
                if(localPoint.x < GetXValue(localPoint.y, vert1, vert2))
                {
                    count++;
                }
            }
        }
        return count;
    }
    private bool IsYAxisInRange(Vector2 localPoint, Vector3 vert1, Vector3 vert2)
    {
        if(vert1.y > vert2.y)
        {
            return localPoint.y < vert1.y && localPoint.y > vert2.y;
        }
        else
        {
            return localPoint.y < vert2.y && localPoint.y > vert1.y;
        }
    }

    private float GetXValue(float y, Vector3 vert1, Vector3 vert2)
    {
        float k = (vert1.x - vert2.x) / (vert1.y - vert2.y);

        return vert1.x - k * (vert1.y - y);
    }
}
