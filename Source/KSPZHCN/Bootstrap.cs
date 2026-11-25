using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using KSP;

namespace KSPZHCN
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class Bootstrap : MonoBehaviour
    {
        static bool started;

        void Awake()
        {
            if (started) return;
            started = true;
            try
            {
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string root = Path.GetFullPath(Path.Combine(dir, ".."));
                string flag = Path.Combine(Path.Combine(root, "DllLocalization"), "disabled.txt");
                if (File.Exists(flag))
                {
                    Util.Log("disabled by DllLocalization\\disabled.txt");
                    return;
                }
                string dict = Path.Combine(Path.Combine(root, "DllLocalization"), "zh-cn.txt");
                Translator.Load(dict);
                if (Translator.Count == 0)
                {
                    Util.Warn("dictionary is empty or missing, patches skipped");
                    return;
                }
                Patches.Apply(new Harmony("com.kspzhcn.extended"));
            }
            catch (Exception e)
            {
                Util.Warn("bootstrap failed: " + e);
            }
        }
    }
}
