using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlphaGame.Core.GridSystem
{
    public class PathNode
    {
        public Grid<PathNode> grid;
        public List<PathNode> neighbourList;
        public bool isWalkable;
        public int x, z;
        public int gCost;
        public int hCost;
        public int fCost { get { return gCost + hCost; } }
        public PathNode previousNode;
        public PathNode(Grid<PathNode> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;

            isWalkable = true;

            neighbourList = new List<PathNode>();
        }
        public override string ToString()
        {
            return "";//$"(x: {x}, z: {z}, fCost: {fCost}, gCost: {gCost}, hCost: {hCost})";
        }
        public void SetGrid(Grid<PathNode> grid)
        {
            this.grid = grid;
        }

        public List<PathNode> GetNeighbours()
        {
            return neighbourList;
        }
        public void CopyFromBuildingGrid(Grid<BuildingNode> buildingGrid)
        {
            if(buildingGrid == null)
            {
                Debug.LogError("Building grid is null");
                return;
            }

            BuildingNode buildingNode = buildingGrid.GetGridObject(x, z);
            
            isWalkable = !buildingNode.isOccupied[1];
            if(isWalkable) // if walkable, get neighbours
            {
                int left = 0, right = 0, up = 0, down = 0;
                int leftUp = 0, leftDown = 0, rightUp = 0, rightDown = 0;
                neighbourList = new List<PathNode>();
                if (buildingNode.x > 0 && !buildingNode.hasWall[(int)Edge.Left]) // if left node is walkable
                {
                    left += 1;

                    var leftNode = buildingGrid.GetGridObject(buildingNode.x - 1, buildingNode.z);
                    
                    if (buildingNode.z > 0 && !leftNode.hasWall[(int)Edge.Down])
                    {
                        leftDown += 1; 
                    }
                    if (buildingNode.z < buildingGrid.zGridCount - 1 && !leftNode.hasWall[(int)Edge.Up])
                    {
                        leftUp += 1;
                    }
                }
                if (buildingNode.x < buildingGrid.xGridCount - 1 && !buildingNode.hasWall[(int)Edge.Right]) // if right node is walkable
                {
                    right += 1;

                    var rightNode = buildingGrid.GetGridObject(buildingNode.x + 1, buildingNode.z);
                    if (buildingNode.z > 0 && !rightNode.hasWall[(int)Edge.Down])
                    {
                        rightDown += 1;
                    }
                    if (buildingNode.z < buildingGrid.zGridCount - 1 && !rightNode.hasWall[(int)Edge.Up])
                    {
                        rightUp += 1;
                    }
                }
                if (buildingNode.z > 0 && !buildingNode.hasWall[(int)Edge.Down])
                {
                    down += 1;

                    var downNode = buildingGrid.GetGridObject(buildingNode.x, buildingNode.z - 1);
                    if (buildingNode.x > 0 && !downNode.hasWall[(int)Edge.Left])
                    {
                        leftDown += 1;
                    }
                    if (buildingNode.x < buildingGrid.xGridCount - 1 && !downNode.hasWall[(int)Edge.Right])
                    {
                        rightDown += 1;
                    }
                }
                if (buildingNode.z < buildingGrid.zGridCount - 1 && !buildingNode.hasWall[(int)Edge.Up])
                {
                    up += 1;

                    var upNode = buildingGrid.GetGridObject(buildingNode.x, buildingNode.z + 1);
                    if (buildingNode.x > 0 && !upNode.hasWall[(int)Edge.Left])
                    {
                        leftUp += 1;
                    }
                    if (buildingNode.x < buildingGrid.xGridCount - 1 && !upNode.hasWall[(int)Edge.Right])
                    {
                        rightUp += 1;
                    }
                }
                // Add the nodes that we can reach from all possibilities.
                if(left == 1) 
                {
                    neighbourList.Add(grid.GetGridObject(buildingNode.x - 1, buildingNode.z));
                }
                if(right == 1) 
                {
                    neighbourList.Add(grid.GetGridObject(buildingNode.x + 1, buildingNode.z));
                }
                if(up == 1) 
                {
                    neighbourList.Add(grid.GetGridObject(buildingNode.x, buildingNode.z + 1));
                }
                if(down == 1) 
                {
                    neighbourList.Add(grid.GetGridObject(buildingNode.x, buildingNode.z - 1));
                }
                if(leftUp == 2) 
                {
                    neighbourList.Add(grid.GetGridObject(buildingNode.x - 1, buildingNode.z + 1));
                }
                if(leftDown == 2) 
                {
                    neighbourList.Add(grid.GetGridObject(buildingNode.x - 1, buildingNode.z - 1));
                }
                if(rightUp == 2) 
                {
                    neighbourList.Add(grid.GetGridObject(buildingNode.x + 1, buildingNode.z + 1));
                }
                if(rightDown == 2) 
                {
                    neighbourList.Add(grid.GetGridObject(buildingNode.x + 1, buildingNode.z - 1));
                }
            }
            else
            {
                neighbourList = new List<PathNode>();
            }
        }
    }
}
