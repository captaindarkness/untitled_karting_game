using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {

    public float speed = 5;

    void Start()
    {
        if (!networkView.isMine)
        {
            enabled = false;
        }
    }

    void Update() 
    {
        if (networkView.isMine) 
        {
            Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.Translate(speed * moveDir * Time.deltaTime);
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.position;
            stream.Serialize(ref pos);
        }
        else
        {
            Vector3 posRec = Vector3.zero;
            stream.Serialize(ref posRec);
            transform.position = posRec;
        }
    }
}
