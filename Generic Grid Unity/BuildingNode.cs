using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlphaGame.Core.GridSystem
{
    public enum Edge{Left, Up, Right, Down}
    public class BuildingNode
    {
        public int x, z;
        public Grid<BuildingNode> grid;
        public Vector3[] corners;
        public Vector3 position;
        public bool[] isOccupied;
        public bool hasFloor;
        public bool[] hasWall;
        public BuildingNode[] neighbours;
        public bool isRoom;
        public BuildingNode(Grid<BuildingNode> grid, int x, int z)
        {
            this.x = x;
            this.z = z;
            this.grid = grid;
            corners = new Vector3[4];
            hasWall = new bool[4]{false, false, false, false};
            neighbours = new BuildingNode[4];
            position = grid.GetWorldPosition(x, z);
            
            corners[0] = position; // bottom left
            corners[1] = position + new Vector3(0, 0, grid.scale); // top left
            corners[2] = position + new Vector3(grid.scale, 0, grid.scale); // top right
            corners[3] = position + new Vector3(grid.scale, 0, 0); // bottom right

            isOccupied = new bool[3] {false, false, false}; // 0 = floor (for rugs), 1 = general objects(desk etc.) , 2 = ceiling(lamps)
            hasFloor = false;
            isRoom = false;
            
            grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
        }
        private void Grid_OnGridObjectChanged(object sender, Grid<BuildingNode>.OnGridObjectChangedEventArgs e)
        {
        }
        public void SetGrid(Grid<BuildingNode> grid)
        {
            this.grid = grid;
        }
        public void SetOccupied(GameObject obj, int index)
        {
            isOccupied[index] = true;
            grid.TriggerGridObjectChanged(x, z);
        }
        public void AddWall(int index)
        {
            hasWall[index] = true;
            if(neighbours[index] != null)
            {
                neighbours[index].hasWall[(index + 2) % 4] = true;
                grid.TriggerGridObjectChanged(neighbours[index].x, neighbours[index].z);
            }
            grid.TriggerGridObjectChanged(x, z);
        }

        public void CalculateNeighbours()
        {
            if (x > 0) neighbours[0] = grid.GetGridObject(x - 1, z); // left
            if (z < grid.zGridCount - 1) neighbours[1] = grid.GetGridObject(x, z + 1); // up
            if (x < grid.xGridCount - 1) neighbours[2] = grid.GetGridObject(x + 1, z); // right
            if (z > 0) neighbours[3] = grid.GetGridObject(x, z - 1); // down
        }
        public override string ToString()
        {
            return $"IsOccupied: {isOccupied} \n Left: {hasWall[0]} \n Up: {hasWall[1]} \n Right: {hasWall[2]} \n Down: {hasWall[3]}";
        }

    }
}