using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isGameOver = false;
    private EnemySpawnManager enemySpawnManager;
    private InventoryManager inventoryManager;
    private UIController uiController;
    private SlotMachineController slotMachineController;

    private void Awake()
    {
        enemySpawnManager = GameObject.FindAnyObjectByType<EnemySpawnManager>();
        inventoryManager = GameObject.FindAnyObjectByType<InventoryManager>();
        uiController = GameObject.FindAnyObjectByType<UIController>();
        slotMachineController = GameObject.FindAnyObjectByType<SlotMachineController>();
    }

    public void GameOver() 
    {
        isGameOver = true;

        enemySpawnManager.StopSpawning();

        uiController.SetPauseState(true);
    }

    public void Restart()
    {
        isGameOver = false;

        enemySpawnManager.Restart();

        inventoryManager.Restart();

        uiController.Restart();

        uiController.SetPauseState(false);

        slotMachineController.Reset();

        foreach(GameObject vapePoint in GameObject.FindGameObjectsWithTag("VapeZone"))
        {
            vapePoint.GetComponent<VapePlacementPointController>().RemoveVape();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
