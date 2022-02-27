using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Ingames
{
    public class EatingModule : MonoBehaviour
    {
        public int currentAcornCount;
        public float eatingDelay = 2f;
        public IntEventReference onEatAcorn;
        public VoidBaseEventReference onEatAcornEnd;

        private async void Eat(Acorn acorn)
        {
            if (IsEatAvilable())
            {
                currentAcornCount++;
                onEatAcorn.Event.Raise(currentAcornCount);
                await acorn.Eated(eatingDelay);
                onEatAcornEnd.Event.Raise();
            }
        }

        private bool IsEatAvilable()
        {
            return currentAcornCount < IngameProgressManager.Instance.maxAcornCount;
        }

        public void InitialzeAcorn()
        {
            currentAcornCount = 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Acorn acorn))
            {
                Debug.Log("Acorn Collided");

                if (!acorn.isEated)
                {
                    Eat(acorn);
                }
            }
        }


    }

}
