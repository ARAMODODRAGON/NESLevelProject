using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * created by Domara Shlimon
 * ver 4.0
 */

[System.Serializable]
public class Ctrigger {
    public string name;
    //public string layerName;
    public LayerMask layerMask;
    public Vector2 offset;
    public Vector2 size;
    public bool DrawGizmo;
    public bool isTriggered = false; // not in the inspector
}

public class MultiTrigger : MonoBehaviour {
    [HideInInspector]
    public List<Ctrigger> Triggers = new List<Ctrigger>();
    private List<GameObject> listOfGO = new List<GameObject>();

    private int numOfCollisions;

    private void Awake() {
        numOfCollisions = 0;
        if (Triggers.Count == 0) return;

        for (int i = 0; i < Triggers.Count; i++) {
            Triggers[i].isTriggered = false;
            if (listOfGO.Find(GameObject => GameObject.name == Triggers[i].name)) Triggers[i].name += 1;
            listOfGO.Add(new GameObject(Triggers[i].name));
            listOfGO[i].transform.SetParent(transform);
            listOfGO[i].transform.position = transform.position;
            listOfGO[i].AddComponent<ChildTrigger>().lm = Triggers[i].layerMask;
            //listOfGO[i].hideFlags = HideFlags.HideInHierarchy;
            var bc = listOfGO[i].AddComponent<BoxCollider2D>();
            bc.size = Triggers[i].size;
            bc.offset = Triggers[i].offset;
            bc.isTrigger = true;
        }
    }

    public bool Check(string triggerName) {
        return Triggers.Find(Ctrigger => Ctrigger.name == triggerName).isTriggered && IsColliding;
    }

    public bool disable(params string[] na) {
        foreach (var n in na) {
            listOfGO.FindAll(GameObject => GameObject.name == n).ForEach(GameObject => GameObject.SetActive(false));
        }
        return true;
    }

    public bool enable(params string[] na) {
        foreach (var n in na) {
            listOfGO.FindAll(GameObject => GameObject.name == n).ForEach(GameObject => GameObject.SetActive(true));
        }
        return true;
    }

    private void OnCollisionEnter2D(Collision2D col) { numOfCollisions++; }
    private void OnCollisionExit2D(Collision2D col) { numOfCollisions--; }
    private bool IsColliding {
        get {
            return numOfCollisions != 0;
        }
    }

    public void setTrigger(bool b, string n) {
        Triggers.Find(Ctrigger => Ctrigger.name == n).isTriggered = b;
    }
    
    private void OnDrawGizmos() {
        Color outline = Color.white;
        outline.r = 0.1f;
        outline.g = 0.6f;
        outline.b = 0.9f;
        outline.a = 1f;
        Color fill = outline;
        fill.a = 0.3f;

        foreach (var trig in Triggers) {
            if (!trig.DrawGizmo) continue;

            Gizmos.color = fill;
            Gizmos.DrawCube((Vector3)trig.offset + transform.position, trig.size);

            Gizmos.color = outline;
            Gizmos.DrawWireCube((Vector3)trig.offset + transform.position, trig.size);
        }
    }

    public override string ToString() {
        string s = "";
        foreach (var item in Triggers) {
            s += "( " + item.isTriggered + " ) ";
        }
        return s;
    }
}
