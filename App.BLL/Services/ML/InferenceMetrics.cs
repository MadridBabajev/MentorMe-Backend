// Services/InferenceMetrics.cs
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace App.BLL.Services.ML;

public class InferenceMetrics
{
    public double LoadTimeMs { get; set; }
    public double InferenceTimeMs { get; set; }
    public double MemoryBeforeMb { get; set; }
    public double MemoryAfterMb { get; set; }
    public double CpuPercent { get; set; }
    public double GpuUtilPercent { get; set; }
    public long ModelSizeMb { get; set; }

    public static long DirectorySizeBytes(string path) =>
        Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
                 .Sum(f => new FileInfo(f).Length);

    public static (double usedMb, double utilPct) SampleGpu()
    {
        var psi = new ProcessStartInfo("nvidia-smi",
            "--query-gpu=utilization.gpu --format=csv,noheader,nounits")
        {
            RedirectStandardOutput = true,
            UseShellExecute = false
        };
        using var proc = Process.Start(psi)!;
        var line = proc.StandardOutput.ReadLine();
        proc.WaitForExit();
        if (double.TryParse(line, out var gpuPct))
            return (0, gpuPct);
        return (0, 0);
    }

    public static async Task<InferenceMetrics> MeasureAsync(
        Func<Task> loadModelAsync,
        Func<Task> runInferenceAsync,
        string modelDirectory)
    {
        var proc     = Process.GetCurrentProcess();
        var cpuCores = Environment.ProcessorCount;

        // 1) Model size
        var sizeMb = DirectorySizeBytes(modelDirectory) / 1024 / 1024;

        // 2) Before load
        var memBefore    = proc.WorkingSet64 / (1024.0 * 1024.0);
        var cpuTimeMid1  = proc.TotalProcessorTime;
        var sw           = Stopwatch.StartNew();

        // 3) Load
        await loadModelAsync();
        sw.Stop();
        var loadMs = sw.Elapsed.TotalMilliseconds;

        // 4) Before inference
        var memMid      = proc.WorkingSet64 / (1024.0 * 1024.0);
        var cpuTimeMid2 = proc.TotalProcessorTime;

        // 5) Inference
        sw.Restart();
        await runInferenceAsync();
        sw.Stop();
        var infMs = sw.Elapsed.TotalMilliseconds;

        // 6) After inference
        var memAfter     = proc.WorkingSet64 / (1024.0 * 1024.0);
        var cpuTimeAfter = proc.TotalProcessorTime;

        // 7) CPU %
        var cpuDelta   = (cpuTimeAfter - cpuTimeMid2).TotalMilliseconds;
        var cpuPercent = cpuDelta / infMs / cpuCores * 100.0;

        // 8) GPU %
        var gpuSample = SampleGpu();

        return new InferenceMetrics
        {
            LoadTimeMs      = loadMs,
            InferenceTimeMs = infMs,
            MemoryBeforeMb  = memMid,
            MemoryAfterMb   = memAfter,
            CpuPercent      = cpuPercent,
            GpuUtilPercent  = gpuSample.utilPct,
            ModelSizeMb     = sizeMb
        };
    }
}
