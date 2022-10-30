using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler instance;

    [SerializeField]
    CinemachineBrain cinemachineBrain;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        cinemachineBrain = GetComponent<CinemachineBrain>();
    }


}
