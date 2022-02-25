
using System;
using System.Collections.Generic;
using UnityEngine;

#if ATOMS_CUSTOM
namespace UnityAtoms
{
    public static class VariableContainer
    {
        private static Dictionary<int, AtomBaseVariable> variableTable = new Dictionary<int, AtomBaseVariable>();

        public static bool RegisterVariable<T>(T variable) where T : AtomBaseVariable
        {
            if (variableTable == null)
            {
                variableTable = new Dictionary<int, AtomBaseVariable>();
            }

            var key = variable.Id.GetHashCode();
            if (variableTable.ContainsKey(key))
            {
                Debug.LogWarning($"Key: {key} is duplicate");
                return false;
            }

            variableTable.Add(key, variable);

            return true;
        }

        public static bool UnregisterVariable<T>(T variable) where T : AtomBaseVariable
        {
            if (variableTable == null)
            {
                variableTable = new Dictionary<int, AtomBaseVariable>();
                Debug.LogWarning("Variable table was null");
                return false;
            }

            var key = variable.Id.GetHashCode();
            if (!variableTable.ContainsKey(key))
            {
                return false;
            }

            variableTable.Remove(key);

            return true;
        }

        public static AtomBaseVariable GetVariable(string id)
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                return default;
            }

            return variable;
        }

        public static AtomBaseVariable GetVariable(string id, out bool isSuccess)
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                isSuccess = false;
                return default;
            }

            isSuccess = true;
            return variable;
        }

        public static T GetVariable<T>(string id) where T : AtomBaseVariable
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                return default;
            }

            if (!typeof(T).IsAssignableFrom(variable.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {variable.GetType()}");
                return default;
            }

            return variable as T;
        }

        public static T GetVariable<T>(string id, out bool isSuccess) where T : AtomBaseVariable
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                isSuccess = false;
                return default;
            }

            if (!typeof(T).IsAssignableFrom(variable.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {variable.GetType()}");
                isSuccess = false;
                return default;
            }

            isSuccess = true;
            return variable as T;
        }

        public static bool GetVariable(string id, out AtomBaseVariable atomVariable)
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                atomVariable = default;
                return false;
            }

            atomVariable = variable;
            return true;
        }

        public static bool GetVariable<T>(string id, out T atomVariable) where T : AtomBaseVariable
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                atomVariable = default;
                return false;
            }

            if (!typeof(T).IsAssignableFrom(variable.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {variable.GetType()}");
                atomVariable = default;
                return false;
            }

            atomVariable = variable as T;
            return true;
        }


        public static Type GetVariableType(string id)
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                return default;
            }

            return variable.GetType();
        }

        public static Type GetValueType(string id)
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                return default;
            }

            return variable.GetType().BaseType.GetGenericArguments()[0];
        }
        /// <summary>
        /// Boxing is exist!. Should cache variable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetVariable<T, U>(string id, U value) where T : AtomBaseVariable
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                return false;
            }

            if (!typeof(T).IsAssignableFrom(variable.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {variable.GetType()}");
                return false;
            }

            if (!typeof(U).IsAssignableFrom(variable.BaseValue.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(U)} is not assignable from valueType {variable.BaseValue.GetType()}");
                return false;
            }

            variable.BaseValue = value;
            return true;
        }
    }

    public static class VariableInstancerContainer
    {
        private static Dictionary<int, MonoBehaviour> variableInstancerTable = new Dictionary<int, MonoBehaviour>();
        private static Dictionary<int, AtomBaseVariable> variableTable = new Dictionary<int, AtomBaseVariable>();

        public static bool RegisterInstance<T, V>(AtomBaseVariableInstancer<T, V> instancer) where V : AtomBaseVariable<T>
        {
            if (variableInstancerTable == null)
            {
                variableInstancerTable = new Dictionary<int, MonoBehaviour>();
            }

            if (variableTable == null)
            {
                variableTable = new Dictionary<int, AtomBaseVariable>();
            }

            var key = instancer.id.GetHashCode();

            if (variableInstancerTable.ContainsKey(key))
            {
                Debug.LogWarning($"Key: {key} is duplicate");
                return false;
            }

            variableInstancerTable.Add(key, instancer);
            variableTable.Add(key, instancer.Variable);
            Debug.Log(instancer.name + " is registered");
            Debug.Log(typeof(T));
            Debug.Log(typeof(V));
            return true;
        }

        public static bool UnregisterInstancer<T, V>(AtomBaseVariableInstancer<T, V> instancer) where V : AtomBaseVariable<T>
        {
            if (variableInstancerTable == null)
            {
                variableInstancerTable = new Dictionary<int, MonoBehaviour>();
                Debug.LogWarning("Variable table was null");
                return false;
            }

            var key = instancer.id.GetHashCode();
            if (!variableInstancerTable.ContainsKey(key))
            {
                return false;
            }

            variableInstancerTable.Remove(key);

            return true;
        }

        public static MonoBehaviour GetInstancer(string id)
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                return default;
            }

            return instancer;
        }

        public static MonoBehaviour GetInstancer(string id, out bool isSuccess)
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                isSuccess = false;
                return default;
            }

            isSuccess = true;
            return instancer;
        }

        public static bool GetInstancer(string id, out MonoBehaviour atomInstancer)
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                atomInstancer = default;
                return false;
            }

            atomInstancer = instancer;
            return true;
        }

        public static AtomBaseVariableInstancer<T, V> GetInstancer<T, V>(string id) where V : AtomBaseVariable<T>
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                return default;
            }

            if (!typeof(AtomBaseVariableInstancer<T, V>).IsAssignableFrom(instancer.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(AtomBaseVariableInstancer<T, V>)} is not assignable from instance type {instancer.GetType()}");
                return default;
            }

            return instancer as AtomBaseVariableInstancer<T, V>;
        }

        public static AtomBaseVariableInstancer<T, V> GetInstancer<T, V>(string id, out bool isSuccess) where V : AtomBaseVariable<T>
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                isSuccess = false;
                return default;
            }

            if (!typeof(AtomBaseVariableInstancer<T, V>).IsAssignableFrom(instancer.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(AtomBaseVariableInstancer<T, V>)} is not assignable from instance type {instancer.GetType()}");
                isSuccess = false;
                return default;
            }

            isSuccess = true;
            return instancer as AtomBaseVariableInstancer<T, V>;
        }

        public static V GetVariable<T, V>(string id) where V : AtomBaseVariable<T>
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                return default;
            }

            if (!typeof(AtomBaseVariableInstancer<T, V>).IsAssignableFrom(instancer.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {instancer.GetType()}");
                return default;
            }

            return (instancer as AtomBaseVariableInstancer<T, V>).Variable;
        }
        public static V GetVariable<T, V>(string id, out bool isSuccess) where V : AtomBaseVariable<T>
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                isSuccess = false;
                return default;
            }

            if (!typeof(AtomBaseVariableInstancer<T, V>).IsAssignableFrom(instancer.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {instancer.GetType()}");
                isSuccess = false;
                return default;
            }
            isSuccess = true;
            return (instancer as AtomBaseVariableInstancer<T, V>).Variable;
        }

        public static AtomBaseVariable GetVariable(string id)
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                return default;
            }

            return variable;
        }

        public static AtomBaseVariable GetVariable(string id, out bool isSuccess)
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                isSuccess = false;
                return default;
            }

            isSuccess = true;
            return variable;
        }


        public static T GetVariable<T>(string id) where T : AtomBaseVariable
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                return default;
            }

            if (!typeof(T).IsAssignableFrom(variable.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {variable.GetType()}");
                return default;
            }

            return variable as T;
        }

        public static T GetVariable<T>(string id, out bool isSuccess) where T : AtomBaseVariable
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                isSuccess = false;
                return default;
            }

            if (!typeof(T).IsAssignableFrom(variable.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {variable.GetType()}");
                isSuccess = false;
                return default;
            }

            isSuccess = true;
            return variable as T;
        }


        public static bool GetVariable(string id, out AtomBaseVariable atomVariable)
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                atomVariable = default;
                return false;
            }

            atomVariable = variable;
            return true;
        }

        public static bool GetVariable<T>(string id, out T atomVariable) where T : AtomBaseVariable
        {
            var key = id.GetHashCode();
            if (!variableTable.TryGetValue(key, out var variable))
            {
                atomVariable = default;
                return false;
            }

            if (!typeof(T).IsAssignableFrom(variable.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {variable.GetType()}");
                atomVariable = default;
                return false;
            }

            atomVariable = variable as T;
            return true;
        }

        public static T GetValue<T, V>(string id) where V : AtomBaseVariable<T>
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                return default;
            }

            if (!typeof(AtomBaseVariableInstancer<T, V>).IsAssignableFrom(instancer.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {instancer.GetType()}");
                return default;
            }

            return (instancer as AtomBaseVariableInstancer<T, V>).Value;
        }

        public static T GetValue<T, V>(string id, out bool isSuccess) where V : AtomBaseVariable<T>
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                isSuccess = false;
                return default;
            }

            if (!typeof(AtomBaseVariableInstancer<T, V>).IsAssignableFrom(instancer.GetType()))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from variableType {instancer.GetType()}");
                isSuccess = false;
                return default;
            }
            isSuccess = true;
            return (instancer as AtomBaseVariableInstancer<T, V>).Value;
        }

        public static Type GetInstancerType(string id)
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                return default;
            }

            return instancer.GetType();
        }

        public static Type GetVariableType(string id)
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                return default;
            }
             
            return instancer.GetType().BaseType.GetGenericArguments()[0];
        }

        public static Type GetValueType(string id)
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                return default;
            }

            return instancer.GetType().BaseType.GetGenericArguments()[2];
        }

        /// <summary>
        /// Boxing is exist!. Should cache variable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetValue<T>(string id, T value)
        {
            var key = id.GetHashCode();
            if (!variableInstancerTable.TryGetValue(key, out var instancer))
            {
                return false;
            }
            if (!typeof(T).IsAssignableFrom(instancer.GetType().GetGenericArguments()[2]))
            {
                Debug.LogWarning($"Type: {typeof(T)} is not assignable from value type {instancer.GetType().GetGenericArguments()[2]}");
                return false;
            }

            (instancer as AtomBaseVariableInstancer<T, AtomBaseVariable<T>>).Value = value;
            return true;
        }
    }
}
#endif