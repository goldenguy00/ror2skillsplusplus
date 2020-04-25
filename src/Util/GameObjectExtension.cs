using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Skills {
    public static class GameObjectExtension {

        public static bool TryGetComponent<T>(this GameObject gameObject, out T component) where T : Component {
            component = gameObject.GetComponent<T>();
            return component != null;
        }

    }
}
