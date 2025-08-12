using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utility
{
    public static void SetUIImageAlpha(this Image img, float alpha) 
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }

}
