using System;
using UnityEngine;

namespace SkillsPlusPlus {
    internal class Logger {

        private static readonly string LOG_TAG = "Skills++";

        public static void Debug(string message, params object[] formatArgs) {
#if DEBUG
            var formattedMessage = "null";
            if(message != null) {
                formattedMessage = String.Format(message, formatArgs);
            }
            UnityEngine.Debug.unityLogger.Log(LOG_TAG, formattedMessage);
#endif
        }
        public static void Debug(object message) {
#if DEBUG
            if (message == null) {
                message = "null";
            }
            UnityEngine.Debug.unityLogger.Log(LOG_TAG, message);
#endif
        }

        public static void Warn(string message, params object[] formatArgs) {
            var formattedMessage = "null";
            if(message != null) {
                formattedMessage = String.Format(message, formatArgs);
            }
            UnityEngine.Debug.unityLogger.LogWarning(LOG_TAG, formattedMessage);
        }

        public static void Error(string message, params object[] formatArgs) {
            var formattedMessage = "null";
            if (message != null) {
                formattedMessage = String.Format(message, formatArgs);
            }
            UnityEngine.Debug.unityLogger.LogError(LOG_TAG, formattedMessage);
        }

        public static void Error(Exception exception) {
            UnityEngine.Debug.unityLogger.LogError(LOG_TAG, exception);
        }

    }
}
