using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
public class UIRing : MaskableGraphic
{
    [SerializeField] private float thickness = 10f;
    [SerializeField] private float radius = 100f;
    [SerializeField] private int segments = 64;

    public float Radius { get => radius; set { radius = value; SetVerticesDirty(); } }
    public float Thickness { get => thickness; set { thickness = value; SetVerticesDirty(); } }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float outerRadius = radius;
        float innerRadius = radius - thickness;
        float angleDelta = 2 * Mathf.PI / segments;

        Vector2 center = Vector2.zero;

        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleDelta;
            float angle2 = (i + 1) * angleDelta;

            Vector2 outer1 = center + new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * outerRadius;
            Vector2 outer2 = center + new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2)) * outerRadius;
            Vector2 inner1 = center + new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * innerRadius;
            Vector2 inner2 = center + new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2)) * innerRadius;

            int index = vh.currentVertCount;

            vh.AddVert(outer1, color, Vector2.zero);
            vh.AddVert(outer2, color, Vector2.zero);
            vh.AddVert(inner2, color, Vector2.zero);
            vh.AddVert(inner1, color, Vector2.zero);

            vh.AddTriangle(index, index + 1, index + 2);
            vh.AddTriangle(index + 2, index + 3, index);
        }
    }
}
