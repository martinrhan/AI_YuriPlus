using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AI_YuriPlus.PatcherClasses_ShapeBody {
    static class Patcher_InitShapeBody_ChaControl {
        static bool IsModified = false;
        [HarmonyPatch(typeof(AIChara.ChaControl), nameof(AIChara.ChaControl.InitShapeBody))]
        [HarmonyPrefix]
        static bool Prefix(AIChara.ChaControl __instance) {
            if (ConfigValues.DisableBodyShapeChange) {
                if (__instance.isPlayer) {
                    __instance.isPlayer = false;
                    IsModified = true;
                }
            }
            return true;
        }
        [HarmonyPatch(typeof(AIChara.ChaControl), nameof(AIChara.ChaControl.InitShapeBody))]
        [HarmonyPostfix]
        static void Postfix(AIChara.ChaControl __instance) {
            if (IsModified) {
                __instance.isPlayer = !__instance.isPlayer;
                IsModified = false;
            }
        }
    }

    static class Patcher_SetShapeBodyValue_ChaControl {
        static bool IsModified = false;
        [HarmonyPatch(typeof(AIChara.ChaControl), nameof(AIChara.ChaControl.SetShapeBodyValue))]
        [HarmonyPrefix]
        static bool Prefix(AIChara.ChaControl __instance) {
            //Debug.Log("SetShapeBodyValuePrefix" + __instance.name);
            bool ChangeToIsPlayer = false;
            if (ConfigValues.DisableBodyShapeChange) {
                //Debug.Log("a");
            } else {
                Manager.HSceneManager hSceneManager = PatcherClasses_Main.Patcher_InitCoroutine_HScene.HSceneManager;
                if (hSceneManager == null) {
                    //Debug.Log("b");
                } else if (hSceneManager.females == null) {
                    //Debug.Log("c");
                } else if (!Manager.HSceneManager.isHScene) {
                    //Debug.Log("c");
                } else if (hSceneManager.females[1].ChaControl == __instance) {
                    //Debug.Log("d");
                    ChangeToIsPlayer = true;
                } else {
                    //Debug.Log("e");
                }
            }
            if (__instance.isPlayer != ChangeToIsPlayer) {
                //Debug.Log(ChangeToIsPlayer);
                __instance.isPlayer = ChangeToIsPlayer;
                IsModified = true;
            }
            return true;
        }
        [HarmonyPatch(typeof(AIChara.ChaControl), nameof(AIChara.ChaControl.SetShapeBodyValue))]
        [HarmonyPostfix]
        static void Postfix(AIChara.ChaControl __instance) {
            if (IsModified) {
                __instance.isPlayer = !__instance.isPlayer;
                IsModified = false;
            }
        }
    }

    static class Patcher_UpdateShapeBodyValueFromCustomInfo_ChaControl {
        static bool IsModified = false;
        [HarmonyPatch(typeof(AIChara.ChaControl), nameof(AIChara.ChaControl.UpdateShapeBodyValueFromCustomInfo))]
        [HarmonyPrefix]
        static bool Prefix(AIChara.ChaControl __instance) {
            if (ConfigValues.DisableBodyShapeChange) {
                if (__instance.isPlayer) {
                    __instance.isPlayer = false;
                    IsModified = true;
                }
            }
            return true;
        }
        [HarmonyPatch(typeof(AIChara.ChaControl), nameof(AIChara.ChaControl.UpdateShapeBodyValueFromCustomInfo))]
        [HarmonyPostfix]
        static void Postfix(AIChara.ChaControl __instance) {
            if (IsModified) {
                __instance.isPlayer = !__instance.isPlayer;
                IsModified = false;
            }
        }
    }


}
