using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KSPZHCN
{
    /// <summary>
    /// Dictionary-based UI string translator. No Unity dependencies.
    /// Lookups are read-only after Load, so they are safe to call from any thread.
    /// Case-insensitive matching uses a custom ASCII comparer: it avoids both
    /// Mono's buggy StringComparer.OrdinalIgnoreCase path (which can throw
    /// NullReferenceException inside TextInfo.GetInvariantCaseInsensitiveHashCode)
    /// and the per-call string allocations of ToLowerInvariant.
    /// </summary>
    public static class Translator
    {
        sealed class AsciiIgnoreCaseComparer : IEqualityComparer<string>
        {
            public bool Equals(string a, string b)
            {
                if (ReferenceEquals(a, b)) return true;
                if (a == null || b == null || a.Length != b.Length) return false;
                for (int i = 0; i < a.Length; i++)
                {
                    char ca = a[i], cb = b[i];
                    if (ca == cb) continue;
                    if (ca >= 'A' && ca <= 'Z') ca = (char)(ca + 32);
                    if (cb >= 'A' && cb <= 'Z') cb = (char)(cb + 32);
                    if (ca != cb) return false;
                }
                return true;
            }

            public int GetHashCode(string s)
            {
                if (s == null) return 0;
                int h = 0;
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];
                    if (c >= 'A' && c <= 'Z') c = (char)(c + 32);
                    h = h * 31 + c;
                }
                return h;
            }
        }

        static readonly Dictionary<string, string> exact = new Dictionary<string, string>(StringComparer.Ordinal);
        static readonly Dictionary<string, string> ci = new Dictionary<string, string>(new AsciiIgnoreCaseComparer());

        public static bool Enabled = true;
        public static bool Verbose = false;

        public static int Count { get { return exact.Count; } }

        public static void Clear()
        {
            exact.Clear();
            ci.Clear();
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
                if (!ci.ContainsKey(k)) ci[k] = v;
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
                    if (trimmed.Length != n)
                    {
                        if (exact.TryGetValue(trimmed, out r))
                        {
                            int lead = s.IndexOf(trimmed, StringComparison.Ordinal);
                            if (lead < 0) return s;
                            return s.Substring(0, lead) + r + s.Substring(lead + trimmed.Length);
                        }
                        if (ci.TryGetValue(trimmed, out r))
                        {
                            int lead = s.IndexOf(trimmed, StringComparison.Ordinal);
                            if (lead < 0) return s;
                            return s.Substring(0, lead) + r + s.Substring(lead + trimmed.Length);
                        }
                        return s;
                    }
                    if (ci.TryGetValue(trimmed, out r)) return r;
                    return s;
                }
                if (ci.TryGetValue(s, out r)) return r;
                return s;
            }
            catch
            {
                return s;
            }
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
