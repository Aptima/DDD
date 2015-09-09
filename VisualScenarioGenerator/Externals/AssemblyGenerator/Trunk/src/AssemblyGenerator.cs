using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualStudio;

namespace AssemblyGenerator
{
    [Guid("850F8F5F-267C-4620-A490-FADAD46D9C99")]
    public class AssemblyGenerator : IVsSingleFileGenerator
    {
        public AssemblyGenerator() { }

        public static void Test()
        {
            AssemblyGenerator test = new AssemblyGenerator();
            test.GenerateFromXML("configuration.xml", "Config.parameters");
        }

        // call this to dynamically re-generate from a conforming config file.
        public int GenerateFromXML(String fullXMLFileNameInConfigDirectory, String outputNamespace)
        {
            uint outputInt;
            IntPtr[] outputFile = new IntPtr[1];
            IVsGeneratorProgress generatorProgress = null;
            return this.Generate(Directory.GetCurrentDirectory() + "\\Config\\" + fullXMLFileNameInConfigDirectory, "", outputNamespace, outputFile, out outputInt, generatorProgress);
        }

        #region IVsSingleFileGenerator Members

        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = ".cs";
            return 0;
        }

        // called by custom tool invocation in Visual Studio
        // inputFile is the fullpath and filename of the file the user right clicked on
        public int Generate(string inputFile, string InputFileContents, string defaultNamespace, IntPtr[] outputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            XMLParser myParser = new XMLParser(inputFile);

            String parsingResult = myParser.Parse();

            // append namespace if provided
            if (defaultNamespace.Length > 0)
            {
                String namespaceBegin = "namespace "+ defaultNamespace + " { "+Environment.NewLine;

                parsingResult = namespaceBegin + parsingResult + " }";
            }

            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] forOutputFile = enc.GetBytes(parsingResult);

            if (forOutputFile == null)
            {
                outputFileContents[0] = IntPtr.Zero;

                pcbOutput = 0;
            }
            else
            {
                // allocate memory and copy byte array for output file
                outputFileContents[0] = Marshal.AllocCoTaskMem(forOutputFile.Length);

                Marshal.Copy(forOutputFile, 0, outputFileContents[0], forOutputFile.Length);

                pcbOutput = (uint)forOutputFile.Length; // as per docs, return length of byte stream
            }

            return VSConstants.S_OK;
        }
        #endregion
    }
}
