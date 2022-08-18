using UnityEngine;

[System.Serializable]
public class HSVColor
{
    [Range(0, 360)] public int h; // hue
    [Range(0, 100)] public int s; // saturation
    [Range(0, 100)] public int v; // value

    public HSVColor(int h = 0, int s = 0, int v = 100)
    {
        this.h = h;
        this.s = s;
        this.v = v;
    }

    /// <summary>
    /// returns the color as a standard unity color
    /// </summary>
    public Color AsColor()
    {
        return Color.HSVToRGB((float)h / 360f, (float)s / 100f, (float)v / 100f);
    }

    /// <summary>
    /// returns a HSV color from a standard unity color
    /// </summary>
    public static HSVColor FromColor(Color color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        return new HSVColor(Mathf.RoundToInt(h * 360), Mathf.RoundToInt(s * 100), Mathf.RoundToInt(v * 100));
    }

    /// <summary>
    /// Linearly interpolates between two HSV Colors
    /// </summary>
    public static HSVColor Lerp(HSVColor color1, HSVColor color2, float t)
    {
        HSVColor newColor = new HSVColor();

        // get diffs
        float hDiff = color2.h - color1.h;
        float sDiff = color2.s - color1.s;
        float vDiff = color2.v - color1.v;

        // apply
        newColor.h = color1.h + Mathf.RoundToInt(hDiff * t);
        newColor.s = color1.s + Mathf.RoundToInt(sDiff * t);
        newColor.v = color1.v + Mathf.RoundToInt(vDiff * t);

        return newColor;
    }
}
