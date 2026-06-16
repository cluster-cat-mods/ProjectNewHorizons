using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Graph<T>
{
    private Dictionary<T, List<T>> adjacencyList;
    public DictionaryWrapper<T> wrapper = new();

    public Graph()
    {
        adjacencyList = new Dictionary<T, List<T>>();
    }

    public Graph(Dictionary<T, List<T>> adjacencyListP)
    {
        adjacencyList = adjacencyListP;
    }

    public Dictionary<T, List<T>> GetAdjacencyList()
    {
        return adjacencyList;
    }

    public void Clear()
    {
        adjacencyList.Clear();
    }

    public void RemoveNode(T node)
    {
        if (adjacencyList.ContainsKey(node))
        {
            adjacencyList.Remove(node);
        }

        foreach (var key in adjacencyList.Keys)
        {
            adjacencyList[key].Remove(node);
        }
    }

    public List<T> GetNodes()
    {
        return new List<T>(adjacencyList.Keys);
    }

    public void AddNode(T node)
    {
        if (!adjacencyList.ContainsKey(node))
        {
            adjacencyList[node] = new List<T>();
        }
    }

    public void RemoveEdge(T fromNode, T toNode)
    {
        if (adjacencyList.ContainsKey(fromNode))
        {
            adjacencyList[fromNode].Remove(toNode);
        }
        if (adjacencyList.ContainsKey(toNode))
        {
            adjacencyList[toNode].Remove(fromNode);
        }
    }

    public void AddEdge(T fromNode, T toNode)
    {
        if (!adjacencyList.ContainsKey(fromNode))
        {
            AddNode(fromNode);
        }
        if (!adjacencyList.ContainsKey(toNode))
        {
            AddNode(toNode);
        }

        adjacencyList[fromNode].Add(toNode);
        adjacencyList[toNode].Add(fromNode);
    }

    public List<T> GetNeighbors(T node)
    {
        return new List<T>(adjacencyList[node]);
    }

    public int GetNodeCount()
    {
        return adjacencyList.Count;
    }

    public void PrintGraph()
    {
        foreach (var node in adjacencyList)
        {
            Debug.Log($"{node.Key}: {string.Join(", ", node.Value)}");
        }
    }
    
    public bool CheckConnectivityBFS(out HashSet<T> visited)
    {
        visited = BFS(GetNodes()[0]);
        int visitedCount = visited.Count;
        return GetNodeCount() == visitedCount;
    }
    
    public bool CheckConnectivityDFS(out HashSet<T> visited)
    {
        visited = DFS(GetNodes()[0]);
        int visitedCount = visited.Count;
        return GetNodeCount() == visitedCount;
    }
    
    public bool CheckConnectivityDFSRecursive(out HashSet<T> visited)
    {
        visited =  DFSRecursive(GetNodes()[0]);
        int visitedCount = visited.Count;
        return GetNodeCount() == visitedCount;
    }
    
    public HashSet<T> BFS (T startNode)
    {
        Queue<T> queue = new Queue<T>();
        HashSet<T> visited = new HashSet<T>();
        queue.Enqueue(startNode);
        visited.Add(startNode);
        while (queue.Count > 0)
        {
            T v = queue.Dequeue();
            foreach (var edge in GetNeighbors(v))
            {
                if (!visited.Contains(edge))
                {
                    queue.Enqueue(edge);
                    visited.Add(edge);
                }
            }
            
        }
        
        return new HashSet<T>(visited);
    }
    
    
    public HashSet<T> DFS(T startNode)
    {
        Stack<T> stack = new Stack<T>();
        HashSet<T> visited = new HashSet<T>();
        stack.Push(startNode);
        visited.Add(startNode);
        while (stack.Count > 0)
        {
            T v = stack.Pop();
            foreach (var edge in GetNeighbors(v))
            {
                if (!visited.Contains(edge))
                {
                    stack.Push(edge);
                    visited.Add(edge);
                }
            }
        }
        
        return new HashSet<T>(visited);
    }
    
    public HashSet<T> DFSRecursive(T v)
    {
        HashSet<T> discovered = new HashSet<T>();
        DFSRecursion(discovered, v);
        return discovered;
    }
    
    private void DFSRecursion(HashSet<T> discovered, T v)
        {
            if (!discovered.Contains(v))
            {
                discovered.Add(v);
                foreach (T w in GetNeighbors(v))
                {
                    DFSRecursion(discovered, w);
                }
            }
        }
    
    public void SaveGraph()
    {
        wrapper.keys = adjacencyList.Keys.ToList();
        wrapper.values = adjacencyList.Values.ToList();
        string filePath = "Data/graphData.json";
        string jsonString = JsonUtility.ToJson(wrapper, true);
        Debug.Log(jsonString);
        File.WriteAllText(filePath, jsonString);
    }

    public void LoadGraph()
    {
        string filePath = "Data/graphData.json";
        string jsonString = File.ReadAllText(filePath);
        wrapper = JsonUtility.FromJson<DictionaryWrapper<T>>(jsonString);
        adjacencyList = wrapper.keys.Zip(wrapper.values, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);
        
    }
     
    
}

[System.Serializable]
public class DictionaryWrapper<T>
{
    public List<T> keys;
    public List<List<T>> values;
}