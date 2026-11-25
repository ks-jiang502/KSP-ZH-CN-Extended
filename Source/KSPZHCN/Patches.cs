using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace KSPZHCN
{
    /// <summary>Harmony prefixes that translate UI strings before they are rendered.</summary>
    public static class Patches
    {
        static int ok;
        static int fail;
        static bool applied;
        static readonly List<string> errors = new List<string>();

        public static void Apply(Harmony harmony)
        {
            if (applied) return;
            applied = true;
            ok = 0;
            fail = 0;
            errors.Clear();

            PatchGui(harmony);
            PatchGuiLayout(harmony);
            PatchGuiContent(harmony);
            PatchText(harmony);
            PatchScreenMessages(harmony);
            PatchDialogs(harmony);

            Util.Log(string.Format("harmony patches: ok={0} fail={1}", ok, fail));
            for (int i = 0; i < errors.Count && i < 30; i++) Util.Warn("patch: " + errors[i]);
        }

        // ---------------- GUI ----------------

        static void PatchGui(Harmony h)
        {
            const BindingFlags F = BindingFlags.Public | BindingFlags.Static;
            Type g = typeof(GUI);
            TryMethod(h, g, "Label", new[] { typeof(Rect), typeof(string) }, "P0", F);
            TryMethod(h, g, "Label", new[] { typeof(Rect), typeof(string), typeof(GUIStyle) }, "P0", F);
            TryMethod(h, g, "Box", new[] { typeof(Rect), typeof(string) }, "P0", F);
            TryMethod(h, g, "Box", new[] { typeof(Rect), typeof(string), typeof(GUIStyle) }, "P0", F);
            TryMethod(h, g, "Button", new[] { typeof(Rect), typeof(string) }, "P0", F);
            TryMethod(h, g, "Button", new[] { typeof(Rect), typeof(string), typeof(GUIStyle) }, "P0", F);
            TryMethod(h, g, "RepeatButton", new[] { typeof(Rect), typeof(string) }, "P0", F);
            TryMethod(h, g, "RepeatButton", new[] { typeof(Rect), typeof(string), typeof(GUIStyle) }, "P0", F);
            TryMethod(h, g, "TextField", new[] { typeof(Rect), typeof(string) }, "P0", F);
            TryMethod(h, g, "TextField", new[] { typeof(Rect), typeof(string), typeof(int) }, "P0", F);
            TryMethod(h, g, "TextField", new[] { typeof(Rect), typeof(string), typeof(GUIStyle) }, "P0", F);
            TryMethod(h, g, "TextArea", new[] { typeof(Rect), typeof(string) }, "P0", F);
            TryMethod(h, g, "TextArea", new[] { typeof(Rect), typeof(string), typeof(int) }, "P0", F);
            TryMethod(h, g, "TextArea", new[] { typeof(Rect), typeof(string), typeof(GUIStyle) }, "P0", F);
            TryMethod(h, g, "Toggle", new[] { typeof(Rect), typeof(bool), typeof(string) }, "P2", F);
            TryMethod(h, g, "Toggle", new[] { typeof(Rect), typeof(bool), typeof(string), typeof(GUIStyle) }, "P2", F);
            TryMethod(h, g, "BeginGroup", new[] { typeof(Rect), typeof(string) }, "P1", F);
            TryMethod(h, g, "BeginGroup", new[] { typeof(Rect), typeof(string), typeof(GUIStyle) }, "P1", F);
            TryMethod(h, g, "Window", new[] { typeof(int), typeof(Rect), typeof(GUI.WindowFunction), typeof(string) }, "P3", F);
            TryMethod(h, g, "Window", new[] { typeof(int), typeof(Rect), typeof(GUI.WindowFunction), typeof(string), typeof(GUIStyle) }, "P3", F);
            TryMethod(h, g, "ModalWindow", new[] { typeof(int), typeof(Rect), typeof(GUI.WindowFunction), typeof(string) }, "P3", F);
            TryMethod(h, g, "ModalWindow", new[] { typeof(int), typeof(Rect), typeof(GUI.WindowFunction), typeof(string), typeof(GUIStyle) }, "P3", F);
            PropertySetter(h, g, "tooltip", "P0");
        }

        static void PatchGuiLayout(Harmony h)
        {
            const BindingFlags F = BindingFlags.Public | BindingFlags.Static;
            Type g = typeof(GUILayout);
            string[] simple = { "Label", "Box", "Button", "RepeatButton", "TextField", "TextArea" };
            foreach (string name in simple)
            {
                TryMethod(h, g, name, new[] { typeof(string), typeof(GUILayoutOption[]) }, "P0", F);
                TryMethod(h, g, name, new[] { typeof(string), typeof(GUIStyle), typeof(GUILayoutOption[]) }, "P0", F);
            }
            TryMethod(h, g, "TextField", new[] { typeof(string), typeof(int), typeof(GUILayoutOption[]) }, "P0", F);
            TryMethod(h, g, "TextArea", new[] { typeof(string), typeof(int), typeof(GUILayoutOption[]) }, "P0", F);
            TryMethod(h, g, "Toggle", new[] { typeof(bool), typeof(string), typeof(GUILayoutOption[]) }, "P1", F);
            TryMethod(h, g, "Toggle", new[] { typeof(bool), typeof(string), typeof(GUIStyle), typeof(GUILayoutOption[]) }, "P1", F);
            TryMethod(h, g, "BeginHorizontal", new[] { typeof(string), typeof(GUIStyle), typeof(GUILayoutOption[]) }, "P0", F);
            TryMethod(h, g, "BeginVertical", new[] { typeof(string), typeof(GUIStyle), typeof(GUILayoutOption[]) }, "P0", F);
            TryMethod(h, g, "BeginArea", new[] { typeof(Rect), typeof(string) }, "P1", F);
            TryMethod(h, g, "BeginArea", new[] { typeof(Rect), typeof(string), typeof(GUIStyle) }, "P1", F);
            TryMethod(h, g, "Window", new[] { typeof(int), typeof(Rect), typeof(GUI.WindowFunction), typeof(string), typeof(GUILayoutOption[]) }, "P3", F);
            TryMethod(h, g, "Window", new[] { typeof(int), typeof(Rect), typeof(GUI.WindowFunction), typeof(string), typeof(GUIStyle), typeof(GUILayoutOption[]) }, "P3", F);
        }

        static void PatchGuiContent(Harmony h)
        {
            TryCtor(h, typeof(GUIContent), new[] { typeof(string) }, "P0");
            TryCtor(h, typeof(GUIContent), new[] { typeof(string), typeof(Texture) }, "P0");
            TryCtor(h, typeof(GUIContent), new[] { typeof(string), typeof(string) }, "P0");
            TryCtor(h, typeof(GUIContent), new[] { typeof(string), typeof(Texture), typeof(string) }, "P0");
            TryMethod(h, typeof(GUIContent), "Temp",
                new[] { typeof(string) },
                "P0", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            PropertySetter(h, typeof(GUIContent), "text", "P0");
            PropertySetter(h, typeof(GUIContent), "tooltip", "P0");
        }

        static void PatchText(Harmony h)
        {
            PropertySetter(h, typeof(Text), "text", "P0");
        }

        // ---------------- reflection-based (KSP types) ----------------

        static void PatchScreenMessages(Harmony h)
        {
            Type t = FindType("ScreenMessages");
            if (t == null) { Fail("ScreenMessages type not found"); return; }
            foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                if (m.Name != "PostScreenMessage") continue;
                ParameterInfo[] ps = m.GetParameters();
                if (ps.Length == 0 || ps[0].ParameterType != typeof(string)) continue;
                PatchMethod(h, m, "P0", "ScreenMessages." + m.Name);
            }
        }

        static void PatchDialogs(Harmony h)
        {
            Type label = FindType("DialogGUILabel");
            if (label != null)
            {
                foreach (ConstructorInfo c in label.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    ParameterInfo[] ps = c.GetParameters();
                    if (ps.Length > 0 && ps[0].ParameterType == typeof(string))
                        PatchCtor(h, c, "P0", "DialogGUILabel..ctor");
                }
            }
            Type button = FindType("DialogGUIButton");
            if (button != null)
            {
                foreach (ConstructorInfo c in button.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    ParameterInfo[] ps = c.GetParameters();
                    if (ps.Length > 0 && ps[0].ParameterType == typeof(string))
                        PatchCtor(h, c, "P0", "DialogGUIButton..ctor");
                }
            }
            Type multi = FindType("MultiOptionDialog");
            if (multi != null)
            {
                foreach (ConstructorInfo c in multi.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    ParameterInfo[] ps = c.GetParameters();
                    if (ps.Length < 3) continue;
                    if (ps[1].ParameterType != typeof(string) || ps[2].ParameterType != typeof(string)) continue;
                    PatchCtor(h, c, "P1", "MultiOptionDialog..ctor(message)");
                    PatchCtor(h, c, "P2", "MultiOptionDialog..ctor(title)");
                }
            }
        }

        static Type FindType(string name)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    Type t = a.GetType(name, false);
                    if (t != null) return t;
                    t = a.GetType("KSP.UI.Screens." + name, false);
                    if (t != null) return t;
                }
                catch { }
            }
            return null;
        }

        // ---------------- helpers ----------------

        static MethodInfo PrefixMethod(string name)
        {
            return typeof(Patches).GetMethod(name, BindingFlags.Public | BindingFlags.Static);
        }

        static void TryMethod(Harmony h, Type t, string name, Type[] args, string prefix, BindingFlags flags)
        {
            try
            {
                MethodInfo m = t.GetMethod(name, flags, null, args, null);
                if (m == null) { Fail(t.Name + "." + name + Args(args) + " not found"); return; }
                h.Patch(m, new HarmonyMethod(PrefixMethod(prefix)));
                ok++;
            }
            catch (Exception e)
            {
                Fail(t.Name + "." + name + Args(args) + ": " + e.GetType().Name + ": " + e.Message);
            }
        }

        static void PatchMethod(Harmony h, MethodInfo m, string prefix, string label)
        {
            try
            {
                h.Patch(m, new HarmonyMethod(PrefixMethod(prefix)));
                ok++;
            }
            catch (Exception e)
            {
                Fail(label + ": " + e.GetType().Name + ": " + e.Message);
            }
        }

        static void TryCtor(Harmony h, Type t, Type[] args, string prefix)
        {
            try
            {
                ConstructorInfo c = t.GetConstructor(args);
                if (c == null) { Fail(t.Name + "..ctor" + Args(args) + " not found"); return; }
                h.Patch(c, new HarmonyMethod(PrefixMethod(prefix)));
                ok++;
            }
            catch (Exception e)
            {
                Fail(t.Name + "..ctor" + Args(args) + ": " + e.GetType().Name + ": " + e.Message);
            }
        }

        static void PatchCtor(Harmony h, ConstructorInfo c, string prefix, string label)
        {
            try
            {
                h.Patch(c, new HarmonyMethod(PrefixMethod(prefix)));
                ok++;
            }
            catch (Exception e)
            {
                Fail(label + ": " + e.GetType().Name + ": " + e.Message);
            }
        }

        static void PropertySetter(Harmony h, Type t, string property, string prefix)
        {
            try
            {
                PropertyInfo p = t.GetProperty(property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (p == null) { Fail(t.Name + "." + property + " property not found"); return; }
                MethodInfo setter = p.GetSetMethod(true);
                if (setter == null) { Fail(t.Name + "." + property + " has no setter"); return; }
                h.Patch(setter, new HarmonyMethod(PrefixMethod(prefix)));
                ok++;
            }
            catch (Exception e)
            {
                Fail(t.Name + "." + property + ": " + e.GetType().Name + ": " + e.Message);
            }
        }

        static string Args(Type[] args)
        {
            string s = "(";
            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0) s += ", ";
                s += args[i].Name;
            }
            return s + ")";
        }

        static void Fail(string msg)
        {
            fail++;
            errors.Add(msg);
        }

        // ---------------- prefix bodies ----------------

        // Prefixes must never throw: an exception here aborts the caller's GUI
        // element (Mono logs it every frame) and can make the game unplayable.
        public static void P0(ref string __0) { if (__0 == null) return; try { __0 = Translator.T(__0); } catch { } }
        public static void P1(ref string __1) { if (__1 == null) return; try { __1 = Translator.T(__1); } catch { } }
        public static void P2(ref string __2) { if (__2 == null) return; try { __2 = Translator.T(__2); } catch { } }
        public static void P3(ref string __3) { if (__3 == null) return; try { __3 = Translator.T(__3); } catch { } }
    }
}
