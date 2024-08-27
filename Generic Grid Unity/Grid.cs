using System;
using UnityEngine;

namespace AlphaGame.Core.GridSystem
{
    public class Grid<TGridObject>
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
        public class OnGridObjectChangedEventArgs : EventArgs 
        {
            public int x;
            public int z;
        }
        public float scale;
        public int xGridCount, zGridCount;
        public TGridObject[,] gridNodes;
        public Vector3 originPosition;

        public Grid(int xGridCount, int zGridCount, float scale, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) 
        {
            this.xGridCount = xGridCount;
            this.zGridCount = zGridCount;
            this.scale = scale;
            this.originPosition = originPosition;

            gridNodes = new TGridObject[xGridCount, zGridCount];

            for (int x = 0; x < gridNodes.GetLength(0); x++) {
                for (int z = 0; z < gridNodes.GetLength(1); z++) {
                    gridNodes[x, z] = createGridObject(this, x, z);
                }
            }

            bool showDebug = Globals.showGridDebug;
            if (showDebug) {
                GameObject parent = new GameObject("Grid Debug Parent");
                TextMesh[,] debugTextArray = new TextMesh[xGridCount, zGridCount];

                for (int x = 0; x < gridNodes.GetLength(0); x++) {
                    for (int z = 0; z < gridNodes.GetLength(1); z++) {
                        debugTextArray[x, z] = new GameObject().AddComponent<TextMesh>();
                        debugTextArray[x, z].transform.position = GetWorldPosition(x, z)  + new Vector3(scale, 0.1f, scale) / 2f;
                        debugTextArray[x, z].characterSize = .1f;
                        debugTextArray[x, z].fontSize = 10;
                        debugTextArray[x, z].anchor = TextAnchor.MiddleCenter;
                        debugTextArray[x, z].alignment = TextAlignment.Center;
                        debugTextArray[x, z].color = Color.black;
                        debugTextArray[x, z].transform.localScale = new Vector3(1, 1, -1);
                        debugTextArray[x, z].transform.rotation = Quaternion.Euler(90, 0, 0);
                        debugTextArray[x, z].transform.parent = parent.transform;
                        debugTextArray[x, z].text = gridNodes[x, z]?.ToString();

                        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, zGridCount), GetWorldPosition(xGridCount, zGridCount), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(xGridCount, 0), GetWorldPosition(xGridCount, zGridCount), Color.white, 100f);

                OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                    debugTextArray[eventArgs.x, eventArgs.z].text = gridNodes[eventArgs.x, eventArgs.z]?.ToString();
                };
            }
        }
        public Vector3 GetWorldPosition(int x, int z) 
        {
            return new Vector3(x, 0, z) * scale + originPosition;
        }
        public void GetXZ(Vector3 worldPosition, out int x, out int z) 
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / scale);
            z = Mathf.FloorToInt((worldPosition - originPosition).z / scale);
        }
        public void SetGridObject(int x, int z, TGridObject value)
        {
            if(x >= 0 && z >= 0 && x < xGridCount && z < zGridCount) {
                gridNodes[x, z] = value;
                if(OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, z = z });
            }
        }
        public void TriggerGridObjectChanged(int x, int z) {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, z = z });
        }
        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            SetGridObject(x, z, value);
        }
        public TGridObject GetGridObject(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < xGridCount && z < zGridCount) {
                return gridNodes[x, z];
            } else {
                return default(TGridObject);
            }
        }
        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            return GetGridObject(x, z);
        }
    }
}