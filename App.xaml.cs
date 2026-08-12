using System.Windows;
using Velopack;
using Velopack.Sources;

namespace CrappyLauncher
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            VelopackApp.Build().Run();
            base.OnStartup(e);

            _ = CheckForUpdatesAsync();
        }

        public async Task CheckForUpdatesAsync()
        {
            var mgr = new UpdateManager(new GithubSource(
                "https://github.com/SaigoNoTanuki/Crappy-Launcher",
                accessToken: null,
                prerelease: false));

            if (!mgr.IsInstalled)
            {
                return;
            }

            try
            {
                var newVersion = await mgr.CheckForUpdatesAsync();
                if (newVersion == null)
                    return;

                await mgr.DownloadUpdatesAsync(newVersion);

                var result = MessageBox.Show(
                    "An update has been downloaded. Restart to apply it?",
                    "Update Installed",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                    mgr.ApplyUpdatesAndRestart(newVersion);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

    }
}
