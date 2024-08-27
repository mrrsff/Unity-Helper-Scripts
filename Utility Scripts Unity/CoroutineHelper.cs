using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AlphaGame.HelperModules
{
    public class CoroutineMonoBehaviour : MonoBehaviour { }

    public static class CoroutineHelper
    {
        private static Dictionary<string, GameObject> coroutineObjects = new Dictionary<string, GameObject>();
        public static Coroutine StartCoroutine(IEnumerator routine, string id)
        {
            // get a reference to the GameObject that will hold the coroutine
            GameObject coroutineObject = new GameObject("CoroutineObject"+ id);
            // add a component to the GameObject that will execute the coroutine
            CoroutineMonoBehaviour behaviour = coroutineObject.AddComponent<CoroutineMonoBehaviour>();
            // add the GameObject to the dictionary
            coroutineObjects.Add(id, coroutineObject);
            // start the coroutine
            return behaviour.StartCoroutine(routine);
        }

        public static void StopCoroutine(Coroutine coroutine, string id)
        {
            // find the GameObject that holds the coroutine
            GameObject coroutineObject = coroutineObjects[id];
            // stop the coroutine
            coroutineObject.GetComponent<CoroutineMonoBehaviour>().StopCoroutine(coroutine);
            // remove from the dictionary
            coroutineObjects.Remove(id);
            // destroy the GameObject
            GameObject.Destroy(coroutineObject);
        }
    }
}