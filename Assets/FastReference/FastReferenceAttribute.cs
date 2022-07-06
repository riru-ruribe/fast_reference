using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FastReferenceAttribute : Attribute, IFastReference
{
    public string Key;

    public void SetDirty(MonoBehaviour mb, FieldInfo fi, BindingFlags bf)
    {
        var fieldName = Key ?? fi.Name;
        try
        {
            var ts = new List<Transform>();
            CollectTransforms(mb.transform, fieldName, ref ts);
            if (ts.Count > 1)
            {
                Debug.LogWarning($"{fieldName}: duplicate found.");
            }
            var t = ts[0];
            if (fi.FieldType == typeof(Transform))
            {
                fi.SetValue(mb, t);
            }
            else if (fi.FieldType == typeof(GameObject))
            {
                fi.SetValue(mb, t.gameObject);
            }
            else
            {
                fi.SetValue(mb, t.GetComponent(fi.FieldType));
            }
        }
        catch (Exception exp)
        {
            Debug.LogError(exp);
        }
    }

    void CollectTransforms(Transform parent, string name, ref List<Transform> ts)
    {
        foreach (Transform t in parent)
        {
            if (t.name.Equals(name))
            {
                ts.Add(t);
            }
            CollectTransforms(t, name, ref ts);
        }
    }

    public FastReferenceAttribute()
    {
    }

    public FastReferenceAttribute(string key)
    {
        this.Key = key;
    }
}
