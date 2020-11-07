using UnityEngine;
using UnityEditor;
using System.Linq;

public class CreateParentObjectWithSameName {
    [MenuItem("GameObject/Create Centered Empty Parent With Same Name _q", priority = 0)]
    public static void CreateEmpty() {
        foreach(var obj in Selection.gameObjects.Where(go => go.transform.parent == null)){
            var empty = new GameObject(obj.name);
            obj.transform.parent = empty.transform;
        }
    }
}