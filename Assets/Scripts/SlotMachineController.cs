using System;
using UnityEngine;

public class SlotMachineController : MonoBehaviour
{
    public float initialChanceForMixedBerry = 75;
    public float initialChanceForLushIce = 25;
    public float initialChanceForHeisenberg = 5;
    private float currentChanceForMixedBerry;
    private float currentChanceForLushIce;
    private float currentChanceForHeisenberg;

    public void Reset()
    {
        currentChanceForMixedBerry = initialChanceForMixedBerry;
        currentChanceForLushIce = initialChanceForLushIce;
        currentChanceForHeisenberg = initialChanceForHeisenberg;
    }

    public void Play()
    {

    }
}
