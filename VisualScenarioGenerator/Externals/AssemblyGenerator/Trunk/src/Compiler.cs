using System;
using System.Collections.Generic;
using System.Text;

namespace AssemblyGenerator
{
    public interface Compiler
    {
        void Compile(String code, String path, String assemblyName, bool debugOutput);
    }
}
