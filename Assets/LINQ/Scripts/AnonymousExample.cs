using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AGGE.LINQ {
    public class AnonymousExample : MonoBehaviour {
        event Action onStart;

        protected void Awake() {
            onStart += () => {
                string name = "Kadda";

                void func(string message) {
                    Debug.Log($"{message} {name}");
                }

                func("Hallo");

                name = "Daniel";

                func("Tschüss");
            };
        }

        protected void Start() {
            onStart?.Invoke();
        }

        Transform GetParent() {
            if (!transform.parent) {
                transform.parent = GameObject.FindGameObjectsWithTag("Parent")[0].transform;
            }
            return transform.parent;
        }

        class Obj {
            public int id;
        }
        List<Obj> objs = new List<Obj>();

        Obj GetObjByIdProcedural(int id) {
            for (int i = 0; i < objs.Count; i++) {
                if (objs[i].id == id) {
                    return objs[i];
                }
            }
            return default;
        }

        Obj GetObjByIdFunctional(int id) {
            return objs.FirstOrDefault(obj => obj.id == id);
        }

        class Vector {
            public float x;
            public float y;
        }

        // impure
        Vector SetX(Vector vector, float newX) {
            vector.x = newX;
            return vector;
        }
        // pure
        Vector WithX(Vector vector, float newX) {
            return new Vector {
                x = newX,
                y = vector.y
            };
        }
    }
}
