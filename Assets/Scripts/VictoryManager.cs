using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static int enemiesAlive = 0;

    void Update()
    {
        Debug.Log("Enemies left: " + enemiesAlive); // для отладки

        if (enemiesAlive <= 0)
        {
            Debug.Log("Победа! Загружаем сцену победы...");
            SceneManager.LoadScene("VictoryScene"); // убедись в названии сцены
        }
    }
}