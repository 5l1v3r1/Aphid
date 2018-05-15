﻿using Components.Aphid.UI;
using Components.Caching;
using System.IO;
using System.Text;

namespace Components.Aphid.Parser
{
    public static class AphidScript
    {
        public static string Read(string scriptFile)
        {
            return AphidConfig.Current.ScriptCaching ? 
                Encoding.UTF8
                    .GetString(FileMemoryCache.ReadAllBytes(scriptFile))
                    .Trim(new char[] { '\uFEFF', '\u200B' }) :
                File.ReadAllText(scriptFile);
        }
    }
}