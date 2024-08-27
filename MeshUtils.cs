using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshUtils
{
    public static GameObject CombineMeshes(GameObject[] objects)
    {
        GameObject combined = new GameObject("Combined");
        // Mesh filters may be in the children
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        foreach (GameObject go in objects)
        {
            var filters = go.GetComponentsInChildren<MeshFilter>();
            foreach (var filter in filters)
            {
                meshFilters.Add(filter);
            }
        }
        // Combine all meshes
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        int i = 0;
        while (i < meshFilters.Count)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }
        // Create a new mesh on the combined object
        MeshFilter mf = (MeshFilter)combined.AddComponent(typeof(MeshFilter));
        mf.sharedMesh = new Mesh();
        mf.sharedMesh.CombineMeshes(combine);
        // Use the first material of the first mesh
        MeshRenderer mr = (MeshRenderer)combined.AddComponent(typeof(MeshRenderer));
        mr.material = meshFilters[0].GetComponent<Renderer>().sharedMaterial;
        
        // Delete the original objects
        foreach (GameObject go in objects)
        {
            GameObject.Destroy(go);
        }
        return combined;
    }
}
