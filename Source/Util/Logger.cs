using System;
using UnityEngine;

namespace SkillsPlusPlus {
    public class Logger {

        public enum LogLevel { 
            None = 0,
            Error = 1,
            Warning = 2,
            Debug = 3
        }

        private static readonly string LOG_TAG = "Skills++";

        public static LogLevel LOG_LEVEL = LogLevel.Warning;

        internal static void Debug(string message, params object[] formatArgs) {
            if(LOG_LEVEL >= LogLevel.Debug) {
                var formattedMessage = "null";
                if(message != null) {
                    formattedMessage = String.Format(message, formatArgs);
                }
                UnityEngine.Debug.unityLogger.Log(LOG_TAG, formattedMessage);
            }
        }

        internal static void Debug(object message) {
            if(LOG_LEVEL >= LogLevel.Debug) {
                if (message == null) {
                    message = "null";
                }
                UnityEngine.Debug.unityLogger.Log(LOG_TAG, message);
            }
        }

        internal static void Warn(string message, params object[] formatArgs) {
            if(LOG_LEVEL >= LogLevel.Warning) {
                var formattedMessage = "null";
                if(message != null) {
                    formattedMessage = String.Format(message, formatArgs);
                }
                UnityEngine.Debug.unityLogger.LogWarning(LOG_TAG, formattedMessage);
            }
        }

        internal static void Error(string message, params object[] formatArgs) {
            if(LOG_LEVEL >= LogLevel.Error) {
                var formattedMessage = "null";
                if(message != null) {
                    formattedMessage = String.Format(message, formatArgs);
                }
                UnityEngine.Debug.unityLogger.LogError(LOG_TAG, formattedMessage);
            }
        }

        internal static void Error(Exception exception) {
            if (LOG_LEVEL >= LogLevel.Error) {
                UnityEngine.Debug.unityLogger.LogError(LOG_TAG, exception);
            }
        }

    }
}
