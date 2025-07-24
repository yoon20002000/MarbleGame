using UnityEngine;

public class ColorUtil
{
    public static Color RandomColor()
    {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
    public static Color ColorFromAnyString(string input)
    {
        // 1. 문자열을 안정적인 해시로 변환 (예: MD5)
        int hash = CommonUtil.StableHash(input);

        // 2. Hue를 0~1 사이로 변환 (0~360도 범위를 0~1로 정규화)
        float hue = (hash % 360) / 360f;

        // 3. Saturation, Lightness 고정해서 보기 좋게 설정
        float saturation = 0.6f;
        float lightness = 0.5f;

        // 4. HSL to RGB 변환
        return ColorFromHSL(hue, saturation, lightness);
    }
    private static Color ColorFromHSL(float h, float s, float l)
    {
        float r, g, b;

        if (s == 0f)
        {
            r = g = b = l; // 회색조
        }
        else
        {
            float q = l < 0.5f ? l * (1 + s) : (l + s) - (l * s);
            float p = 2 * l - q;

            r = HueToRGB(p, q, h + 1f / 3f);
            g = HueToRGB(p, q, h);
            b = HueToRGB(p, q, h - 1f / 3f);
        }

        return new Color(r, g, b, 1f);
    }

    private static float HueToRGB(float p, float q, float t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1f / 6f) return p + (q - p) * 6f * t;
        if (t < 1f / 2f) return q;
        if (t < 2f / 3f) return p + (q - p) * (2f / 3f - t) * 6f;
        return p;
    }
}
