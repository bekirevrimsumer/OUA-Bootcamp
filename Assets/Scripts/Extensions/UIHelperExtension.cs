using UnityEngine;

public static class UIHelperExtension
{
    public static GameObject FindChildObject(this GameObject parent, string childName)
    {
        var ts = parent.GetComponentsInChildren<Transform>(true);
        foreach (var t in ts)
        {
            if (t.name == childName)
            {
                return t.gameObject;
            }
        }

        return null;
    }

    public static T FindChildObject<T>(this GameObject parent, string childName) where T : Component
    {
        var ts = parent.GetComponentsInChildren<Transform>(true);
        foreach (var t in ts)
        {
            if (t.name == childName)
            {
                return t.GetComponent<T>();
            }
        }

        return null;
    }
}