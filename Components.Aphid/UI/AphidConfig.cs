﻿using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Components.Aphid.UI
{
    // Todo:
    // *Wire up
    // *generate class from INI file.
    // *Save embedded default config to disk if none present.
    public class AphidConfig : DefaultSingleton<AphidConfig>
    {
        public const string FileName = "Components.Aphid.dll.config";

        private static Lazy<Configuration> _config = new Lazy<Configuration>(LoadConfig);

        private Lazy<bool>
            _strictMode = GetBool(AphidSettings.StrictMode, defaultValue: true),
            _saveErrors = GetBool(AphidSettings.SaveErrors, defaultValue: false),
            _stackTraceParams = GetBool(AphidSettings.StackTraceParams, defaultValue: false),
            _scriptCaching = GetBool(AphidSettings.ScriptCaching, defaultValue: false),
            _ignoreDebugger = GetBool(AphidSettings.IgnoreDebugger, defaultValue: false);

        private Lazy<string[]>
            _imports = GetArray(AphidSettings.AutoImport, defaultValue: new string[0]),
            _includes = GetArray(AphidSettings.AutoInclude, defaultValue: new string[0]);

        public string[] Imports
        {
            get { return _imports.Value; }
        }

        public bool StrictMode
        {
            get { return _strictMode.Value; }
        }

        public bool SaveErrors
        {
            get { return _saveErrors.Value; }
        }

        public bool StackTraceParams
        {
            get { return _stackTraceParams.Value; }
        }

        public bool ScriptCaching
        {
            get { return _scriptCaching.Value; }
        }

        public bool IgnoreDebugger
        {
            get { return _ignoreDebugger.Value; }
        }

        public bool ExceptionHandlingDisabled { get; set; }

        public bool ExceptionHandlingClrStack { get; set; }

        public bool ReplShowHelpArgWarning { get; set; }

        public bool ReplJit { get; set; }

        public bool ReplLoggingInput { get; set; }

        public bool ReplLoggingOutput { get; set; }

        public bool ReplLoggingCombined { get; set; }

        public bool ConsoleAsync { get; set; }

        public AphidConfig()
            : base()
        {

        }

        private static Lazy<bool> GetBool(string name, bool defaultValue = false)
        {
            return _config.Value != null ?
                new Lazy<bool>(() => Convert.ToBoolean(GetSetting(name))) :
                new Lazy<bool>(() => defaultValue);
        }

        private static Lazy<string[]> GetArray(string name, string[] defaultValue = null)
        {
            return _config.Value != null ?
                new Lazy<string[]>(() => (GetSetting(name) ?? "")
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToArray()) :
                new Lazy<string[]>(() => defaultValue);
        }

        private static string GetSetting(string name)
        {
            var setting = _config.Value.AppSettings.Settings[name];

            return setting != null ? setting.Value : null;
        }

        private static Configuration LoadConfig()
        {
            try
            {
                var l = typeof(AphidConfig).Assembly.Location;
                string path;

                if (l != null)
                {
                    path = Path.Combine(Path.GetDirectoryName(l), FileName);
                }
                else
                {
                    path = FileName;
                }

                return ConfigurationManager.OpenMappedExeConfiguration(
                    new ExeConfigurationFileMap
                    {
                        ExeConfigFilename = path,
                    },
                    ConfigurationUserLevel.None);
            }
            catch
            {
                return null;
            }
        }
    }
}
