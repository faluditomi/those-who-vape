using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private bool isGameOver = false;
    private EnemySpawnManager1 enemySpawnManager1;
    private EnemySpawnManager2 enemySpawnManager2;
    private InventoryManager inventoryManager;
    private UIController uiController;
    private SlotMachineController slotMachineController;
    private AudioManager audioManager;
    private static InputActions inputActions;
    private bool isPaused = false;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        enemySpawnManager1 = GameObject.FindAnyObjectByType<EnemySpawnManager1>();
        enemySpawnManager2 = GameObject.FindAnyObjectByType<EnemySpawnManager2>();
        inventoryManager = GameObject.FindAnyObjectByType<InventoryManager>();
        uiController = GameObject.FindAnyObjectByType<UIController>();
        slotMachineController = GameObject.FindAnyObjectByType<SlotMachineController>();
        inputActions = new InputActions();
    }

    private void Start()
    {
        inputActions.Enable();
        inputActions.Gameplay.Restart.performed += RestartButton;
        inputActions.Gameplay.Pause.performed += Pause;
    }

    public void GameOver() 
    {
        isGameOver = true;
        enemySpawnManager1.StopSpawning();
        enemySpawnManager2.StopSpawning();
        uiController.SetPauseState(true);
    }

    private void RestartButton(InputAction.CallbackContext context)
    {
        Restart();
    }

    private void PausePause()
    {
        uiController.SetPauseState(!uiController.GetPauseState());
        isPaused = !isPaused;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        PausePause();
    }

    public void Restart()
    {
        if(isPaused)
        {
            PausePause();
            return;
        }

        audioManager.canPlayNewSounds = false;
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
        
        audioManager.canPlayNewSounds = true;
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
