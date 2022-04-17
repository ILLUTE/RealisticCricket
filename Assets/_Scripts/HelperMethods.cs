using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
    public static float GetInRange(this float currentValue, float oldMinValue, float oldMaxValue, float newMinValue, float newMaxValue)
    {
        float oldRange = oldMaxValue - oldMinValue;
        float newRange = newMaxValue - newMinValue;

        float newValue = (((currentValue - oldMinValue) * newRange) / oldRange) + newMinValue;
        currentValue = newValue;

        return currentValue;
    }
}
