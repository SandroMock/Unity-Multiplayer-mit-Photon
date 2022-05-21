using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject connectBtn = null;
    [SerializeField] GameObject loadingBtn = null;

    [SerializeField] GameObject connectionLostPanel = null;
    [SerializeField] TextMeshProUGUI connectionLostText = null;

    public override void OnConnectedToMaster()
    {
        loadingBtn.SetActive(false);
        connectBtn.SetActive(true);
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Connected to Master");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("Join Failed");
        CreateOwnRoom();
    }
    public void ConnectBtn()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    void CreateOwnRoom()
    {
        RoomOptions roomOps = new RoomOptions();
        roomOps.MaxPlayers = 2;
        PhotonNetwork.CreateRoom("Battle", roomOps);
        Debug.Log("Eigener Raum wird erstellt");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateOwnRoom();
        Debug.LogError("Fail");
        PhotonNetwork.Disconnect();
    }
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("Spieler wird erstellt");
    }
    public void ExitBtn()
    {
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.SendRate = 20;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause _cause)
    {
        connectionLostPanel.SetActive(true);
        connectionLostText.text = "Verbindung fehlgeschlagen\nGrund: " + _cause;
    }

}
