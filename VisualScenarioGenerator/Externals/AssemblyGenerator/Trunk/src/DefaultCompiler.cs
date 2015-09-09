using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Drawing;

namespace AssemblyGenerator
{
    public class DefaultCompiler : Compiler
    {
        public void Compile(String code, String path, String assemblyName, bool debugOutput)
        {
            if (debugOutput) // for debugging compiler errors
            {
                System.Console.WriteLine("*");
                System.Console.WriteLine("*");
                System.Console.WriteLine("*");
                System.Console.WriteLine(code);
                System.Console.WriteLine("*");
                System.Console.WriteLine("*");
                System.Console.WriteLine("*");
            }

            // Approach:  Make a code provider, give it compiler parameters, pass it the code source to produce an assembly,
            // instantiate an object out of that assembly, and bind to the PropertyGrid
            CSharpCodeProvider myCodeProvider = new CSharpCodeProvider();

            // reference assemblies to provide to the compiler, followed by the assembly name
            CompilerParameters myCompilerParameters = new CompilerParameters(new String[] { "System.dll", "System.Data.dll", "System.Drawing.dll" }, path+"\\"+assemblyName+".dll");

            myCompilerParameters.GenerateExecutable = false;   // no need
            myCompilerParameters.GenerateInMemory = false;

            // takes parameters and a string[] of code
            CompilerResults myCompilerResults = myCodeProvider.CompileAssemblyFromSource(myCompilerParameters, new String[] { code });

            if (myCompilerResults.Errors.Count > 0) // print any errors
            {
                foreach (CompilerError error in myCompilerResults.Errors)
                {
                    System.Console.WriteLine(error.ToString());
                }
            }
        }
    }
}
