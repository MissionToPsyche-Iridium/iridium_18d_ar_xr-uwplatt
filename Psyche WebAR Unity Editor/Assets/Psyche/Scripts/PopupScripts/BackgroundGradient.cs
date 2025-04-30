using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Modifies background gradient to specified hues.
/// </summary>
[ExecuteAlways]
public class BackgroundGradient : BaseMeshEffect
{
    public int topR = 89, topG = 38, topB = 81, topA = 120; // purple
    public int bottomR = 48, bottomG = 33, bottomB = 68, bottomA = 120; // dark purple

    /// <summary>
    /// Converts RGBA integer values into unity color format
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    private Color RGBA(int r, int g, int b, int a)
    {
        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }

    /// <summary>
    /// Modify mesh to apply color gradient.
    /// </summary>
    /// <param name="vh"></param>
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        // Set color values, set vertex vales.
        UIVertex vertex = new UIVertex();
        int count = vh.currentVertCount;
        Color topColor = RGBA(topR, topG, topB, topA);
        Color bottomColor = RGBA(bottomR, bottomG, bottomB, bottomA);

        // Iterate through each vertex to apply gradient.
        for (int i = 0; i < count; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);
            float lerpValue = Mathf.InverseLerp(-1f, 1f, vertex.position.y);
            vertex.color = Color.Lerp(bottomColor, topColor, lerpValue);
            vh.SetUIVertex(vertex, i);
        }
    }
}
