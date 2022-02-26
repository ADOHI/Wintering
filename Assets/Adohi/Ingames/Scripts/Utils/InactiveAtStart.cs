using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class InactiveAtStart : MonoBehaviour
    {
        void Start()
        {
            gameObject.SetActive(false);
        }

    }

}
