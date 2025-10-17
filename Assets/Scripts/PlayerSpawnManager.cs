using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private int m_playerCount;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = spawnPoints[m_playerCount].transform.position;
        if (m_playerCount == 0)
        {

        }
        m_playerCount++;
    }
}
