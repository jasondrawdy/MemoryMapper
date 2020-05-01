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
using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;

#endregion
namespace MemoryMapper
{
    internal class ResourceReader
    {
        #region Variables

        [DllImport("kernel32.dll")]
        static extern IntPtr GetModuleHandle(string module);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr FindResource(IntPtr hModule, string lpName, string lpType);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

        #endregion
        #region Methods

        /// <summary>
        /// Reads a resource from a native assembly.
        /// </summary>
        /// <param name="baseName">The base name of the resource to read.</param>
        /// <param name="baseType">The type of data to read from the resource.</param>
        public static byte[] ReadNative(string baseName = "Encrypted", string baseType = "RT_RCDATA")
        {
            // Get the handle of a resource from a defined file.
            IntPtr moduleHandle = GetModuleHandle(Assembly.GetExecutingAssembly().Location);
            IntPtr resourceLocataion = FindResource(moduleHandle, baseName, baseType);
            IntPtr mountPointer = LoadResource(moduleHandle, resourceLocataion);
            uint size = SizeofResource(moduleHandle, resourceLocataion);
            byte[] data = new byte[size];
            
            // Copy our the bytes of our resource into a buffer and return those bytes.
            Marshal.Copy(mountPointer, data, 0, (int)size);
            return data;
        }

        /// <summary>
        /// Reads a resource from a managed assembly.
        /// </summary>
        /// <param name="baseName">The base name of the assembly to read.</param>
        /// <param name="resourceName">The actual name of the resource to read.</param>
        /// <returns></returns>
        public static byte[] ReadManaged(string baseName = "Encrypted", string resourceName = "encFile")
        {
            // Obtain the bytes of a resource within the assembly.
            ResourceManager manager = new ResourceManager(baseName, Assembly.GetExecutingAssembly());
            byte[] data = (byte[])manager.GetObject(resourceName);
            return data;
        }

        #endregion
    }
}
