using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingames
{
    public class Acorn : MonoBehaviour
    {
        public bool isEated;

        public async UniTask Eated(float delay)
        {
            if (isEated)
            {
                return;
            }
            isEated = true;
            StopAcorn();
            await UniTask.Delay((int)(delay * 1000f));
            Destroy(gameObject);
        }

        public void StopAcorn()
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //GetComponent<Rigidbody2D>().fr = true;
        }
    }

}
