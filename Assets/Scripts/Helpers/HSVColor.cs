using UnityEngine;

[System.Serializable]
public class HSVColor
{
    [Range(0, 360)] public int h; // hue
    [Range(0, 100)] public int s; // saturation
    [Range(0, 100)] public int v; // value
    [Range(0, 100)] public int a; // alphA

    public HSVColor()
    {
        this.h = 0;
        this.s = 0;
        this.v = 0;
        this.a = 0;
    }

    public HSVColor(int h, int s, int v, int a = 100)
    {
        this.h = h;
        this.s = s;
        this.v = v;
        this.a = a;
    }

    /// <summary>
    /// returns the color as a standard unity color
    /// </summary>
    public Color AsColor()
    {
        Color asColor = Color.HSVToRGB((float)h / 360f, (float)s / 100f, (float)v / 100f);
        asColor.a = (float)a / 100f;
        return asColor;
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
