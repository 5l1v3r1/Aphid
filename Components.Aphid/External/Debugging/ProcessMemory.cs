﻿using Components.PInvoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Components.Cypress
{
    public class ProcessMemory
    {
        public IntPtr Handle { get; private set; }

        public ProcessMemory(IntPtr handle)
        {
            Handle = handle;
        }

        public byte[] ReadBytes(IntPtr address, int count)
        {
            var buffer = new byte[count];
            var bytesRead = 0;

            if (!Kernel32.ReadProcessMemory(
                Handle,
                address,
                buffer,
                buffer.Length,
                out bytesRead))
            {
                return null;
            }

            Array.Resize(ref buffer, bytesRead);

            return buffer;
        }

        public uint[] ReadUInt32s(IntPtr address, int count)
        {
            var bytes = ReadBytes(address, count * sizeof(uint));

            if (bytes == null)
            {
                return null;
            }

            var uints = new uint[count];

            for (int i = 0; i < count; i++)
            {
                uints[i] = BitConverter.ToUInt32(bytes, i * sizeof(uint));
            }

            return uints;
        }

        public MEMORY_BASIC_INFORMATION ReadMemoryInfo(IntPtr address)
        {
            MEMORY_BASIC_INFORMATION m;

            var result = Kernel32.VirtualQueryEx(
                Handle,
                address,
                out m,
                (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

            return m;
        }

        public bool IsExecutable(IntPtr address)
        {
            var memInfo = ReadMemoryInfo(address);

            return
                memInfo.Protect == AllocationProtect.PAGE_EXECUTE ||
                memInfo.Protect == AllocationProtect.PAGE_EXECUTE_READ ||
                memInfo.Protect == AllocationProtect.PAGE_EXECUTE_READWRITE ||
                memInfo.Protect == AllocationProtect.PAGE_EXECUTE_WRITECOPY;
        }

        public CONTEXT GetContext(uint threadId)
        {
            var threadHandle = Kernel32.OpenThread(ThreadAccess.THREAD_GET_CONTEXT, false, threadId);
            var context = new CONTEXT();
            context.ContextFlags = CONTEXT_FLAGS.CONTEXT_ALL;
            var success = Kernel32.GetThreadContext(threadHandle, ref context);
            Kernel32.CloseHandle(threadHandle);
            return context;
        }

        private static Encoding _encoding = Encoding.GetEncoding("iso-8859-1");

        public string ReadAscii(IntPtr address, int bufferSize = 0x100)
        {
            var sb = new StringBuilder();
            var inString = true;
            do
            {
                var tmp = ReadBytes(address, bufferSize);

                if (tmp == null)
                {
                    return sb.ToString();
                }

                int i = 0;
                for (i = 0; i < tmp.Length; i ++)
                {
                    if (tmp[i] == 0)
                    {
                        inString = false;
                        break;
                    }
                }

                sb.Append(_encoding.GetString(tmp, 0, i));
                address += bufferSize;
            } while (inString);


            return sb.ToString();
        }

        public string ReadUnicode(IntPtr address, int bufferSize = 0x100)
        {
            var sb = new StringBuilder();
            var inString = true;
            do
            {
                var tmp = ReadBytes(address, bufferSize);

                if (tmp == null)
                {
                    return sb.ToString();
                }
                
                int i = 0;
                for (i = 0; i < tmp.Length; i += 2)
                {
                    if (tmp[i] == 0 && tmp[i + 1] == 0)
                    {
                        inString = false;
                        break;
                    }
                }

                sb.Append(Encoding.Unicode.GetString(tmp, 0, i));

                address += bufferSize;
            } while (inString);


            return sb.ToString();
        }

        public int Write(IntPtr address, byte[] buffer)
        {
            int bytesWritten;

            return Kernel32.WriteProcessMemory(
                Handle,
                address, buffer, buffer.Length,
                out bytesWritten) ? 
                bytesWritten :
                -1;
        }

        public IntPtr Allocate(
            uint size,
            MemoryProtection memoryProtection)
        {
            return Allocate(
                size,
                AllocationType.Reserve | AllocationType.Commit,
                memoryProtection);
        }

        public IntPtr Allocate(
            uint size,
            AllocationType allocationType,
            MemoryProtection memoryProtection)
        {
            return Kernel32.VirtualAllocEx(
                Handle,
                IntPtr.Zero,
                size,
                allocationType,
                memoryProtection);
        }

        public bool ReadArg(IntPtr thread, int number, out uint value)
        {
            var ctx = ThreadContext.GetContext(thread, CONTEXT_FLAGS.CONTEXT_ALL);
            var valueBuffer = ReadUInt32s((IntPtr)(ctx.Esp + (0x4 * (number + 1))), 1);

            if (valueBuffer == null || valueBuffer.Length != 1)
            {
                value = 0;
                return false;
            }
            else
            {
                value = valueBuffer[0];
                return true;
            }
        }

        public uint[] ReadArgs(IntPtr thread, int count)
        {
            var ctx = ThreadContext.GetContext(thread, CONTEXT_FLAGS.CONTEXT_ALL);
            return ReadUInt32s((IntPtr)(ctx.Esp + 0x4), count);
        }
    }
}