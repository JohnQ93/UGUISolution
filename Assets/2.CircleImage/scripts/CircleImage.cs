using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class CircleImage : Image
{
    /// <summary>
    /// 圆形由多少块三角形组成
    /// </summary>
    private int segements;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
        float uvWidth = uv.z - uv.x;
        float uvHeight = uv.w - uv.y;
        Vector2 uvCenter = new Vector2(uvWidth * 0.5f, uvHeight * 0.5f);
        Vector2 convertRatio = new Vector2(uvWidth / width, uvHeight / height);
    }
}
