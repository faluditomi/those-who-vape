using UnityEngine;

public class VapeController : MonoBehaviour
{
    public enum VapeType
    {
        MixedBerry,
        LushIce,
        Heisenberg
    }

    public VapeType vapeType;

    public VapeType GetVapeType()
    {
        return vapeType;
    }
}
