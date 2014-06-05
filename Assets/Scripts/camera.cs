using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour
{


    void Update()
    {
        if (networkView.isMine)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
