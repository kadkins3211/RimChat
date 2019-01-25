using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace RimChat
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            //FileLog.Log("Starting harmony patcher..");
            // Set our instance of harmony
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.madlad.rimchat");
            //harmony.PatchAll(Assembly.GetExecutingAssembly());

            //FileLog.Log("Patching WindowStack Add..");
            // Prefix patch the window stack with 
            //harmony.Patch(typeof(WindowStack).GetMethod("Add"),
            //    new HarmonyMethod(typeof(HarmonyPatches).GetMethod("PrefixWarningLog")));

            //FileLog.Log("Patching DoWindowContents for Page_ModsConfig..");
            // Postfix the mods config page with a testbutton that logs 
            //harmony.Patch(typeof(Page_ModsConfig).GetMethod("DoWindowContents"),
            //    new HarmonyMethod(null),
            //    new HarmonyMethod(typeof(HarmonyPatches).GetMethod("PostfixPageModsConfig")));
        }

        // Adds a log every time a the Window.Add method is called?
        public static void PrefixWarningLog(Window window)
        {
            Log.Warning("This is the patched window: " + window);
        }

        // Postfix add a button to the mods page
        public static void PostfixPageModsConfig(Rect rect, Page_ModsConfig __instance)
        {
            Vector2 BottomButSize = new Vector2(150f, 38f);
            float num = 10f;
            Rect rect4 = new Rect(rect.x + rect.width / 2f - BottomButSize.x / 2f, num, BottomButSize.x, BottomButSize.y);
            if(Widgets.ButtonText(rect4, "RimChat.MC.Page.Button.Test".Translate(), true, false, true))
            {
                Log.Warning("Clicked our test postfix button!");
            }
        }
    }
}
