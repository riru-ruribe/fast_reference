using System;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class FastReferenceMyselfAttribute : Attribute, IFastReference
{
    public void SetDirty(MonoBehaviour mb, FieldInfo fi, BindingFlags bf)
    {
        try
        {
            if (fi.FieldType == typeof(Transform))
            {
                fi.SetValue(mb, mb.transform);
            }
            else if (fi.FieldType == typeof(GameObject))
            {
                fi.SetValue(mb, mb.gameObject);
            }
            else
            {
                fi.SetValue(mb, mb.GetComponent(fi.FieldType));
            }
        }
        catch (Exception exp)
        {
            Debug.LogError(exp);
        }
    }
}
