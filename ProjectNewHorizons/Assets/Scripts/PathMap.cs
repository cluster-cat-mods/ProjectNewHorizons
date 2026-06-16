using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine.Serialization;


public class PathMap : MonoBehaviour
{
    [SerializeField] private bool editMode = false;
    private Graph<Vector3> _graph;
    public List<Vector3> keys;
    public List<List<Vector3>> values;
    void Start()
    {
        _graph = new Graph<Vector3>();
    }
    
    void Update()
    {
        for (int i = 0; i < _graph.GetNodeCount() - 1; i++)
        {
            Debug.DrawLine(_graph.GetNodes()[i], _graph.GetNodes()[i + 1], Color.white);
        }

        if (editMode && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("raycast hit");
                if (_graph.GetNodeCount() < 1) _graph.AddNode(hit.point);
                else _graph.AddEdge(hit.point, _graph.GetNodes()[_graph.GetNodeCount() - 1]);
            }
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        }

    }
    
    [Button]
    private void SaveMap()
    {
        _graph.SaveGraph();
    }

    [Button]
    private void LoadMap()
    {
        _graph.LoadGraph();
    }
    
}
