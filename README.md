<p align="center">
    <img width="256" height="256" src="https://user-images.githubusercontent.com/40871836/43434251-81f1bcb6-9440-11e8-8eaa-505914246ae6.png">
<p>

# Memory Mapper
<p align="left">
    <!-- Version -->
    <img src="https://img.shields.io/badge/version-1.0.0-brightgreen.svg">
    <!-- <img src="https://img.shields.io/appveyor/ci/gruntjs/grunt.svg"> -->
    <!-- Docs -->
    <img src="https://img.shields.io/badge/docs-not%20found-lightgrey.svg">
    <!-- License -->
    <img src="https://img.shields.io/badge/license-MIT-blue.svg">
</p>

Memory Mapper is a lightweight library which allows the ability to map both native and managed assemblies into memory by either using process injection of a process specified by the user or self-injection; the technique of injecting an assembly into the currently running process attempting to do the injection. The library comes with tools not only to map assemblies, but with the capabilities to encrypt, decrypt, and generate various amounts of cryptographically strong data.

### Requirements
***Note:*** *(For the running assembly using Memory Mapper ***ONLY*** — not for stubs/shellcode)*

- Windows 7 SP1 & Higher
- .NET Framework 4.6.1

# Features
- Explore the structure of a PE (portable executable)
- Read resources from both managed and native assemblies
- Map native assemblies into memory using process injection and self-injection
- Map managed assemblies into memory using process injection and other techniques
- Obtain an array of bytes for any file of any file size
- Encrypt and decrypt entire files and raw bytes
- Generate and validate checksums of files and raw bytes
- Generate cryptographically strong random data using a `SecureRandom` object
- Comes bundled with multiple encryption and hashing algorithms
    #### Encryption
    - AES *(ECB)*
    - AES *(CBC)*
    - AES *(CFB)*
    - AES *(OFB)*
    - AES *(CTR)*

    #### Hashing
    - MD5
    - RIPEMD160
    - SHA1
    - SHA256
    - SHA384
    - SHA512

# Examples
### Native Injection
This example shows how to statically map a native assembly into memory using the `NativeLoader` tool. The example loads the file by reading all of its bytes from disk and then injects the *PE (portable executable)* associated with the bytes directly into memory. Using the native loader in conjunction with *Dynamic Code Compilation* found in my [Amaterasu](https://github.com/CloneMerge/Amaterasu) library one could accomplish on-the-fly code compilation and injection all from code in-memory.

```c#
using System;
using System.IO;
using System.Reflection;
using MemoryMapper;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the bytes of the file we want to load.
            var filePath = "FileToReadBytesOf";
            var fileBytes = File.ReadAllBytes(filePath);

            // Check if the assembly is managed or native.
            bool isManaged = false;
            try
            {
                // Note — this is one of the simplest variations of checking assemblies
                var assemblyName = AssemblyName.GetAssemblyName(filePath);
                if (assemblyName != null)
                    if (assemblyName.FullName != null)
                        isManaged = true;
            }
            catch { isManaged = false; }

            // Try loading the assembly if it's truly native.
            if (!isManaged)
            {
                NativeLoader loader = new NativeLoader();
                if (loader.LoadAssembly(fileBytes))
                    Console.WriteLine("Assembly loaded successfully!");
                else
                    Console.WriteLine("Assembly could not be loaded.");
            }

            // Wait for user interaction.
            Console.Read();
        }
    }
}
```

### Managed Injection
This example shows how to statically map a managed assembly into memory by reading in its bytes — or by using an embedded byte array — and then using the `ManagedLoader` to inject into a currently running process. Almost any managed assembly can be mapped using the provided `ManagedLoader` tool.

```c#
using System;
using System.IO;
using System.Reflection;
using MemoryMapper;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the bytes of the file we want to load.
            var filePath = "FileToReadBytesOf";
            var fileBytes = File.ReadAllBytes(filePath);

            // Check if the assembly is managed or native.
            bool isManaged = false;
            try
            {
                // Note — this is one of the simplest variations of checking assemblies
                var assemblyName = AssemblyName.GetAssemblyName(filePath);
                if (assemblyName != null)
                    if (assemblyName.FullName != null)
                        isManaged = true;
            }
            catch { isManaged = false; }

            // Try loading the assembly if it's truly managed.
            if (isManaged)
            {
                // Set the name of a surrogate process - the process we'll inject into.
                var processName = "explorer.exe"; // Can also be the current process's name for self-injection.
                ManagedLoader loader = new ManagedLoader();
                if (loader.LoadAssembly(fileBytes, processName))
                    Console.WriteLine("Assembly loaded successfully!");
                else
                    Console.WriteLine("Assembly could not be loaded.");
            }

            // Wait for user interaction.
            Console.Read();
        }
    }
}
```
# Credits
**Icon:** `DesignBolts` <br>
http://www.designbolts.com/

# License

Copyright © ∞ Jason Drawdy 

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
