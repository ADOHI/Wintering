using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ingames
{
    namespace Maps
    {
        public class BackgroundGenerator : MonoBehaviour
        {
            public List<TreeData> treeObjectData;
            public Transform treeParent;

            public float minTreeInterval;
            public float maxTreeInterval;



            

            






            [Button]
            public void GenerateTrees()
            {
                //Destroy(treeParent.destro)

                foreach (Transform child in treeParent)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }

                var layerAmount = treeObjectData.Count;

                foreach (var treeData in treeObjectData)
                {
                    var currentWidth = 0f;

                    while (currentWidth < MapManager.Instance.mapWidth)
                    {
                        var width = Random.Range(treeData.minTreeInterval, treeData.maxTreeInterval);

                        currentWidth += width;

                        var treeIndex = Random.Range(0, treeData.treePrefabs.Count);

                        var spawnedTree = Instantiate(treeData.treePrefabs[treeIndex]);

                        spawnedTree.transform.SetParent(treeParent);

                        spawnedTree.transform.position = new Vector3(currentWidth, 0f, treeData.depth);

                        spawnedTree.SetActive(true);
                    }
                }
            }
        }

        [System.Serializable]
        public struct TreeData
        {
            public int layerIndex;
            public List<GameObject> treePrefabs;

            public float minTreeInterval;
            public float maxTreeInterval;

            public float depth;
        }

    }
}
