using Serilog;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace GlobalTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILogger _logger;


        public MainWindow()
        {
#if DEBUG
            LogBuilder.Initialize("YSGlobalTime", "GlobalTime-debug");
#else
            LogBuilder.Initialize("YSGlobalTime", "GlobalTime");
#endif
            _logger ??= Log.ForContext<MainWindow>();
            InitializeComponent();

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            if (fvi != null && fvi.FileVersion != null)
            {
                string version = fvi.FileVersion;
                Title = $"{Title} {version}";
            }

            _logger.Debug("MainWindow initialized");

            FillData();
        }

        private async void FillData()
        {
            var currentCursor = TimeListView.Cursor;
            TimeListView.Cursor = Cursors.Wait;

            List<TimeItem> timeItems = await LoadTimeItems();

            Dispatcher.Invoke(new Action(() =>
            {
                TimeListView.ItemsSource = timeItems;
            }));

            TimeListView.Cursor = currentCursor;
        }

        private async Task<List<TimeItem>> LoadTimeItems()
        {
            Configuration? configuration = Configuration.ReadConfiguration();
            var timeItems = new List<TimeItem>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", configuration?.ApiKey);
                List<Task<TimeItem>> fetchTasks = new List<Task<TimeItem>>();
                if (configuration != null && configuration.Cities != null)
                {
                    foreach (var cityConfig in configuration.Cities)
                    {
                        fetchTasks.Add(FetchTimeForCityAsync(httpClient, cityConfig));
                    }
                }

                await Task.WhenAll(fetchTasks);
                foreach (var task in fetchTasks)
                {
                    timeItems.Add(task.Result);
                }
                return timeItems;
            }
        }

        private Task<TimeItem> FetchTimeForCityAsync(HttpClient httpClient, CityConfig cityConfig)
        {
            return Task.Run(() =>
            {
                try
                {
                    var response = httpClient.GetAsync($"https://api.api-ninjas.com/v1/timezone?timezone={cityConfig.TimeZone}").Result;

                    response.EnsureSuccessStatusCode();


                    var currentTime = JsonSerializer.Deserialize<CurrentTime>(response.Content.ReadAsStringAsync().Result);

                    response.Dispose();

                    return new TimeItem(cityConfig.Name ?? "Unknown", currentTime?.LocalTime ?? "Error");
                }
                catch (Exception ex)
                {
                    _logger.Error("Error fetching time for {City}: {Message}", cityConfig.Name, ex.Message);
                    return new TimeItem(cityConfig.Name ?? "Unknown", "Error fetching time");
                }
            });
        }
    }
}