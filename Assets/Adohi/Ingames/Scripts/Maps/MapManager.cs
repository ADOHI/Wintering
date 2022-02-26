using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ingames
{
    namespace Maps
    {
        public class MapManager : Singleton<MapManager>
        {
            public float mapWidth;
            public float mapHeight;

            public BackgroundGenerator backgroundGenerator;


            [Header("House")]
            public float houseX;
            public GameObject startPoint;
            public void Spawn()
            {
                backgroundGenerator.Generate();
            }
        }
    }
}

