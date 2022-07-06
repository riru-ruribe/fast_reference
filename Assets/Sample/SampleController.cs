using UnityEngine;
using UnityEngine.UI;

public class SampleController : MonoBehaviour
{
    [SerializeField, FastReferenceMyself] Transform t;
    [SerializeField, FastReference] Image image;
    [SerializeField, FastReference("Btn")] Button button;
    [SerializeField, FastReferenceFormat("Container_[0-9]")] SampleContainer[] containers;
}
