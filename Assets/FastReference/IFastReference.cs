using System.Reflection;
using UnityEngine;

public interface IFastReference
{
    void SetDirty(MonoBehaviour mb, FieldInfo fi);
}
