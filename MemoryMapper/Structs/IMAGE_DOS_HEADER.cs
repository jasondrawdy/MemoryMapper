/*
==============================================================================
Copyright © Jason Drawdy 

All rights reserved.

The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

Except as contained in this notice, the name of the above copyright holder
shall not be used in advertising or otherwise to promote the sale, use or
other dealings in this Software without prior written authorization.
==============================================================================
*/

#region Imports

using System;
using System.Runtime.InteropServices;

#endregion
namespace GhostMapper
{
    //typedef struct _IMAGE_DOS_HEADER {      // DOS .EXE header
    //WORD   e_magic;                     // Magic number
    //WORD   e_cblp;                      // Bytes on last page of file
    //WORD   e_cp;                        // Pages in file
    //WORD   e_crlc;                      // Relocations
    //WORD   e_cparhdr;                   // Size of header in paragraphs
    //WORD   e_minalloc;                  // Minimum extra paragraphs needed
    //WORD   e_maxalloc;                  // Maximum extra paragraphs needed
    //WORD   e_ss;                        // Initial (relative) SS value
    //WORD   e_sp;                        // Initial SP value
    //WORD   e_csum;                      // Checksum
    //WORD   e_ip;                        // Initial IP value
    //WORD   e_cs;                        // Initial (relative) CS value
    //WORD   e_lfarlc;                    // File address of relocation table
    //WORD   e_ovno;                      // Overlay number
    //WORD   e_res[4];                    // Reserved words
    //WORD   e_oemid;                     // OEM identifier (for e_oeminfo)
    //WORD   e_oeminfo;                   // OEM information; e_oemid specific
    //WORD   e_res2[10];                  // Reserved words
    //LONG   e_lfanew;                    // File address of new exe header
    //} IMAGE_DOS_HEADER, *PIMAGE_DOS_HEADER;
    
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_DOS_HEADER
    {
        public UInt16 e_magic;       // Magic number
        public UInt16 e_cblp;        // Bytes on last page of file
        public UInt16 e_cp;          // Pages in file
        public UInt16 e_crlc;        // Relocations
        public UInt16 e_cparhdr;     // Size of header in paragraphs
        public UInt16 e_minalloc;    // Minimum extra paragraphs needed
        public UInt16 e_maxalloc;    // Maximum extra paragraphs needed
        public UInt16 e_ss;          // Initial (relative) SS value
        public UInt16 e_sp;          // Initial SP value
        public UInt16 e_csum;        // Checksum
        public UInt16 e_ip;          // Initial IP value
        public UInt16 e_cs;          // Initial (relative) CS value
        public UInt16 e_lfarlc;      // File address of relocation table
        public UInt16 e_ovno;        // Overlay number
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public UInt16[] e_res1;        // Reserved words
        public UInt16 e_oemid;       // OEM identifier (for e_oeminfo)
        public UInt16 e_oeminfo;     // OEM information; e_oemid specific
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public UInt16[] e_res2;        // Reserved words
        public Int32 e_lfanew;      // File address of new exe header
    }

    //[StructLayout(LayoutKind.Explicit)]
    //public struct IMAGE_DOS_HEADER
    //{
    //    [FieldOffset(0)]
    //    public ushort magic;      // WORD - Magic number

    //    [FieldOffset(2)]
    //    public ushort cblp;       // WORD - Bytes on last page of file

    //    [FieldOffset(4)]
    //    public ushort cp;         // WORD - Pages in file

    //    [FieldOffset(6)]
    //    public ushort crlc;       // WORD - Relocations

    //    [FieldOffset(8)]
    //    public ushort cparhdr;    // WORD - Size of header in paragraphs

    //    [FieldOffset(10)]
    //    public ushort minalloc;   // WORD - Minimum extra paragraphs needed 

    //    [FieldOffset(12)]
    //    public ushort maxalloc;   // WORD - Maximum extra paragraphs needed 

    //    [FieldOffset(14)]
    //    public ushort ss;         // WORD - Initial (relative) SS value

    //    [FieldOffset(16)]
    //    public ushort sp;         // WORD - Initial SP value

    //    [FieldOffset(18)]
    //    public ushort csum;       // WORD - Checksum

    //    [FieldOffset(20)]
    //    public ushort ip;         // WORD - Initial IP value

    //    [FieldOffset(22)]
    //    public ushort cs;         // WORD - Initial (relative) CS value

    //    [FieldOffset(24)]
    //    public ushort lfarlc;     // WORD - File address of relocation table

    //    [FieldOffset(26)]
    //    public ushort ovno;       // WORD - Overlay number

    //    //[FieldOffset(28)]       // WORD[4] = 2x4 = 8
    //    // public ushort[] e_res;      // WORD[4] - Reserved words

    //    [FieldOffset(36)]
    //    public ushort oemid;      // WORD - OEM identifier (for e_oeminfo)

    //    [FieldOffset(38)]
    //    public ushort oeminfo;    // WORD - OEM information (e_oemid specific)

    //    //[FieldOffset(40)]       // WORD[10] = 2x10 = 20
    //    //public ushort[] d_res2;     // WORD[10] - Reserved words 

    //    [FieldOffset(60)]
    //    public int lfanew;        // LONG - File address of new exe header

    //}
}