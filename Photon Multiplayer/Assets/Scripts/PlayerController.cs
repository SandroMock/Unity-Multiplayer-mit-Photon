using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField] float speed = 0;
    Vector2 movement = Vector2.zero;
    Vector3 mousePos = Vector3.zero;
    float direction = 0.0f;
    PhotonView PV;
    int maxHealth = 10;
    public int currentHealth = 0;

    public Health health = null;


    [SerializeField] float fireRate = 0.0f;
    float canFire = 0.0f;

    AudioSource playerAudioSource = null;
    [SerializeField] AudioClip laserAudio = null;
    [SerializeField] Transform spawnLaser = null;
    GameObject laser = null;





    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        playerAudioSource = GetComponent<AudioSource>();
        playerAudioSource.clip = laserAudio;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (Input.GetKey(KeyCode.Mouse0) && Time.time > canFire)
        {
            FireLaser();
        }
    }

    public void Movement()
    {
        if (!PV.IsMine)
        {
            return;
        }
        else
        {
            movement = transform.position;

            if (Input.GetKey(KeyCode.W))
            {
                movement.y += speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement.y -= speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                movement.x -= speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement.x += speed * Time.deltaTime;
            }


            transform.position = movement;

            mousePos = Input.mousePosition -
                Camera.main.WorldToScreenPoint(transform.position);
            direction = Mathf.Atan2(-mousePos.x, mousePos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(direction, Vector3.forward);

            if (transform.position.x <= -8.3f)
            {
                transform.position = new Vector2(-8.3f, transform.position.y);
            }
            else if (transform.position.x >= 8.3f)
            {
                transform.position = new Vector2(8.3f, transform.position.y);
            }

            if (transform.position.y >= 4.95f)
            {
                transform.position = new Vector2(transform.position.x, 4.95f);
            }
            else if (transform.position.y <= -3.5f)
            {
                transform.position = new Vector2(transform.position.x, -3.5f);
            }
        }
    }
    public void FireLaser()
    {
        if (!PV.IsMine)
        {
            return;
        }
        else
        {
            canFire = Time.time + fireRate;
            laser = PhotonNetwork.Instantiate("Laser", spawnLaser.position,
            transform.rotation);
            laser.GetComponent<Laser>().PVPlayer = PV;
            playerAudioSource.Play();
        }
    }

    public void OnDamage(int _damage, int _actorNr)
    {
        if (!(PV.OwnerActorNr != _actorNr))
        {
            Debug.Log("Trifft nicht!");
            return;
        }
        else
        {
            currentHealth -= _damage;
            Debug.Log("OnDamage Hit");
            PV.RPC("ShowHealth", RpcTarget.All, currentHealth, PV.OwnerActorNr);
            if (currentHealth <= 0)
            {
                PV.RPC("FinishedGame", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void ShowHealth(int _health, int _actorNr)
    {
        if (PV.OwnerActorNr == _actorNr)
        {
            currentHealth = _health;
            //?health.SetHealth(_health);
        }
    }

    [PunRPC]
    public void FinishedGame()
    {
            if (currentHealth <= 0 && PV.IsMine)
            {
                GameManager.GM.GameEnd(false);
            }
            else
            {
                GameManager.GM.GameEnd(true);
            }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PV.RPC("FinishedGame", RpcTarget.All);
    }


}
