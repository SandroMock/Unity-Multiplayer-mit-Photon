using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager GM;
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] Health healthBarOne = null;
    [SerializeField] Health healthBarTwo = null;
    [SerializeField] GameObject endScreenCanvas = null;
    [SerializeField] TextMeshProUGUI endScreenText = null;
    PhotonView PV = null;
    float timer = 2.0f;
    bool shutDown = false;
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] AudioSource source = null;
    [SerializeField] AudioClip hover = null;
    [SerializeField] AudioClip click = null;

    private void Awake()
    {
        if(GM == null)
        {
            GM = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        if (shutDown)
        {
            timer -= Time.deltaTime;
            timerText.text = string.Format("Spiel schließt in: {0}", (int)timer);
            if (timer <= 0.0f)
            {
                shutDown = false;
                endScreenCanvas.SetActive(false);
                PV.RPC("LeaveGame", RpcTarget.All);
                SceneManager.LoadScene("Menu");
            }
        }
    }

    void UpdateHealthbar(int _health, int _healthbar)
    {
        if (_healthbar == 1)
        {
            healthBarOne.SetHealth(_health);
        }
        else
        {
            healthBarTwo.SetHealth(_health);
        }
    }

    public void ExitBtn()
    {
        Debug.Log("Button pressed");
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected");
        SceneManager.LoadScene("Menu");
    }

    public void GameEnd(bool _isWon)
    {
        if (_isWon)
        {
            endScreenText.text = "Du hast gewonnen!";
        }
        else
        {
            endScreenText.text = "Du hast verloren!";
        }
        endScreenCanvas.SetActive(true);
    }

    [PunRPC]
    public void LeaveGame()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        shutDown = true;
    }



    public void ButtonOnHover()
    {
        source.PlayOneShot(hover);
    }
    public void ButtonOnClick()
    {
        source.PlayOneShot(click);
    }
}
