using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

namespace AlphaGame
{
    public class CombineChildrenMeshes : MonoBehaviour
    {
        [Button]
        public void CombineThis(){
            GameObject combinedRoads = MeshUtils.CombineMeshes(new GameObject[]{gameObject});
            combinedRoads.transform.position = Vector3.zero;
            combinedRoads.transform.rotation = Quaternion.identity;
            combinedRoads.transform.localScale = Vector3.one;
            combinedRoads.transform.parent = transform;
            combinedRoads.name = $"Combined {gameObject.name}";
        }
    }
}
