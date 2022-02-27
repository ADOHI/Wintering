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
            public int seed;

            public Transform treeParent;
            public List<TreeData> treeObjectData;
            public Transform fogParent;
            public List<FogData> fogObjectData;


            public ParallaxBackground backgroundParentPrefab;

            public float startBackgroundDepth;
            public float backgroundDepthInterval;


            public void Generate()
            {
                Random.InitState(seed);

                GenerateTrees();
                GenerateFog();
            }


            [Button]
            public void GenerateTrees()
            {
                for (int i = treeParent.childCount - 1; i >= 0; i--)
                {
                    Destroy(treeParent.GetChild(i).gameObject);

                }

                //var layerAmount = treeObjectData.Count;
                var backgroundDepth = 0;
                foreach (var treeData in treeObjectData)
                {
                    var parent = Instantiate(backgroundParentPrefab);

                    parent.transform.SetParent(treeParent);

                    var currentWidth = -MapManager.Instance.mapWidth * 0.5f;

                    parent.camera = Camera.main;

                    parent.moveRatio = treeData.parallaxRatio;

                    parent.gameObject.SetActive(true);

                    while (currentWidth < MapManager.Instance.mapWidth * 0.5f)
                    {
                        var width = Random.Range(treeData.minTreeInterval, treeData.maxTreeInterval);

                        currentWidth += width;

                        var scale = new Vector3(Random.Range(treeData.minTreeScale.x, treeData.maxTreeScale.x), Random.Range(treeData.minTreeScale.y, treeData.maxTreeScale.y), Random.Range(treeData.minTreeScale.z, treeData.maxTreeScale.z));

                        var treeIndex = Random.Range(0, treeData.treePrefabs.Count);

                        var isFlip = Random.Range(0, 2) == 0;

                        var spawnedTree = Instantiate(treeData.treePrefabs[treeIndex]);

                        spawnedTree.transform.SetParent(parent.transform);

                        spawnedTree.transform.position = new Vector3(currentWidth, 0f, startBackgroundDepth + backgroundDepth * backgroundDepthInterval);

                        spawnedTree.transform.localScale = scale;

                        var renderers = spawnedTree.GetComponentsInChildren<SpriteRenderer>();

                        foreach (var renderer in renderers)
                        {
                            renderer.color = treeData.overlayColor;

                            renderer.flipX = isFlip;
                        }

                        spawnedTree.SetActive(true);
                    }

                    backgroundDepth++;
                }
            }

            public void GenerateFog()
            {
                for (int i = fogParent.childCount - 1; i >= 0; i--)
                {
                    Destroy(fogParent.GetChild(i).gameObject);

                }

                //var layerAmount = treeObjectData.Count;
                var backgroundDepth = 0;
                foreach (var fogData in fogObjectData)
                {


                    var spawnedFog = Instantiate(fogData.fogPrefab);

                    spawnedFog.transform.SetParent(fogParent.transform);

                    var material = spawnedFog.GetComponent<SpriteRenderer>().material;

                    spawnedFog.transform.position = new Vector3(0f, 0f, startBackgroundDepth + backgroundDepth * backgroundDepthInterval - 0.1f);

                    spawnedFog.SetActive(true);

                    backgroundDepth++;
                }
            }

        }

        [System.Serializable]
        public struct TreeData
        {
            public List<GameObject> treePrefabs;

            public float minTreeInterval;
            public float maxTreeInterval;

            public Vector3 minTreeScale;
            public Vector3 maxTreeScale;

            public Color overlayColor;
            public float parallaxRatio;
        }

        [System.Serializable]
        public struct FogData
        {
            public GameObject fogPrefab;
        }

    }
}
