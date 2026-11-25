using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KSPZHCN
{
    /// <summary>
    /// Dictionary-based UI string translator. No Unity dependencies.
    /// Lookups are read-only after Load, so they are safe to call from any thread.
    /// Case-insensitive matching uses a prebuilt lowercase table instead of
    /// StringComparer.OrdinalIgnoreCase, whose Mono implementation can throw
    /// NullReferenceException inside TextInfo.GetInvariantCaseInsensitiveHashCode
    /// for some UI strings (observed with MechJeb / RemoteTech labels).
    /// </summary>
    public static class Translator
    {
        static readonly Dictionary<string, string> exact = new Dictionary<string, string>(StringComparer.Ordinal);
        static readonly Dictionary<string, string> lower = new Dictionary<string, string>(StringComparer.Ordinal);

        public static bool Enabled = true;
        public static bool Verbose = false;

        public static int Count { get { return exact.Count; } }

        public static void Clear()
        {
            exact.Clear();
            lower.Clear();
        }

        public static void Load(string path)
        {
            if (!File.Exists(path))
            {
                Util.Warn("dictionary not found: " + path);
                return;
            }
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            int loaded = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string raw = lines[i];
                if (raw.Length == 0 || raw[0] == '#') continue;
                int tab = raw.IndexOf('\t');
                if (tab <= 0) continue;
                string k = Unescape(raw.Substring(0, tab));
                string v = Unescape(raw.Substring(tab + 1));
                if (k.Length == 0) continue;
                exact[k] = v;
                try
                {
                    string lk = k.ToLowerInvariant();
                    if (!lower.ContainsKey(lk)) lower[lk] = v;
                }
                catch { }
                loaded++;
            }
            Util.Log("dictionary loaded: " + loaded + " entries");
        }

        public static string T(string s)
        {
            if (!Enabled || string.IsNullOrEmpty(s)) return s;
            try
            {
                string r;
                if (exact.TryGetValue(s, out r)) return r;

                int n = s.Length;
                if (char.IsWhiteSpace(s[0]) || char.IsWhiteSpace(s[n - 1]))
                {
                    string trimmed = s.Trim();
                    if (trimmed.Length == 0) return s;
                    if (trimmed.Length == n)
                    {
                        return LookupCI(trimmed, s);
                    }
                    if (exact.TryGetValue(trimmed, out r))
                    {
                        int lead = s.IndexOf(trimmed, StringComparison.Ordinal);
                        if (lead < 0) return s;
                        return s.Substring(0, lead) + r + s.Substring(lead + trimmed.Length);
                    }
                    if (lower.TryGetValue(Key(trimmed), out r))
                    {
                        int lead = s.IndexOf(trimmed, StringComparison.Ordinal);
                        if (lead < 0) return s;
                        return s.Substring(0, lead) + r + s.Substring(lead + trimmed.Length);
                    }
                    return s;
                }
                return LookupCI(s, s);
            }
            catch
            {
                return s;
            }
        }

        static string LookupCI(string key, string original)
        {
            string r;
            if (lower.TryGetValue(Key(key), out r)) return r;
            return original;
        }

        /// <summary>Lowercase only when needed, to avoid allocations on every miss.</summary>
        static string Key(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsUpper(s[i])) return s.ToLowerInvariant();
            }
            return s;
        }

        static string Unescape(string s)
        {
            if (s.IndexOf('\\') < 0) return s;
            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == '\\' && i + 1 < s.Length)
                {
                    char n = s[i + 1];
                    if (n == 't') { sb.Append('\t'); i++; continue; }
                    if (n == 'n') { sb.Append('\n'); i++; continue; }
                    if (n == 'r') { sb.Append('\r'); i++; continue; }
                    if (n == '\\') { sb.Append('\\'); i++; continue; }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
