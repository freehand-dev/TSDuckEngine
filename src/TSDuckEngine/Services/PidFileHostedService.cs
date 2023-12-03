using System.IO;

namespace TSDuckEngine.Services
{
    public class PidFileHostedService : IHostedService
    {
        private readonly ILogger<PidFileHostedService> logger;
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment env;

        private bool isPidFileCreated = false;
        private string? pidFile;

        public PidFileHostedService(ILogger<PidFileHostedService> logger,
            IConfiguration configuration,
            IHostEnvironment env)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                pidFile = configuration.GetSection("General")?.GetValue<string>("PIDFile") ?? configuration.GetValue<string>("PIDFile");

                if (!string.IsNullOrEmpty(pidFile))
                {
                    await WritePidFile();
                    isPidFileCreated = true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unexpected error when starting {nameof(PidFileHostedService)}");
            }
        }

        private async Task WritePidFile()
        {
            string? directory = Path.GetDirectoryName(pidFile);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var processId1 = System.Diagnostics.Process.GetCurrentProcess();

            var processId = Environment.ProcessId.ToString();
            
            if (!string.IsNullOrEmpty(pidFile))
                await File.WriteAllTextAsync(pidFile, processId);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (isPidFileCreated && !string.IsNullOrEmpty(pidFile))
                    File.Delete(pidFile);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error when deleting PID file");
            }

            return Task.CompletedTask;
        }
    }
}