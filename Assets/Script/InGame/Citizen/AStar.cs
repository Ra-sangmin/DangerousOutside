using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 참고 링크
/// https://itmining.tistory.com/66
/// </summary>
/// 
[System.Serializable]
public class Node
{
    public Node Parent;
    public NodeType Type;

    public int F { get; private set; } // F = G + H  
    public int G { get; private set; } // 시작지점에서의 거리 
    public int H { get; private set; } // 목적지까지의 거리

    public int X { get; private set; }
    public int Y { get; private set; }

    public Vector2 Position
    {
        get
        {
            return new Vector2(X, Y);
        }
    }

    public Node(Vector2 pos)
    {
        X = (int)pos.x;
        Y = (int)pos.y;
    }
    public Node(float x, float y)
    {
        X = (int)x;
        Y = (int)y;
    }

    public Node(float x, float y, int kind)
    {
        X = (int)x;
        Y = (int)y;

        if (0 <= kind && kind <= 2)
            Type = NodeType.Home;
        else
            Type = (NodeType)kind - 2;
    }

    public Node(float x, float y, Tile_Type type)
    {
        X = (int)x;
        Y = (int)y;
        Type = NodeType.Tile_Offset + (int)type;
    }

    public void SetNodeType(Building_Type kind)
    {
        if (Building_Type.Small <= kind && kind <= Building_Type.Big)
            Type = NodeType.Home;
        else
            Type = (NodeType)kind - 2;
    }

    public void SetNodeType(Tile_Type type)
    {
        Type = NodeType.Tile_Offset + (int)type;
    }

    public void CalcCost(Node startNode, Node endNode)
    {
        SetG(startNode);
        SetH(endNode);
        F = G + H;
    }

    public void CalcCost(Node destination, int g)
    {
        GetH(destination);
        G = g;
        F = G + H;
    }

    /// <summary>
    /// 시작지점에서의 거리 세팅
    /// </summary>
    void SetG(Node startNode)
    {
        int diff_x = Mathf.Abs(X - startNode.X);
        int diff_y = Mathf.Abs(Y - startNode.Y);

        G = (diff_x + diff_y) * 5;
    }

    /// <summary>
    /// 목적지 까지 거리 세팅
    /// </summary>
    /// <param name="endNode"></param>
    void SetH(Node endNode)
    {
        int diff_x = Mathf.Abs(endNode.X - X);
        int diff_y = Mathf.Abs(endNode.Y - Y);
        H = (diff_x + diff_y) * 10;
    }

    void GetH(Node destination)
    {
        int diffX = Mathf.Abs(destination.X - X);
        int diffY = Mathf.Abs(destination.Y - Y);
        H = (diffX + diffY) * 10;
    }
}

public class AStar
{
    public List<Node> OpenList;
    public List<Node> ClosedList;

    TileController tileController;
    Node[,] Map;

    Node StartNode, Destination;
    int newG;
    bool success;

    public AStar(TileController tileController)
    {
        this.tileController = tileController;
        LoadMapData(tileController.Map);
    }

    /// <summary>
    /// 각 노드의 정보 ex) 건물 여부
    /// </summary>
    void LoadMapData(Node[,] map)
    {
        Map = map;
    }

    public Stack<Node> FindPath(Node startNode, Node destination)
    {
        StartNode = startNode;
        Destination = destination;

        Init();
        return AStar_Dir4();
    }

    void Init()
    {
        newG = 0;
        success = false;

        OpenList = new List<Node>();
        ClosedList = new List<Node>();
    }

    /// <summary>
    /// 탐색한 경로 전달
    /// </summary>
    Stack<Node> AStar_Dir4()
    {
        OpenList.Add(StartNode);
        Node CurrentNode = StartNode;
        StartNode.Parent = null;

        while (OpenList.Count > 0)
        {
            if (CurrentNode == Destination)
            {
                success = true; break;
            }

            var adjacents = FindAdjacentNodes(CurrentNode);
            newG = CurrentNode.G + 10;


            foreach (var adj in adjacents)
            {
                //if (ClosedList.Contains(adj))
                //    continue;

                if (OpenList.Contains(adj))
                {
                    // better case
                    if (newG < adj.G)
                    {
                        adj.CalcCost(Destination, newG);
                    }
                }
                else
                {
                    adj.CalcCost(Destination, newG);
                    adj.Parent = CurrentNode;

                    OpenList.Insert(0, adj); // 우선체크
                }
            }

            var best = OpenList.Min(n => n.F);
            CurrentNode = OpenList.First(n => n.F == best);

            ClosedList.Add(CurrentNode);
            OpenList.Remove(CurrentNode);
        }

        //BestPath = new List<Node>();
        Stack<Node> stack = new Stack<Node>();
        while (CurrentNode != null)
        {
            stack.Push(CurrentNode);
            //BestPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        //BestPath.Reverse();
        //if (!success)
        //    Debug.Log("Not Pass");
        return success ? stack : null;
    }

    List<Node> FindAdjacentNodes(Node currentNode)
    {
        int x = currentNode.X;
        int y = currentNode.Y;

        Node[] adjArray = new Node[4];
        adjArray[0] = AddAdjacent(x + 1, y);
        adjArray[1] = AddAdjacent(x, y - 1);
        adjArray[2] = AddAdjacent(x - 1, y);
        adjArray[3] = AddAdjacent(x, y + 1);

        List<Node> results = new List<Node>();
        for (int i = 0; i < adjArray.Length; i++)
        {
            if (adjArray[i] != null)
                results.Add(adjArray[i]);
        }

        return results;
    }
    TileController ctrl;
    Node AddAdjacent(int x, int y)
    {
        // Map 인덱스 벗어날 때
        //if (x < 0 || x >= TileController.x_max_value || y < 0 || y >= TileController.y_max_value)
          //  return null;

        Node adjacent = Map[x, y];

        //if (tileController.banTileList.Contains(adjacent))
        //    return null;

        if (adjacent == null || ClosedList.Contains(adjacent))
            return null;

        //if (home != null && adjacent == Map[(int)home.pos.x, (int)home.pos.y]) // 집에 갇혀서 못 나오는 상황 예외처리
        //    return adjacent;

        if (adjacent == Destination)
            return adjacent; // 목적지가 건물일 때는 인접 노드로 추가해야한다.

        if (NodeType.Home <= adjacent.Type && adjacent.Type <= NodeType.Nothing) // 건물 // 장애물
            return null;

        return adjacent;
    }

    //public void SetHome(Apartment home)
    //{
    //    this.home = home;
    //}
}