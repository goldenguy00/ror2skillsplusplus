using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace SkillsPlusPlus {
    internal static class GameObjectExtension {

        public static bool TryGetComponent<T>(this GameObject gameObject, out T component) where T : Component {
            component = gameObject.GetComponent<T>();
            return component != null;
        }

        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component {
            component = gameObject.GetComponentInChildren<T>();
            return component != null;
        }

    }
    internal static class ComponentExtension {

        public static bool TryGetComponent<T>(this Component thisComponent, out T component) where T : Component {
            component = thisComponent.GetComponent<T>();
            return component != null;
        }

        public static bool TryGetComponentInChildren<T>(this Component thisComponent, out T component) where T : Component {
            component = thisComponent.GetComponentInChildren<T>();
            return component != null;
        }

    }
}
