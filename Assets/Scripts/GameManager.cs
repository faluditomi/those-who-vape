using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isGameOver = false;

    public void GameOver() 
    {
        isGameOver = true;
    }

    public void Restart()
    {
        isGameOver = false;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
