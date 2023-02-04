using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Mesh;

using PlanetStructureTypes;
using System.ComponentModel;
using System.Transactions;
using System;
using Unity.Collections.LowLevel.Unsafe;

public class PlanetStructureGenerator : MonoBehaviour
{
    [Serializable]
    private struct DebugData
    {
        public bool activateDebug;
        public GameObject nodeMarkerObject;
        [Min(0)]
        public int nodeMarkerStartIndex;
        [Min(1)]
        public int nodeMarkersCount;
    }

    [SerializeField]
    private DebugData debugData;

    [SerializeField]
    private MeshFilter planetMesh;

    private PlanetStructure planetStructure;

    private void Awake()
    {
        GenerateData();
        if (debugData.activateDebug)
        {
            DrawDebug();
        }
    }

    private void GenerateData()
    {
        planetStructure = new PlanetStructure();
        StructureNode[] nodes = null;

        if (planetMesh)
        {
            // CRITICALLY IMPORTANT: meshFilter.sharedMesh is the mesh IN THE ASSET. It must NEVER be modified.
            using (MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(planetMesh.sharedMesh))
            {

                if (meshDataArray.Length == 0) { Debug.LogError("The planet structure model does not contain any mesh"); return; }
                if (planetMesh.sharedMesh.subMeshCount == 0) { Debug.LogError("The planet structure mesh does not contains any subMesh"); return; }

                if (meshDataArray.Length > 1) { Debug.LogError("The planet structure model contains multiple meshes"); return; }
                if (planetMesh.sharedMesh.subMeshCount > 1) { Debug.LogError("The planet structure mesh contains multiple subMeshes"); return; }

                // Read nodes
                nodes = new StructureNode[meshDataArray[0].vertexCount];
                using (NativeArray<VertexData> vertices = meshDataArray[0].GetVertexData<VertexData>(0))
                {
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        nodes[i] = new StructureNode(i, transform, vertices[i]);
                    }

                    // Contains an array of triangles. For each triangle, the 3 vertices indices are specified in sequence.
                    int[] triagles = planetMesh.sharedMesh.GetTriangles(0, true);

                    for (int i = 0; i < triagles.Length; i += 3)
                    {
                        nodes[triagles[i]].addNeighbour(nodes[triagles[i + 1]]);
                        nodes[triagles[i + 1]].addNeighbour(nodes[triagles[i + 2]]);
                        nodes[triagles[i + 2]].addNeighbour(nodes[triagles[i]]);
                    }
                }
            }           
        }

        if (nodes != null)
        {
            planetStructure = new PlanetStructure(nodes);
        }
    }

    private void DrawDebug()
    {
        if (Debug.isDebugBuild && debugData.nodeMarkerObject != null
            && planetStructure != null
            && planetMesh != null && planetMesh.gameObject != null)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            for (int i = debugData.nodeMarkerStartIndex; i < Mathf.Min(debugData.nodeMarkerStartIndex + debugData.nodeMarkersCount, planetStructure.nodes.Length); i++)
            {
                Transform newObjectTransform = planetMesh.gameObject.transform;
                GameObject newMarker = Instantiate((GameObject)null, planetMesh.transform);
                newMarker.transform.SetLocalPositionAndRotation(planetStructure.nodes[i].localPosition, planetStructure.nodes[i].localRotation);
            }
        }
    }
}
