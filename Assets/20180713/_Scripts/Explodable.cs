﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _20180713._Scripts;

public class Explodable : MonoBehaviour
{
    public float ExplodeInSeconds = 10;
    public float Radius;
    public float Force;

    private float time = 0;
    private bool running = false;

    void Update()
    {
        if (!running) return;

        time += Time.deltaTime;
        if (time > ExplodeInSeconds)
        {
            var colliders = Physics.OverlapSphere(transform.position, Radius);
            foreach (var collider in colliders)
            {
                var block = collider.GetComponent<Block>();
                if (block != null)
                {
                    var holder = block.GetHolder();
                    if (holder != null)
                    {
                        var @base = holder.GetComponent<Base>();
                        if (@base != null)
                        {
                            @base.DetachBlock(block);
                        }
                    }
                }

                var body = collider.GetComponent<Rigidbody>();
                if (body == null && collider.transform.parent != null)
                {
                    body = collider.transform.parent.GetComponent<Rigidbody>();
                }

                if (body != null)
                {
                    body.AddExplosionForce(Force, transform.position, Radius);
                }
            }

            Destroy(gameObject);
        }
    }

    void Arm()
    {
        if (time < 1)
        {
            ExplodeInSeconds = Random.Range(10, 20);
            time = 0;
        }

        running = true;
    }

    void Disarm()
    {
        time = 0;
        running = false;
    }
}