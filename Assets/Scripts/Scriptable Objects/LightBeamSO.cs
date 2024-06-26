using UnityEngine;

[CreateAssetMenu(fileName = "LightBeam", menuName = "Light Reflections/Light Beam")]
public class LightBeamSO : ScriptableObject
{
    public LightBeamColorType LightBeamColorType;
    public Color LightBeamColor;
    public int ReflectionLimit;
}

public enum LightBeamColorType
{
    Red,
    Green,
    Blue,
    Yellow,
    Cyan,
    Magenta
}
