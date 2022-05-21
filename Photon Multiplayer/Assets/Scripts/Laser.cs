using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float speed = 0;
    public PhotonView PVPlayer = null;
    PlayerController player = null;

    private void Start()
    {
    }
    void Update()
    {
        transform.Translate(new Vector2(0, 1f) * speed * Time.deltaTime);
        DestroyLaser();
    }
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Player")
        {
            player = _other.GetComponent<PlayerController>();
            player.OnDamage(1, PVPlayer.OwnerActorNr);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    void DestroyLaser()
    {
        if (transform.position.x <= -15.0f)
        {
            transform.position = new Vector2(-15.0f, transform.position.y);
            Destroy(this.gameObject);
        }
        else if (transform.position.x >= 15.0f)
        {
            transform.position = new Vector2(15.0f, transform.position.y);
            Destroy(this.gameObject);
        }

        if (transform.position.y >= 10.0f)
        {
            transform.position = new Vector2(transform.position.x, 10.0f);
            Destroy(this.gameObject);
        }
        else if (transform.position.y <= -10.0f)
        {
            transform.position = new Vector2(transform.position.x, -10.0f);
            Destroy(this.gameObject);
        }
    }
}
