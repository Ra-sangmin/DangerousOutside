using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar_SM
{
    TileController tileController;

    List<Node> closeList = new List<Node>();

    public Astar_SM(TileController tileController)
    {
        this.tileController = tileController;
    }

    public List<Node> GetPathList(Node startNode , Node endNode)
    {
        closeList = new List<Node>();

        Node currentNode = startNode;

        List<Node> pathList = new List<Node>();

        int yMoveCnt = endNode.Y - startNode.Y;
        int xMoveCnt = endNode.X - startNode.X;

        while (true)
        {
            if (currentNode.X == endNode.X && currentNode.Y == endNode.Y) 
            {
                break;
            }

            if (currentNode.X != endNode.X)
            {
                float xValue = currentNode.X < endNode.X ?
                               currentNode.X + 1 : currentNode.X - 1;

                currentNode = new Node(xValue, currentNode.Y);
            }
            else if (currentNode.Y != endNode.Y)
            {
                float yValue = currentNode.Y < endNode.Y ? 
                               currentNode.Y + 1 : currentNode.Y - 1;
                currentNode = new Node(currentNode.X, yValue);
            }

            pathList.Add(currentNode);
        }


        /*
        while (true)
        {
            if (currentNode.Position == endNode.Position)
                break;

            List<Node> checkNodeList = FindAdjacentNodes(currentNode);

            if (checkNodeList.Count == 0)
                break;

            foreach (var checkNode in checkNodeList)
            {
                checkNode.CalcCost(startNode, endNode);
            }

            int min_f = checkNodeList.Min(data => data.F);
            Node nextNode = checkNodeList.FirstOrDefault(data => data.F == min_f);

            if (nextNode == null)
                break;

            pathList.Add(nextNode);
            closeList.Add(currentNode);

            currentNode = nextNode;
        }*/

        return pathList;
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
            if (adjArray[i] != null && closeList.Find(data => data.Position == adjArray[i].Position) == null)
                results.Add(adjArray[i]);
        }

        return results;
    }

    Node AddAdjacent(int x, int y)
    {
        // Map 인덱스 벗어날 때
        if (x < 0 || x >= GameManager.Ins.tileController.x_max_value || y < 0 || y >= GameManager.Ins.tileController.y_max_value ||
            GameManager.Ins.tileController.wall == null)
            return null;

        //벽이라면
        bool wall = GameManager.Ins.tileController.wall[x, y];

        if (wall)
            return null;

        return new Node(x,y);
    }


}
