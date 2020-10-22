using System;
using System.Diagnostics;
using System.IO;
using MInject;

namespace Loader
{
    internal static class Program
    {
        private const string ProcessName = "PlagueIncEvolved";

        private static void Main()
        {
            var targetProcesses = Process.GetProcessesByName(ProcessName);
            if (targetProcesses.Length == 0)
            {
                Console.WriteLine($"Process {ProcessName} not found");
                Console.ReadKey();
                Environment.Exit(1);
            }
                
            var targetProcess = targetProcesses[0];
            
            MonoProcess monoProcess;
            if (!MonoProcess.Attach(targetProcess, out monoProcess)) return;
            
            var assemblyBytes = File.ReadAllBytes("Trainer.dll");

            var monoDomain = monoProcess.GetRootDomain();
            monoProcess.ThreadAttach(monoDomain);
            monoProcess.SecuritySetMode(0);
    
            monoProcess.DisableAssemblyLoadCallback();

            var rawAssemblyImage = monoProcess.ImageOpenFromDataFull(assemblyBytes);
            var assemblyPointer = monoProcess.AssemblyLoadFromFull(rawAssemblyImage);
            var assemblyImage = monoProcess.AssemblyGetImage(assemblyPointer);
            var classPointer = monoProcess.ClassFromName(assemblyImage, "Trainer", "Loader");
            var methodPointer = monoProcess.ClassGetMethodFromName(classPointer, "Init");
        
            monoProcess.RuntimeInvoke(methodPointer);

            monoProcess.EnableAssemblyLoadCallback();    
    
            monoProcess.Dispose();
        }
    }
}