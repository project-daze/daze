using UnityEngine;

namespace Daze.Player.Support
{
    public class Utils
    {
        public static float LinearMap(
            float value,
            float inMin,
            float inMax,
            float outMin,
            float outMax
        )
        {
            // Clamp the input value to the input range.
            value = Mathf.Clamp(value, inMin, inMax);

            // Map the input value to the output range using the linear
            // mapping formula.
            float mappedValue = outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);

            return mappedValue;
        }
    }
}
