using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPun, IPunObservable
{
    public Slider slider;

    public void OnPhotonSerializeView(PhotonStream _stream, PhotonMessageInfo _info)
    {
        if (_stream.IsWriting)
        {
            _stream.SendNext(slider.value);
        }
        else
        {
            slider.value = (float)_stream.ReceiveNext();
        }
    }

    public void SetHealth(int _health)
    {
        Debug.LogError("Set Health");
        slider.value = _health;
    }
}
