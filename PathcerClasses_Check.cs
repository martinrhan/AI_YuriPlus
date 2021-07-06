using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_YuriPlus {
    static class Patcher_CheckMotionLimit_HScene {
        static int nPromiscuity_Saved;
        static bool IsModified_nPromiscuity = false;
        static bool IsModified_bMerchantMotion = false;
        static int nHentai_Saved;
        static bool IsModified_nHentai = false;

        [HarmonyPatch(typeof(HSceneSprite), "CheckMotionLimit")]
        [HarmonyPrefix]
        static bool Prefix(int ___PlayerSex, HSceneFlagCtrl ___ctrlFlag, ref HScene.AnimationListInfo lstAnimInfo) {
            string[] splitedStrings = lstAnimInfo.assetpathFemale.Split('/');
            string lastString = splitedStrings[splitedStrings.Length - 1];//
            if (___PlayerSex == 1 && !___ctrlFlag.bFutanari && (lastString == "aibu.unity3d" || lastString == "tokushu.unity3d")) {
                if (lastString == "tokushu.unity3d" && lstAnimInfo.id == 0) {
                    return true;
                }
                nPromiscuity_Saved = lstAnimInfo.nPromiscuity;
                lstAnimInfo.nPromiscuity = 2;
                IsModified_nPromiscuity = true;
                if (ConfigValues.DisableMotionLimit) {
                    if (Singleton<Manager.HSceneManager>.Instance.bMerchant) {
                        if (!lstAnimInfo.bMerchantMotion) {
                            lstAnimInfo.bMerchantMotion = true;
                            IsModified_bMerchantMotion = true;
                        }
                    } else {
                        nHentai_Saved = lstAnimInfo.nHentai;
                        lstAnimInfo.nHentai = 0;
                        IsModified_nHentai = true;
                    }
                }
            }
            return true;
        }

        [HarmonyPatch(typeof(HSceneSprite), "CheckMotionLimit")]
        [HarmonyPostfix]
        static void Postfix(ref HScene.AnimationListInfo lstAnimInfo) {
            if (IsModified_nPromiscuity) {
                lstAnimInfo.nPromiscuity = nPromiscuity_Saved;
                IsModified_nPromiscuity = false;
            }
            if (IsModified_bMerchantMotion) {
                lstAnimInfo.bMerchantMotion = false;
                IsModified_bMerchantMotion = false;
            }
            if (IsModified_nHentai) {
                lstAnimInfo.nHentai = nHentai_Saved;
                IsModified_nHentai = false;
            }
        }
    }

    static class Patcher_CheckMotionLimit_HPointCtrl {
        static int nPromiscuity_Saved;
        static bool IsModified_nPromiscuity = false;
        static bool IsModified_bMerchantMotion = false;
        static int nHentai_Saved;
        static bool IsModified_nHentai = false;

        [HarmonyPatch(typeof(HPointCtrl), "CheckMotionLimit", new Type[] { typeof(int), typeof(HScene.AnimationListInfo) })]
        [HarmonyPrefix]
        static bool Prefix(int ___playerSex, HSceneFlagCtrl ___ctrlFlag, ref HScene.AnimationListInfo lstAnimInfo) {
            string[] splitedStrings = lstAnimInfo.assetpathFemale.Split('/');
            string lastString = splitedStrings[splitedStrings.Length - 1];// 
            if (___playerSex == 1 && !___ctrlFlag.bFutanari && (lastString == "aibu.unity3d" || lastString == "tokushu.unity3d")) {
                if (lastString == "tokushu.unity3d" && lstAnimInfo.id == 0) {
                    return true;
                }
                nPromiscuity_Saved = lstAnimInfo.nPromiscuity;
                lstAnimInfo.nPromiscuity = 2;
                IsModified_nPromiscuity = true;
                if (ConfigValues.DisableMotionLimit) {
                    if (Singleton<Manager.HSceneManager>.Instance.bMerchant) {
                        if (!lstAnimInfo.bMerchantMotion) {
                            lstAnimInfo.bMerchantMotion = true;
                            IsModified_bMerchantMotion = true;
                        }
                    } else {
                        nHentai_Saved = lstAnimInfo.nHentai;
                        lstAnimInfo.nHentai = 0;
                        IsModified_nHentai = true;
                    }
                }
            }
            return true;
        }

        [HarmonyPatch(typeof(HPointCtrl), "CheckMotionLimit", new Type[] { typeof(int), typeof(HScene.AnimationListInfo) })]
        [HarmonyPostfix]
        static void Postfix(ref HScene.AnimationListInfo lstAnimInfo) {
            if (IsModified_nPromiscuity) {
                lstAnimInfo.nPromiscuity = nPromiscuity_Saved;
                IsModified_nPromiscuity = false;
            }
            if (IsModified_bMerchantMotion) {
                lstAnimInfo.bMerchantMotion = false;
                IsModified_bMerchantMotion = false;
            }
            if (IsModified_nHentai) {
                lstAnimInfo.nHentai = nHentai_Saved;
                IsModified_nHentai = false;
            }
        }
    }

    static class Patcher_CheckAutoMotionLimit_HSceneSprite {
        static int nPromiscuity_Saved;
        static bool IsModified_nPromiscuity = false;
        static bool IsModified_bMerchantMotion = false;
        static int nHentai_Saved;
        static bool IsModified_nHentai = false;

        [HarmonyPatch(typeof(HSceneSprite), "CheckAutoMotionLimit")]
        [HarmonyPrefix]
        static bool Prefix(int ___PlayerSex, HSceneFlagCtrl ___ctrlFlag, ref HScene.AnimationListInfo lstAnimInfo) {
            string[] splitedStrings = lstAnimInfo.assetpathFemale.Split('/');
            string lastString = splitedStrings[splitedStrings.Length - 1];// 
            if (___PlayerSex == 1 && !___ctrlFlag.bFutanari && (lastString == "aibu.unity3d" || lastString == "tokushu.unity3d")) {
                if (lastString == "tokushu.unity3d" && lstAnimInfo.id == 0) {
                    return true;
                }
                nPromiscuity_Saved = lstAnimInfo.nPromiscuity;
                lstAnimInfo.nPromiscuity = 2;
                IsModified_nPromiscuity = true;
                if (ConfigValues.DisableMotionLimit) {
                    if (Singleton<Manager.HSceneManager>.Instance.bMerchant) {
                        if (!lstAnimInfo.bMerchantMotion) {
                            lstAnimInfo.bMerchantMotion = true;
                            IsModified_bMerchantMotion = true;
                        }
                    } else {
                        nHentai_Saved = lstAnimInfo.nHentai;
                        lstAnimInfo.nHentai = 0;
                        IsModified_nHentai = true;
                    }
                }
            }
            return true;
        }

        [HarmonyPatch(typeof(HSceneSprite), "CheckAutoMotionLimit")]
        [HarmonyPostfix]
        static void Postfix(ref HScene.AnimationListInfo lstAnimInfo) {
            if (IsModified_nPromiscuity) {
                lstAnimInfo.nPromiscuity = nPromiscuity_Saved;
                IsModified_nPromiscuity = false;
            }
            if (IsModified_bMerchantMotion) {
                lstAnimInfo.bMerchantMotion = false;
                IsModified_bMerchantMotion = false;
            }
            if (IsModified_nHentai) {
                lstAnimInfo.nHentai = nHentai_Saved;
                IsModified_nHentai = false;
            }
        }
    }

    static class Patcher_CheckPlace_HPointCtrl {
        [HarmonyPatch(typeof(HPointCtrl), nameof(HPointCtrl.CheckPlace))]
        [HarmonyPrefix]
        static bool Prefix(int mode, ref bool __result) {
            if (mode == 0) {
                __result = true;
                return false;
            } else {
                return true;
            }
        }
    }
}
