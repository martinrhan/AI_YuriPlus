using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Collections;

namespace AI_YuriPlus.PatcherClasses_Main {
    static class Patcher_ChangeAnimation_HScene {
        internal static bool IsModified_cha = false;
        internal static HScene.AnimationListInfo info;//Do not attempt do use Singleton<HScene>.Instance.ctrlFlag.nowAnimationInfo instead.

        static List<float> RecordedShapeBodyValue = new List<float>();

        [HarmonyPatch(typeof(HScene), "ChangeAnimation")]
        [HarmonyPrefix]
        static bool Prefix(HScene.AnimationListInfo _info, ref AIChara.ChaControl[] ___chaFemales, ref AIChara.ChaControl[] ___chaMales) {
            info = _info;
            if (!IsModified_cha) {
                if (_info.nPromiscuity < 2) {
                    Modify();
                }
            } else if (_info.nPromiscuity == 2) {
                ModifyBack();
            }
            return true;
        }

        [HarmonyPatch(typeof(HScene), "ChangeAnimation")]
        [HarmonyPostfix]
        static void Postfix() {
            //Debug.Log("ChangeAnimationPostfix");
            Manager.HSceneManager hSceneManager = Singleton<Manager.HSceneManager>.Instance;
            hSceneManager.females[0].ChaControl.SetShapeBodyValue(0, hSceneManager.females[0].ChaControl.GetShapeBodyValue(0));
            hSceneManager.females[1].ChaControl.SetShapeBodyValue(0, hSceneManager.females[1].ChaControl.GetShapeBodyValue(0));
        }

        private static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        internal static void Modify() {
            HScene hScene = Singleton<HScene>.Instance;
            Manager.HSceneManager hSceneManager = Singleton<Manager.HSceneManager>.Instance;
            AIChara.ChaControl[] chaFemales = hScene.GetFemales();
            AIChara.ChaControl[] chaMales = hScene.GetMales();
            chaMales[0] = hSceneManager.females[1].ChaControl;
            chaFemales[1] = null;
            hSceneManager.male = hSceneManager.Player;
            List<System.Tuple<int, int, MotionIK>> lstMotionIK = (List<System.Tuple<int, int, MotionIK>>)(typeof(HScene).GetField("lstMotionIK", bindingFlags).GetValue(hScene));
            lstMotionIK.Clear();
            IsModified_cha = true;
        }
        internal static void ModifyBack() {
            HScene hScene = Singleton<HScene>.Instance;
            Manager.HSceneManager hSceneManager = Singleton<Manager.HSceneManager>.Instance;
            AIChara.ChaControl[] chaFemales = (AIChara.ChaControl[])(typeof(HScene).GetField("chaFemales", bindingFlags).GetValue(hScene));
            AIChara.ChaControl[] chaMales = (AIChara.ChaControl[])(typeof(HScene).GetField("chaMales", bindingFlags).GetValue(hScene));
            chaFemales[1] = hSceneManager.females[1].ChaControl;
            chaMales[0] = null;
            hSceneManager.male = null;
            List<System.Tuple<int, int, MotionIK>> lstMotionIK = (List<System.Tuple<int, int, MotionIK>>)(typeof(HScene).GetField("lstMotionIK", bindingFlags).GetValue(hScene));
            lstMotionIK.Clear();
            IsModified_cha = false;
        }

        /*
        [HarmonyPatch(typeof(HScene), "ChangeAnimation")]
        [HarmonyPostfix]
        static void Postfix(HScene __instance) {
            try {
                List<HScene.AnimationListInfo>[] lstAnimInfo = Singleton<Manager.Resources>.Instance.HSceneTable.lstAnimInfo;
                UnityGameObjectVisualizer.PublicMethods.MarkedObject.Clear();
                foreach (List<HScene.AnimationListInfo> list in lstAnimInfo) {
                    UnityGameObjectVisualizer.PublicMethods.MarkedObject.Add(list);
                }
                UnityGameObjectVisualizer.PublicMethods.ShowMarkedObject();
            } catch (Exception e) {
                Debug.Log(e.Message + e.TargetSite);
            }
        }*/
    }

    static class Patcher_SetPostion_HScene {
        [HarmonyPatch(typeof(HScene), "SetPosition", new Type[] { typeof(Transform), typeof(Vector3), typeof(Vector3), typeof(bool) })]
        [HarmonyPrefix]
        static bool Prefix0(ref Transform[] ___chaMalesTrans, ref Transform[] ___chaFemalesTrans) {
            ___chaMalesTrans[0] = ___chaFemalesTrans[1];
            return true;
        }

        [HarmonyPatch(typeof(HScene), "SetPosition", new Type[] { typeof(Vector3), typeof(Quaternion), typeof(Vector3), typeof(Vector3), typeof(bool) })]
        [HarmonyPrefix]
        static bool Prefix1(ref Transform[] ___chaMalesTrans, ref Transform[] ___chaFemalesTrans) {
            ___chaMalesTrans[0] = ___chaFemalesTrans[1];
            return true;
        }
    }

    static class Patcher_ChangeAnim_HPointCtrl {
        private static List<UnityEx.ValueTuple<Manager.HSceneManager.HEvent, int, HScene.StartMotion>> SavedValue_startList = new List<UnityEx.ValueTuple<Manager.HSceneManager.HEvent, int, HScene.StartMotion>>();
        private static List<UnityEx.ValueTuple<Manager.HSceneManager.HEvent, int, HScene.StartMotion>> startList;
        static bool IsModified = false;

        [HarmonyPatch(typeof(HPointCtrl), "ChangeAnim")]
        [HarmonyPrefix]
        static bool Prefix(ref int ___playerSex, Manager.HSceneManager ___hSceneManager,
            ref List<UnityEx.ValueTuple<Manager.HSceneManager.HEvent, int, HScene.StartMotion>> ___startList,
            ref List<UnityEx.ValueTuple<Manager.HSceneManager.HEvent, int, HScene.StartMotion>> ___startListM) {
            //Debug.Log("Prefix_ChangeAnim_HPointCtrl");
            if (___playerSex == 1 && !___hSceneManager.bFutanari) {
                //Debug.Log("Is non futa female");
                if (___hSceneManager.bMerchant) {
                    startList = ___startListM;
                } else {
                    startList = ___startList;
                }
                SavedValue_startList.Clear();
                List<UnityEx.ValueTuple<Manager.HSceneManager.HEvent, int, HScene.StartMotion>> mediumList = new List<UnityEx.ValueTuple<Manager.HSceneManager.HEvent, int, HScene.StartMotion>>();
                foreach (UnityEx.ValueTuple<Manager.HSceneManager.HEvent, int, HScene.StartMotion> thing in startList) {
                    SavedValue_startList.Add(thing);
                    if (thing.Item3.mode == 0 || thing.Item3.mode == 3 || thing.Item3.mode == 4) {//爱抚，百合，特殊
                        mediumList.Add(thing);
                    }
                }
                startList.Clear();
                foreach (UnityEx.ValueTuple<Manager.HSceneManager.HEvent, int, HScene.StartMotion> thing in mediumList) {
                    startList.Add(thing);
                }
                ___playerSex = 0;
                IsModified = true;
            }
            return true;
        }
        [HarmonyPatch(typeof(HPointCtrl), "ChangeAnim")]
        [HarmonyPostfix]
        static void Postfix(ref int ___playerSex) {
            Debug.Log("Postfix, " + ___playerSex);
            if (IsModified) {
                startList.Clear();
                foreach (UnityEx.ValueTuple<Manager.HSceneManager.HEvent, int, HScene.StartMotion> thing in SavedValue_startList) {
                    startList.Add(thing);
                }
                ___playerSex = 1;
                IsModified = false;
            }
        }
    }

    static class Patcher_Start_HSceneSpriteCategories {
        [HarmonyPatch(typeof(HSceneSpriteCategories), "Start")]
        [HarmonyPrefix]
        static bool Prefix(ref GameObject[] ___SubCategory) {
            if (!Swap.CheckIconExist()) {
                Swap.CreateIcon();
                GameObject[] SubCategory = new GameObject[___SubCategory.Length + 1];
                ___SubCategory.CopyTo(SubCategory, 0);
                SubCategory[___SubCategory.Length] = Swap.IconGameObject;
                ___SubCategory = SubCategory;
            }
            return true;
        }
    }

    static class Patcher_EndProc_HScene {
        [HarmonyPatch(typeof(HScene), "EndProc")]
        [HarmonyPrefix]
        static bool Prefix(AIChara.ChaControl[] ___chaFemales) {
            InternalStaticFuntions.ModifyEverythingBack();
            return true;
        }
    }

    static class Patcher_InitCoroutine_HScene {
        static internal Manager.HSceneManager HSceneManager;

        [HarmonyPatch(typeof(HScene), nameof(HScene.InitCoroutine))]
        [HarmonyPostfix]
        static void Postfix(ref IEnumerator __result, HScene __instance, AIChara.ChaControl[] ___chaMales, AIChara.ChaControl[] ___chaFemales) {
            HSceneManager = Singleton<Manager.HSceneManager>.Instance;
            __result = ModifyIEnumerator(__result, __instance, ___chaMales, ___chaFemales);
        }

        static IEnumerator ModifyIEnumerator(IEnumerator enumerator, HScene hScene, AIChara.ChaControl[] chaMales, AIChara.ChaControl[] chaFemales) {
            yield return enumerator;
            Manager.HSceneManager hSceneManager = Singleton<Manager.HSceneManager>.Instance;
            hScene.ctrlYureMale.chaMale = hSceneManager.females[1].ChaControl;
            hScene.ctrlYureMale.MaleID = 0;
            hScene.ctrlEyeNeckMale[0].Init(hSceneManager.females[1].ChaControl, 0);
            hScene.ctrlEyeNeckMale[0].SetPartner(hSceneManager.females[0].ChaControl.objBodyBone, null, null);
            hScene.ctrlHitObjectMales[0] = hScene.ctrlHitObjectFemales[1];
            PatcherClasses_ClothAndAccessory.ClothAndAccessory.InitialApply();
        }
    }

    static class Patcher_HitObjInit_HitObjectCtrl {
        [HarmonyPatch(typeof(HitObjectCtrl), nameof(HitObjectCtrl.HitObjInit))]
        [HarmonyPrefix]
        static bool Prefix(ref int Sex) {
            Sex = 1;
            return true;
        }
    }

    static class Patcher_BeforeWait_HScene {
        [HarmonyPatch(typeof(HScene),"BeforeWait")]
        [HarmonyPrefix]
        static bool Prefix() {
            return false;
        }
    }

    static class Patcher_Init_HSceneSprite {
        static bool IsModified_bMerchant = false;

        [HarmonyPatch(typeof(HSceneSprite),nameof(HSceneSprite.PopupCommands))]
        [HarmonyPrefix]
        static bool Prefix(HSceneFlagCtrl ___ctrlFlag) {
            ___ctrlFlag.AddParam(7, 1);
            Singleton<HScene>.Instance.SetStartAnimationInfo(Manager.HSceneManager.HEvent.Normal, 4);
            Singleton<HSceneFlagCtrl>.Instance.BeforeHWait = false;
            return false;
        }
    }   
}
