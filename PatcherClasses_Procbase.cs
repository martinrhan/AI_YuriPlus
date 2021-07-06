using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AI_YuriPlus.PatcherClasses_Procbase {
    static class Patcher_Aibu {
        static void Modify(Aibu instance) {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            AIChara.ChaControl[] chaFemales = (AIChara.ChaControl[])(typeof(Aibu).GetField("chaFemales", bindingFlags).GetValue(instance));
            AIChara.ChaControl[] chaMales = (AIChara.ChaControl[])(typeof(Aibu).GetField("chaMales", bindingFlags).GetValue(instance));
            chaFemales[0] = Singleton<Manager.HSceneManager>.Instance.females[0].ChaControl;
            chaMales[0] = Singleton<Manager.HSceneManager>.Instance.females[1].ChaControl;
            //Debug.Log($"{chaFemales[0].GetShapeBodyValue(0)},{chaMales[0].GetShapeBodyValue(0)}");
        }
        [HarmonyPatch(typeof(Aibu), "setPlay")]
        [HarmonyPrefix]
        static bool Prefix0(Aibu __instance) {
            Modify(__instance);
            return true;
        }
        [HarmonyPatch(typeof(Aibu), nameof(Aibu.setAnimationParamater))]
        [HarmonyPrefix]
        static bool Prefix1(Aibu __instance) {
            Modify(__instance);
            return true;
        }
    }

    static class Patcher_Les {
        static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        static FieldInfo FieldInfo_chaFemales = (typeof(Les).GetField("chaFemales", bindingFlags));
        static AIChara.ChaControl[] chaFemales;
        static void Modify(Les instance) {
            chaFemales = (AIChara.ChaControl[])FieldInfo_chaFemales.GetValue(instance);
            chaFemales[0] = Singleton<Manager.HSceneManager>.Instance.females[0].ChaControl;
            chaFemales[1] = Singleton<Manager.HSceneManager>.Instance.females[1].ChaControl;
            //Debug.Log($"{chaFemales[0].GetShapeBodyValue(0)},{chaFemales[1].GetShapeBodyValue(0)}");
        }
        [HarmonyPatch(typeof(Les), "setPlay")]
        [HarmonyPrefix]
        static bool Prefix0(Les __instance) {
            Modify(__instance);
            return true;
        }
        [HarmonyPatch(typeof(Les), nameof(Les.setAnimationParamater))]
        [HarmonyPrefix]
        static bool Prefix1(Les __instance) {
            Modify(__instance);
            for (int i = 0; i < chaFemales.Length; i++) {
                if (chaFemales[i].isPlayer) {
                    chaFemales[i].isPlayer = false;
                    ModifiedChaFemaleIndex = i;
                    break;
                }
            }
                   
            if (Tester.instance == null) {
                Tester.Initialize();
                Tester.instance.a = chaFemales[0].GetShapeBodyValue(0);
                Tester.instance.b = chaFemales[1].GetShapeBodyValue(0);
            }
            chaFemales[0].SetShapeBodyValue(0, Tester.instance.a);
            chaFemales[1].SetShapeBodyValue(0, Tester.instance.b);

            return true;
        }
        static int ModifiedChaFemaleIndex = -1;
        [HarmonyPatch(typeof(Les), nameof(Les.setAnimationParamater))]
        [HarmonyPostfix]
        static void Postfix1(Les __instance) {
            chaFemales[ModifiedChaFemaleIndex].isPlayer = true;
            ModifiedChaFemaleIndex = -1;
        }
    }

    static class Patcher_Masturbation {
        static void Modify(Masturbation instance) {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            AIChara.ChaControl[] chaFemales = (AIChara.ChaControl[])(typeof(Masturbation).GetField("chaFemales", bindingFlags).GetValue(instance));
            chaFemales[0] = Singleton<Manager.HSceneManager>.Instance.females[0].ChaControl;
            //Debug.Log(chaFemales[0].GetShapeBodyValue(0));
        }
        [HarmonyPatch(typeof(Masturbation), "setPlay")]
        [HarmonyPrefix]
        static bool Prefix0(Masturbation __instance) {
            Modify(__instance);
            return true;
        }
        [HarmonyPatch(typeof(Masturbation), nameof(Masturbation.setAnimationParamater))]
        [HarmonyPrefix]
        static bool Prefix1(Masturbation __instance) {
            Modify(__instance);
            return true;
        }
    }

    static class Patcher_Peeping {
        static void Modify(Peeping instance) {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            AIChara.ChaControl[] chaFemales = (AIChara.ChaControl[])(typeof(Peeping).GetField("chaFemales", bindingFlags).GetValue(instance));
            AIChara.ChaControl[] chaMales = (AIChara.ChaControl[])(typeof(Peeping).GetField("chaMales", bindingFlags).GetValue(instance));
            chaFemales[0] = Singleton<Manager.HSceneManager>.Instance.females[0].ChaControl;
            chaMales[0] = Singleton<Manager.HSceneManager>.Instance.females[1].ChaControl;
        }
        [HarmonyPatch(typeof(Peeping), nameof(Peeping.setAnimationParamater))]
        [HarmonyPrefix]
        static bool Prefix1(Peeping __instance) {
            Modify(__instance);
            return true;
        }
    }

    static class Patcher_Sonyu {
        static void Modify(Sonyu instance) {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            AIChara.ChaControl[] chaFemales = (AIChara.ChaControl[])(typeof(Sonyu).GetField("chaFemales", bindingFlags).GetValue(instance));
            AIChara.ChaControl[] chaMales = (AIChara.ChaControl[])(typeof(Sonyu).GetField("chaMales", bindingFlags).GetValue(instance));
            chaFemales[0] = Singleton<Manager.HSceneManager>.Instance.females[0].ChaControl;
            chaMales[0] = Singleton<Manager.HSceneManager>.Instance.females[1].ChaControl;
        }
        [HarmonyPatch(typeof(Sonyu), "setPlay")]
        [HarmonyPrefix]
        static bool Prefix0(Sonyu __instance) {
            Modify(__instance);
            return true;
        }
        [HarmonyPatch(typeof(Sonyu), nameof(Sonyu.setAnimationParamater))]
        [HarmonyPrefix]
        static bool Prefix1(Sonyu __instance) {
            Modify(__instance);
            return true;
        }
    }

    static class Patcher_Spnking {
        static void Modify(Spnking instance) {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            AIChara.ChaControl[] chaFemales = (AIChara.ChaControl[])(typeof(Spnking).GetField("chaFemales", bindingFlags).GetValue(instance));
            AIChara.ChaControl[] chaMales = (AIChara.ChaControl[])(typeof(Spnking).GetField("chaMales", bindingFlags).GetValue(instance));
            chaFemales[0] = Singleton<Manager.HSceneManager>.Instance.females[0].ChaControl;
            chaMales[0] = Singleton<Manager.HSceneManager>.Instance.females[1].ChaControl;
        }
        [HarmonyPatch(typeof(Spnking), "setPlay")]
        [HarmonyPrefix]
        static bool Prefix0(Spnking __instance) {
            Modify(__instance);
            return true;
        }
        [HarmonyPatch(typeof(Spnking), nameof(Spnking.setAnimationParamater))]
        [HarmonyPrefix]
        static bool Prefix1(Spnking __instance) {
            Modify(__instance);
            return true;
        }
    }
}
