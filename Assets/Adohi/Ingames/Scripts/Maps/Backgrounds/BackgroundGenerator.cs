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
            public ParallaxBackground backgroundParentPrefab;
            public float minTreeInterval;
            public float maxTreeInterval;


            public void Generate()
            {
                GenerateTrees();
            }


            [Button]
            public void GenerateTrees()
            {
                for (int i = treeParent.childCount - 1; i >= 0; i--)
                {
                    Destroy(treeParent.GetChild(i).gameObject);

                }

                var layerAmount = treeObjectData.Count;

                foreach (var treeData in treeObjectData)
                {
                    var parent = Instantiate(backgroundParentPrefab);

                    parent.transform.SetParent(treeParent);

                    var currentWidth = 0f;

                    parent.camera = Camera.main;

                    parent.moveRatio = treeData.parallaxRatio;

                    parent.gameObject.SetActive(true);

                    while (currentWidth < MapManager.Instance.mapWidth)
                    {
                        var width = Random.Range(treeData.minTreeInterval, treeData.maxTreeInterval);

                        currentWidth += width;

                        var scale = new Vector3(Random.Range(treeData.minTreeScale.x, treeData.maxTreeScale.x), Random.Range(treeData.minTreeScale.y, treeData.maxTreeScale.y), Random.Range(treeData.minTreeScale.z, treeData.maxTreeScale.z));

                        var treeIndex = Random.Range(0, treeData.treePrefabs.Count);

                        var spawnedTree = Instantiate(treeData.treePrefabs[treeIndex]);

                        spawnedTree.transform.SetParent(parent.transform);

                        spawnedTree.transform.position = new Vector3(currentWidth, 0f, treeData.depth);

                        spawnedTree.transform.localScale = scale;

                        spawnedTree.GetComponent<SpriteRenderer>().color = treeData.overlayColor;

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

            public Vector3 minTreeScale;
            public Vector3 maxTreeScale;

            public Color overlayColor;

            public float depth;

            public float parallaxRatio;
        }

    }
}
