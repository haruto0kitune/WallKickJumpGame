﻿using UnityEngine;
using System.Collections;

public class Pauser : MonoBehaviour, IPause
{
    Rigidbody2D _rigidbody2d;
    Vector2 velocityStore;
    float angularVelocityStore;
    float gravityScaleStore;

    Animator animator;

    void Awake()
    {
        PauseManager.pausers.Add(this);
        _rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
    }

    public void Pause()
    {
        // Pause Rigidbody
        if (_rigidbody2d != null)
        {
            velocityStore = _rigidbody2d.velocity;
            angularVelocityStore = _rigidbody2d.angularVelocity;
            gravityScaleStore = _rigidbody2d.gravityScale;
            _rigidbody2d.gravityScale = 0f;
            _rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
            _rigidbody2d.Sleep();
        }

        // Pause Animation
        if (animator != null)
        {
            animator.speed = 0;
        }
    }

    public void Resume()
    {
        // Resume Rigidbody
        if (_rigidbody2d != null)
        {
            _rigidbody2d.WakeUp();
            _rigidbody2d.velocity = velocityStore;
            _rigidbody2d.angularVelocity = angularVelocityStore;
            _rigidbody2d.gravityScale = gravityScaleStore;
            _rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Resume Animation
        if (animator != null)
        {
            animator.speed = 1;
        }
    }
}
