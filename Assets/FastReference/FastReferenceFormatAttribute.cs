using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FastReferenceFormatAttribute : Attribute, IFastReference
{
    public string Format;

    public void SetDirty(MonoBehaviour mb, FieldInfo fi, BindingFlags bf)
    {
        try
        {
            var ts = new List<Transform>();
            CollectTransforms(mb.transform, ref ts);
            var elemType = fi.FieldType.GetElementType();
            if (elemType == typeof(Transform))
            {
                fi.SetValue(mb, ts.ToArray());
            }
            else if (elemType == typeof(GameObject))
            {
                fi.SetValue(mb, ts.Select(x => x.gameObject).ToArray());
            }
            else
            {
                // fi.SetValue(mb, ts.Select(x => x.GetComponent(elemType)).ToArray()); // NOTE: cast error.
                var dst = Array.CreateInstance(elemType, ts.Count);
                Array.Copy(ts.OrderBy(x => x.name).Select(x => x.GetComponent(elemType)).ToArray(), dst, ts.Count);
                fi.SetValue(mb, dst);
            }
        }
        catch (Exception exp)
        {
            Debug.LogError(exp);
        }
    }

    void CollectTransforms(Transform parent, ref List<Transform> ts)
    {
        foreach (Transform t in parent)
        {
            if (Regex.IsMatch(t.name, Format))
            {
                ts.Add(t);
            }
            CollectTransforms(t, ref ts);
        }
    }

    public FastReferenceFormatAttribute(string format)
    {
        this.Format = format;
    }
}
