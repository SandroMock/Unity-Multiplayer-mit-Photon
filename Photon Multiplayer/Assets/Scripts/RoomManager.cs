using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    public static RoomManager RM;
    [SerializeField] int sceneIndex;
    [SerializeField] int currentScene;
    [SerializeField] GameObject connectionLostPanel = null;

    [SerializeField] AudioSource source = null;
    [SerializeField] AudioClip hover = null;
    [SerializeField] AudioClip click = null;

    public override void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnLoadingScene;
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnLoadingScene;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Starte Spiel");
        StartGame();
    }

    void OnLoadingScene(Scene _scene, LoadSceneMode _loadSceneMode)
    {
        currentScene = _scene.buildIndex;
    }
    void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        else
        {
            PhotonNetwork.LoadLevel(sceneIndex);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void ButtonOnHover()
    {
        source.PlayOneShot(hover);
    }
    public void ButtonOnClick()
    {
        source.PlayOneShot(click);
    }

    public void TryReconnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        connectionLostPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
