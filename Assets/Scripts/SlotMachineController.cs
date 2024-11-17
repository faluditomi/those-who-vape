using System.Collections;
using System.Collections.Generic;
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
    private AudioManager audioManager;
    private Coroutine rollCoroutine;
    private Coroutine imageRandomiserCoroutine;
    private int currentChanceForMixedBerry;
    private int currentChanceForLushIce;
    private int currentChanceForHeisenberg;
    public int initialChanceForMixedBerry = 75;
    public int initialChanceForLushIce = 25;
    public int initialChanceForHeisenberg = 5;
    public int maxChanceForMixedBerry = 90;
    public int maxChanceForLushIce = 75;
    public int maxChanceForHeisenberg = 50;
    public int incrementForBetterChance = 1;
    public float rollDuration = 2;
    public Sprite mixedBerryImage;
    public Sprite lushIceImage;
    public Sprite hisenbergImage;
    public Sprite xImage;
    private Sprite[] spritePool;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        Transform slotMachineUI = GameObject.Find("Slot Machine UI").transform;
        leftImage = slotMachineUI.Find("Left Image").GetComponent<Image>();
        middleImage = slotMachineUI.Find("Middle Image").GetComponent<Image>();
        rightImage = slotMachineUI.Find("Right Image").GetComponent<Image>();
        prizeText = slotMachineUI.Find("Prize Text").GetComponent<TextMeshProUGUI>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    private void Start()
    {
        currentChanceForMixedBerry = initialChanceForMixedBerry;
        currentChanceForLushIce = initialChanceForLushIce;
        currentChanceForHeisenberg = initialChanceForHeisenberg;
        
        Reset();

        spritePool = new Sprite[] { mixedBerryImage, lushIceImage, hisenbergImage, xImage };
    }

    public void Reset()
    {
        List<Image> images = new List<Image> { leftImage, middleImage, rightImage };
        foreach (var image in images)
        {
            image.sprite = xImage;
        }
        currentPrize = null;
        prizeText.SetText("WIN BIG NOW");
        audioManager.Stop("sltmchn_rolling");
        audioManager.Stop("sltmchn_rolling_also");
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
        
        audioManager.Play("sltmchn_lever_pull_1");

        imageRandomiserCoroutine = StartCoroutine(RandomizeUIImagesCoroutine());
        rollCoroutine = StartCoroutine(RollBehaviour());
    }

    public void Double()
    {
        if(rollCoroutine != null || currentPrize == null
        || bustCoroutine != null)
        {
            return;
        }
        
        audioManager.Play("sltmchn_lever_pull_1");

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
        List<Image> images = new List<Image> { leftImage, middleImage, rightImage };

        if(currentPrize == null)
        {
            foreach (var image in images)
            {
                image.sprite = xImage;
            }

            return;
        }

        Sprite targetSprite;

        switch (currentPrize.vapeType)
        {
            case VapeController.VapeType.MixedBerry:
                targetSprite = mixedBerryImage;
                break;
            case VapeController.VapeType.LushIce:
                targetSprite = lushIceImage;
                break;
            case VapeController.VapeType.Heisenberg:
                targetSprite = hisenbergImage;
                break;
            default:
                targetSprite = xImage;
                break;
        }

        if (currentPrize.quantity == 1)
        {
            // Assign two images to the target sprite and one to a random non-target sprite
            int nonTargetIndex = Random.Range(0, images.Count);
            for (int i = 0; i < images.Count; i++)
            {
                images[i].sprite = (i == nonTargetIndex) ? GetRandomNonTargetSprite(targetSprite) : targetSprite;
            }
        }
        else if (currentPrize.quantity >= 2)
        {
            // Assign all images to the target sprite
            foreach (var image in images)
            {
                image.sprite = targetSprite;
            }
        }

        prizeText.SetText("YOU WON " + currentPrize.quantity + " " + currentPrize.vapeType.ToString().ToUpper() + " VAPES");
    }

    private Sprite GetRandomNonTargetSprite(Sprite targetSprite)
    {
        // Get a random sprite that's not the target sprite
        List<Sprite> sprites = new List<Sprite> { mixedBerryImage, lushIceImage, hisenbergImage, xImage };
        sprites.Remove(targetSprite); // Exclude the target sprite
        return sprites[Random.Range(0, sprites.Count)];
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
        yield return new WaitForSecondsRealtime(0.3f);
    
        audioManager.Play("sltmchn_rolling");
        audioManager.Play("sltmchn_rolling_also");

        currentPrize = null;

        yield return new WaitForSecondsRealtime(rollDuration - 0.3f);

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
            audioManager.Play("sltmchn_success");
        }
        else
        {
            bustCoroutine = StartCoroutine(BustBehaviour());
        }

        audioManager.Stop("sltmchn_rolling");
        audioManager.Stop("sltmchn_rolling_also");

        UpdatePrizeTextUponWin();
        IncreaseChances();

        StopCoroutine(imageRandomiserCoroutine);

        rollCoroutine = null;
    }

    private bool RollForDouble()
    {
        int roll = Random.Range(0, 4);

        if(roll > 2)
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
        yield return new WaitForSecondsRealtime(0.3f);
    
        audioManager.Play("sltmchn_rolling");
        audioManager.Play("sltmchn_rolling_also");

        yield return new WaitForSecondsRealtime(rollDuration - 0.3f);

        if(RollForDouble())
        {
            currentPrize.Double();
            UpdatePrizeTextUponWin();
            audioManager.Play("sltmchn_success");
            rollCoroutine = null;
        }
        else
        {
            bustCoroutine = StartCoroutine(BustBehaviour());
        }

        audioManager.Stop("sltmchn_rolling");
        audioManager.Stop("sltmchn_rolling_also");

        rollCoroutine = null;
    }

    private IEnumerator BustBehaviour() 
    {
        audioManager.Play("sltmchn_fail");
        prizeText.SetText("BETTER LUCK NEXT TIME");
        yield return new WaitForSecondsRealtime(3);
        Reset();
        bustCoroutine = null;
    }

    private IEnumerator RandomizeUIImagesCoroutine()
    {
        while(true)
        {
            leftImage.sprite = spritePool[Random.Range(0, spritePool.Length)];
            middleImage.sprite = spritePool[Random.Range(0, spritePool.Length)];
            rightImage.sprite = spritePool[Random.Range(0, spritePool.Length)];

            yield return new WaitForSeconds(0.1f);
        }
    }
}
