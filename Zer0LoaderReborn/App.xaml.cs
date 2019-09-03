using System;
using System.Threading;
using System.Windows;


namespace Zer0LoaderReborn
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex _instanceMutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            _instanceMutex = new Mutex(true, @"qVBF68RV5cLJM-$$_TEST_CHEAT_qVBF68RV5cLJM-$$", out var createdNew);
            if (!createdNew)
            {
                _instanceMutex = null;
                Environment.Exit(0);
                return;
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _instanceMutex?.ReleaseMutex();
            base.OnExit(e);
        }
    }
}
