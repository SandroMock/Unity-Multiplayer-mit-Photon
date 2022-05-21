using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Health healthBarOne = null;
    [SerializeField] Health healthBarTwo = null;
    GameObject PlayerOne = null;
    GameObject PlayerTwo = null;

    // Start is called before the first frame update
    void Start()
    {

        switch (PhotonNetwork.CurrentRoom.PlayerCount)
        {
            case 1:
                PlayerOne = PhotonNetwork.Instantiate("Player 1",
                    GameManager.GM.spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position, Quaternion.identity);
                PlayerOne.GetComponent<PlayerController>().health = healthBarOne;
                break;
            case 2:
                PlayerTwo = PhotonNetwork.Instantiate("Player 2",
                    GameManager.GM.spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position, Quaternion.identity);
                PlayerTwo.GetComponent<PlayerController>().health = healthBarTwo;
                break;
            default:
                Debug.Log("Spawn fehlgeschlagen");
                break;
        }
    }
}
