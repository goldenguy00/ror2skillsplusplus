using System;
using UnityEngine;

namespace Skills {
    class Logger {

        public static void Debug(string message, params object[] formatArgs) {
#if DEBUG
            if (message == null) {
                message = "null";
            }
            UnityEngine.Debug.LogFormat(message, formatArgs);
#endif
        }
        public static void Debug(object message) {
#if DEBUG
            if (message == null) {
                message = "null";
            }
            UnityEngine.Debug.LogFormat(message.ToString());
#endif
        }

        public static void Warn(string message, params object[] formatArgs) {
            if (message == null) {
                message = "null";
            }
            UnityEngine.Debug.LogWarningFormat(message, formatArgs);
        }

        public static void Error(string message, params object[] formatArgs) {
            if (message == null) {
                message = "null";
            }
            UnityEngine.Debug.LogErrorFormat(message, formatArgs);
        }

        public static void Error(Exception exception) {
            UnityEngine.Debug.LogError(exception);
        }

    }
}
