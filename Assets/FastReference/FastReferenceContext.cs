#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

class FastReferenceContext
{
    const BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    [MenuItem("CONTEXT/MonoBehaviour/FastReference")]
    static void Run(MenuCommand menuCommand)
    {
        var mb = menuCommand.context as MonoBehaviour;
        foreach (var fi in mb.GetType().GetFields(bf))
        {
            foreach (var fr in fi.GetCustomAttributes(false).OfType<IFastReference>())
            {
                fr?.SetDirty(mb, fi, bf);
            }
        }
        EditorUtility.SetDirty(mb);
    }
}
#endif
