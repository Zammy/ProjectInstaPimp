using System;
using UnityEngine;

public class ViewController : MonoBehaviour, IViewController
{
    public GameObject GameObject
    {
        get
        {
            return gameObject;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            return _rigidbody.velocity;
        }

        set
        {
            _rigidbody.velocity = value;
        }
    }

    Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}

