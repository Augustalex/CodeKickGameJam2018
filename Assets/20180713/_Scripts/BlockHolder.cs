﻿using System.Collections.Generic;
using UnityEngine;

namespace _20180713._Scripts
{

    [RequireComponent(typeof(ShipOwner))]
    public class BlockHolder : MonoBehaviour
    {
        public SoundOneshot soundOneshot;

        public Transform HoldingPoint;
        
        private Base Base;
        private Block holdingBlock;
        private PlayerMovement playerMovement;

        private bool isPickingUpBlockThisFrame;

        void Awake()
        {
            Base = GetComponent<ShipOwner>().OwnBase;
            playerMovement = GetComponent<PlayerMovement>();
        }

        void Update()
        {
            if (IsTryingToRelease())
            {
                if (Base.IsBlockCloseEnough(holdingBlock))
                {
                    AttachHoldingBlockToBase();
                }
                else
                {
                    ReleaseHoldingBlock();
                    soundOneshot.PlaySound(soundOneshot.dropBlock, transform.position);
                }
            }

            if (IsHoldingBlock())
            {
                var closestJoints = Base.GetClosestTwoJoints(holdingBlock);
                Debug.DrawLine(closestJoints.BlockJoint.GetEndPosition(),
                    closestJoints.BaseJoint.GetEndPosition(), Color.red);
                
            }

            if (isPickingUpBlockThisFrame)
            {
                soundOneshot.PlaySound(soundOneshot.pickupBlock, transform.position);
                isPickingUpBlockThisFrame = false;
            }
 
        }

        public void SetHoldingBlock(Block block)
        {
            holdingBlock = block;
            block.transform.position = HoldingPoint.position;
            block.SetHolder(gameObject);
            isPickingUpBlockThisFrame = true;
        }

        private void ReleaseHoldingBlock()
        {
            holdingBlock.Release();
            holdingBlock = null;
        }

        private void AttachHoldingBlockToBase()
        {
            holdingBlock.Release();
            Base.AttachBlock(holdingBlock);
            holdingBlock = null;
        }

        public bool IsTryingToPickUp()
        {
            return !IsHoldingBlock() && Input.GetButtonDown(playerMovement.InteractInput);
        }

        private bool IsTryingToRelease()
        {
            return IsHoldingBlock() && Input.GetButtonDown(playerMovement.InteractInput);
        }

        private bool IsHoldingBlock()
        {
            return holdingBlock && !isPickingUpBlockThisFrame;
        }
    }
}