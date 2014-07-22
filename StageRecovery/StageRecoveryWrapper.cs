﻿//This StageRecoveryWrapper.cs file is provided as-is and is not to be modified other than to update
//the namespace. Should further modification be made, no support will be provided by the author,
//magico13.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Change this to your mod's namespace!
namespace StageRecovery
{
    /////////////////////////////////////
    // DO NOT EDIT BEYOND THIS POINT!  //
    /////////////////////////////////////
    public class StageRecoveryManager
    {
        private static bool? available = null;
        private static Type SRType = null;
        private static object instance_;


        /* Call this to see if the addon is available. If this returns false, no additional API calls should be made! */
        public static bool StageRecoveryAvailable
        {
            get
            {
                if (available == null)
                {
                    SRType = AssemblyLoader.loadedAssemblies
                        .Select(a => a.assembly.GetExportedTypes())
                        .SelectMany(t => t)
                        .FirstOrDefault(t => t.FullName == "StageRecovery.APIManager");
                    available = SRType != null;
                }
                return (bool)available;
            }
        }

        /* The APIManager instance */
        private static object Instance
        {
            get
            {
                if (StageRecoveryAvailable && instance_ == null)
                {
                    instance_ = SRType.GetProperty("instance").GetValue(null, null);
                }

                return instance_;
            }
        }

        /***************/
        /* API methods */
        /***************/

        /* Adds a listener to the Recovery Success Event. When a vessel is recovered by StageRecovery the method will 
         * be invoked with the Vessel and a Dictionary containing part names and quantities */
        public static void AddRecoverySuccessEvent(Action<Vessel, Dictionary<string, int>> method)
        {
            object successList = SRType.GetProperty("RecoverySuccessEvent").GetValue(Instance, null);
            System.Reflection.MethodInfo addMethod = successList.GetType().GetMethod("Add");
            addMethod.Invoke(successList, new object[] { method });
        }

        /* Removes a listener from the Recovery Success Event */
        public static void RemoveRecoverySuccessEvent(Action<Vessel, Dictionary<string, int>> method)
        {
            object successList = SRType.GetProperty("RecoverySuccessEvent").GetValue(Instance, null);
            System.Reflection.MethodInfo removeMethod = successList.GetType().GetMethod("Remove");
            removeMethod.Invoke(successList, new object[] { method });
        }

        /* Adds a listener to the Recovery Failure Event. When a vessel fails to be recovered, the method will be invoked 
         * with the Vessel and a Dictionary containing part names and quantities */
        public static void AddRecoveryFailureEvent(Action<Vessel, Dictionary<string, int>> method)
        {
            object failList = SRType.GetProperty("RecoveryFailureEvent").GetValue(Instance, null);
            System.Reflection.MethodInfo addMethod = failList.GetType().GetMethod("Add");
            addMethod.Invoke(failList, new object[] { method });
        }

        /* Removes a listener from the Recovery Failure Event */
        public static void RemoveRecoveryFailureEvent(Action<Vessel, Dictionary<string, int>> method)
        {
            object failList = SRType.GetProperty("RecoveryFailureEvent").GetValue(Instance, null);
            System.Reflection.MethodInfo removeMethod = failList.GetType().GetMethod("Remove");
            removeMethod.Invoke(failList, new object[] { method });
        }

    }
}