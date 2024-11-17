using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isGameOver = false;
    private EnemySpawnManager1 enemySpawnManager1;
    private EnemySpawnManager2 enemySpawnManager2;
    private InventoryManager inventoryManager;
    private UIController uiController;
    private SlotMachineController slotMachineController;

    private void Awake()
    {
        enemySpawnManager1 = GameObject.FindAnyObjectByType<EnemySpawnManager1>();
        enemySpawnManager2 = GameObject.FindAnyObjectByType<EnemySpawnManager2>();
        inventoryManager = GameObject.FindAnyObjectByType<InventoryManager>();
        uiController = GameObject.FindAnyObjectByType<UIController>();
        slotMachineController = GameObject.FindAnyObjectByType<SlotMachineController>();
    }

    public void GameOver() 
    {
        isGameOver = true;
        enemySpawnManager1.StopSpawning();
        enemySpawnManager2.StopSpawning();
        uiController.SetPauseState(true);
    }

    public void Restart()
    {
        isGameOver = false;
        enemySpawnManager1.Restart();
        enemySpawnManager2.Restart();
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
