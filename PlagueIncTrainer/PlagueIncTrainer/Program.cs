using System.Diagnostics;
using System.IO;
using MInject;

namespace PlagueIncTrainer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var targetProcess = Process.GetProcessesByName("PlagueIncEvolved")[0];

            if (!MonoProcess.Attach(targetProcess, out var monoProcess)) return;
            
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