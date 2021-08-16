protected override void OnStartup(StartupEventArgs e)
{
    _mutex = new Mutex(true, "App_UniqueName", out bool createdNew);

    if (!createdNew)
    {
        MessageBox.Show("The application is already running","Warning",
                        MessageBoxButton.OK, 
                        MessageBoxImage.Information);
        Current.Shutdown();
    }
    base.OnStartup(e);
}