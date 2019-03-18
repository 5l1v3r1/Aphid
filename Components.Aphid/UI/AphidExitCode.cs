﻿namespace Components.Aphid.UI
{
    public enum AphidExitCode : int
    {
        Success = 0x0,
        ParserError = 0x1000,
        LoadScriptError = 0x10000,
        RuntimeError = 0x100000,
        GeneralError = 0x1000000,
    }
}
