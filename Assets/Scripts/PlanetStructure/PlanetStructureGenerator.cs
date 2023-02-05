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
using UnityEditor.SceneManagement;

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

        [Min(0)]
        public int nodeMarkerRootIndex;
    }

    [SerializeField]
    private DebugData debugData;

    [SerializeField]
    private MeshFilter planetMesh;

    [SerializeField]
    private GameObject nodeMarkerObject;

    [SerializeField]
    private GameObject linkObject;

    private PlanetStructure planetStructure; // only data

    private Dictionary<StructureNode, StructureNodeHandler> nodeHandlers = null;

    private VertexColorInterpreter vertexColorInterpeter;

    private void Awake()
    {
        vertexColorInterpeter = GetComponent<VertexColorInterpreter>();
        GenerateData();
        if (debugData.activateDebug)
        {
            DrawDebug();
        }
    }

    private void GenerateData()
    {
        FindObjectOfType<ModelConfiguration>().BeReady();
        planetStructure = new PlanetStructure();
        nodeHandlers = new Dictionary<StructureNode, StructureNodeHandler>();
        List<StructureNode> nodes = null;
        StructureNode rootNode = null;

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
                nodeHandlers = new Dictionary<StructureNode, StructureNodeHandler>(meshDataArray[0].vertexCount);
                nodes = new List<StructureNode>(meshDataArray[0].vertexCount); // this collapses identical vertices
                List<StructureNode> allNodes = new List<StructureNode>(meshDataArray[0].vertexCount); // this contains duplicated vertices 

                using (NativeArray<VertexData> vertices = meshDataArray[0].GetVertexData<VertexData>(0))
                {
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        StructureNode newNode;
                        if (vertexColorInterpeter != null)
                        {
                            newNode = new StructureNode(transform, vertices[i], vertexColorInterpeter.GetNodeModelFromColor);
                        }
                        else
                        {
                            newNode = new StructureNode(transform, vertices[i], null);
                        }

                        StructureNode duplicatedNode = null;
                        foreach (StructureNode n in nodes)
                        {
                            if (Mathf.Approximately(Vector3.Distance(n.localPosition, newNode.localPosition), 0f))
                            {
                                duplicatedNode = n;
                                if (!newNode.HasSameData(duplicatedNode))
                                {
                                    Debug.LogWarning("Duplicated vertices have different data!");
                                }
                                break;
                            }
                        }

                        if (duplicatedNode == null)
                        {
                            nodes.Add(newNode);
                            allNodes.Add(newNode);

                            GameObject newMarker = Instantiate(nodeMarkerObject, planetMesh.transform);
                            newMarker.transform.SetLocalPositionAndRotation(newNode.localPosition, newNode.localRotation);

                            newMarker.transform.localScale = getPlanetInverseScaleFactor();

                            Collider c = newMarker.GetComponent<Collider>();
                            if (c == null)
                            {
                                c = newMarker.AddComponent<SphereCollider>();
                            }

                            StructureNodeHandler nodeHandler = newMarker.GetComponent<StructureNodeHandler>();
                            if (nodeHandler == null)
                            {
                                nodeHandler = newMarker.AddComponent<StructureNodeHandler>();
                            }
                            nodeHandler.InitializeNodeData(newNode);
                            nodeHandlers.Add(newNode, nodeHandler);

                            if (newNode.model == NodeModel.Root)
                            {
                                rootNode = newNode;
                            }
                        }
                        else
                        {
                            // in this case, the new node is duplicated. Each node is added only once to the graph (nodes collection),
                            // but twice to the allNodes collection, because the triangles indexes refer to the duplicated vertices indexes
                            allNodes.Add(duplicatedNode);
                        }

                    }

                    // Contains an array of triangles. For each triangle, the 3 vertices indices are specified in sequence.
                    int[] triagles = planetMesh.sharedMesh.GetTriangles(0, true);

                    for (int i = 0; i < triagles.Length; i += 3)
                    {
                        allNodes[triagles[i]].addNeighbour(allNodes[triagles[i + 1]]);
                        allNodes[triagles[i + 1]].addNeighbour(allNodes[triagles[i + 2]]);
                        allNodes[triagles[i + 2]].addNeighbour(allNodes[triagles[i]]);
                    }
                }
            }           
        }

        if (nodes != null && nodeHandlers != null)
        {
            if (rootNode == null)
            {
                Debug.LogError("Root node not found. The first node will be reinitialized as root. ");
                int fallbackRootNodeIndex = nodeHandlers.Count > debugData.nodeMarkerRootIndex ? debugData.nodeMarkerRootIndex : (nodeHandlers.Count > 0 ? 0 : -1);
                if (fallbackRootNodeIndex >= 0)
                {
                    rootNode = nodes[fallbackRootNodeIndex];
                    Debug.LogError("Forced root node: '" + nodeHandlers[rootNode].gameObject.name + "'");
                    NodeModel.Root = rootNode.model;
                    nodeHandlers[rootNode].InitializeNodeData(rootNode);
                }
            }

            planetStructure = new PlanetStructure(nodes, rootNode);
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
    
            for (int i = debugData.nodeMarkerStartIndex; i < Mathf.Min(debugData.nodeMarkerStartIndex + debugData.nodeMarkersCount, planetStructure.nodes.Count); i++)
            {
                GameObject newMarker = Instantiate((GameObject)null, planetMesh.transform);
                newMarker.transform.SetLocalPositionAndRotation(planetStructure.nodes[i].localPosition, planetStructure.nodes[i].localRotation);
                newMarker.transform.localScale = getPlanetInverseScaleFactor();
            }
        }
    }

    private Vector3 getPlanetInverseScaleFactor ()
    {
        return new Vector3(1 / planetMesh.transform.localScale.x,
                        1 / planetMesh.transform.localScale.y,
                        1 / planetMesh.transform.localScale.z);
    }

    public void spawnLinkObject (StructureNodeHandler fromNode, StructureNodeHandler toNode)
    {
        GameObject newObject = Instantiate(linkObject, planetMesh.transform);
        Quaternion globalRotation = Quaternion.LookRotation(toNode.nodeData.localPosition - fromNode.nodeData.localPosition, fromNode.nodeData.normalDirection);
        newObject.transform.SetLocalPositionAndRotation(fromNode.nodeData.localPosition, Quaternion.identity);
        newObject.transform.SetPositionAndRotation(newObject.transform.position, globalRotation);

        newObject.transform.localScale = getPlanetInverseScaleFactor();
    }

    public StructureNodeHandler getHandler (StructureNode n)
    {
        return nodeHandlers[n];
    }
}
