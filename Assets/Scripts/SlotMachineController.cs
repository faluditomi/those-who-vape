using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineController : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private Prize currentPrize;
    private Image leftImage;
    private Image middleImage;
    private Image rightImage;
    private TextMeshProUGUI prizeText;
    private Coroutine bustCoroutine;

    private Coroutine rollCoroutine;
    private int currentChanceForMixedBerry;
    private int currentChanceForLushIce;
    private int currentChanceForHeisenberg;
    public int initialChanceForMixedBerry = 75;
    public int initialChanceForLushIce = 25;
    public int initialChanceForHeisenberg = 5;
    public int maxChanceForMixedBerry = 90;
    public int maxChanceForLushIce = 75;
    public int maxChanceForHeisenberg = 50;
    public int incrementForBetterChance = 2;
    public float rollDuration = 3;

    private void Awake()
    {
        Transform slotMachineUI = GameObject.Find("Slot Machine UI").transform;
        leftImage = slotMachineUI.Find("Left Image").GetComponent<Image>();
        middleImage = slotMachineUI.Find("Middle Image").GetComponent<Image>();
        rightImage = slotMachineUI.Find("Right Image").GetComponent<Image>();
        prizeText = slotMachineUI.Find("Prize Text").GetComponent<TextMeshProUGUI>();
        inventoryManager = GameObject.FindAnyObjectByType<InventoryManager>();
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        currentPrize = null;
        currentChanceForMixedBerry = initialChanceForMixedBerry;
        currentChanceForLushIce = initialChanceForLushIce;
        currentChanceForHeisenberg = initialChanceForHeisenberg;
        prizeText.SetText("WIN BIG NOW");
    }

    public void Play()
    {
        if(rollCoroutine != null)
        {
            return;
        }

        if(bustCoroutine != null)
        {
            StopCoroutine(bustCoroutine);
            bustCoroutine = null;
            Reset();
        }

        rollCoroutine = StartCoroutine(RollBehaviour());
    }

    public void Double()
    {
        if(rollCoroutine != null || currentPrize == null
        || bustCoroutine != null)
        {
            return;
        }

        rollCoroutine = StartCoroutine(DoubleBehaviour());
    }

    public void CashOut()
    {
        if(rollCoroutine != null || currentPrize == null
        || bustCoroutine != null)
        {
            return;
        }

        inventoryManager.ManipulateInventory(currentPrize.vapeType, currentPrize.quantity);
        Reset();
    }

    private void UpdatePrizeTextUponWin()
    {
        if(currentPrize == null)
        {
            return;
        }

        prizeText.SetText("YOU WON " + currentPrize.quantity + " " + currentPrize.vapeType.ToString().ToUpper() + " VAPES");
    }

    private void IncreaseChances()
    {
        currentChanceForMixedBerry += incrementForBetterChance;
        Mathf.Clamp(currentChanceForMixedBerry, 0, maxChanceForMixedBerry);
        currentChanceForLushIce += incrementForBetterChance;
        Mathf.Clamp(currentChanceForLushIce, 0, maxChanceForLushIce);
        currentChanceForHeisenberg += incrementForBetterChance;
        Mathf.Clamp(currentChanceForHeisenberg, 0, maxChanceForHeisenberg);
    }

    private IEnumerator RollBehaviour()
    {
        currentPrize = null;

        yield return new WaitForSecondsRealtime(rollDuration);

        int roll = Random.Range(1, 100);
        
        if(roll < currentChanceForHeisenberg)
        {
            currentPrize = new Prize(VapeController.VapeType.Heisenberg, 1);
        }
        else if(roll < currentChanceForLushIce)
        {
            currentPrize = new Prize(VapeController.VapeType.LushIce, 1);
        }
        else if(roll < currentChanceForMixedBerry)
        {
            currentPrize = new Prize(VapeController.VapeType.MixedBerry, 1);
        }

        if(currentPrize != null)
        {
            if(RollForDouble())
            {
                currentPrize.Double();
            }
        }
        else
        {
            bustCoroutine = StartCoroutine(BustBehaviour());
        }

        UpdatePrizeTextUponWin();
        IncreaseChances();
        rollCoroutine = null;
    }

    private bool RollForDouble()
    {
        int roll = Random.Range(0, 2);

        if(roll > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator DoubleBehaviour()
    {
        yield return new WaitForSecondsRealtime(rollDuration);

        if(RollForDouble())
        {
            currentPrize.Double();
            UpdatePrizeTextUponWin();
            rollCoroutine = null;
        }
        else
        {
            bustCoroutine = StartCoroutine(BustBehaviour());
        }

        rollCoroutine = null;
    }

    private IEnumerator BustBehaviour() 
    {
        prizeText.SetText("BETTER LUCK NEXT TIME");
        yield return new WaitForSecondsRealtime(3);
        Reset();
        bustCoroutine = null;
    }
}