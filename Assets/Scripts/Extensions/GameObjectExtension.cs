using UnityEngine;

public static class GameObjectExtension
{
    public static T GetOrAddComponent<T>(this GameObject gameObject, out T component) where T : Component
    {
        component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    public static T GetComponentChildrenOrParentOrAdd<T>(this GameObject gameObject, out T component) where T : Component
    {
        component = gameObject.GetComponentInChildren<T>();
        if (component == null)
        {
            component = gameObject.GetComponentInParent<T>();
        }
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
}
