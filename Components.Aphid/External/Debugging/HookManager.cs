﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Cypress
{
    public class HookManager
    {
        private ProcessMemory _memory;

        private TrampolineHeap _heap;

        public HookManager(ProcessMemory memory)
        {
            _heap = new TrampolineHeap(_memory = memory);
        }

        public bool Hook(IntPtr funcAddr)
        {
            return NativeHook.Hook(_memory, funcAddr, _heap);
        }
    }
}