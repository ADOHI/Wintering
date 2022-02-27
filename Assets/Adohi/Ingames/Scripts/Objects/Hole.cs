using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Ingame
{
    public class Hole : MonoBehaviour
    {
        public VoidBaseEventReference onTriggerHole;


        public void GoInside()
        {
            Debug.Log("Go Inside");
            onTriggerHole.Event.Raise();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out CharacterController characterController))
            {
                //characterController.
                GoInside();
            }
        }
    }

}
