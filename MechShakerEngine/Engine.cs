using MechShakerReader;

namespace MechShakerEngine;

public class Engine : IDisposable
{
    private readonly Audio  _audio                 = new();
    private          string _audioOutputDeviceName = "";

    private CancellationTokenSource? _listenerToken;
    private Task?                    _listenerTask;

    private CancellationTokenSource? _audioToken;
    private Task?                    _audioTask;
    private int                      _currentLatency;

    public async void Dispose()
    {
        _audioToken?.Cancel();
        _listenerToken?.Cancel();
        if (_listenerTask != null)
        {
            await _listenerTask;
            _listenerTask.Dispose();
        }

        if (_audioTask != null)
        {
            await _audioTask;
            _audioTask.Dispose();
        }
    }

    public void Initialise()
    {
        Logging.At(typeof(Engine)).Information("MechShakerEngine initialising...");
    }

    public void BeginListening()
    {
        if (_listenerTask != null)
            throw new InvalidOperationException("Listener already running");
        // TODO Need to know if this fails
        _listenerToken = new CancellationTokenSource();
        _listenerTask  = Task.Run(() => ListenToStream(_listenerToken.Token));
    }

    private async void ListenToStream(CancellationToken token)
    {
        var eventStream = Reader.Read(token);

        await foreach (var eventData in eventStream)
        {
            _audio.OnEventDataReceived(eventData);
        }
    }

    private void Start(string? deviceName, int latency)
    {
        _audioToken = new CancellationTokenSource();
        // TODO Need to know if this fails
        _audioTask = Task.Run(() => _audio.Start(deviceName, latency, _audioToken.Token));
    }

    private async void Stop()
    {
        _audioToken?.Cancel();
        if (_audioTask != null)
            await _audioTask;
    }

    public async void UpdateSettings(Settings.Settings settings)
    {
        _audio.UpdateSettings(settings);
        if (_audioOutputDeviceName == settings.OutputDevice && _currentLatency == settings.Latency)
            return;
        _audioOutputDeviceName = settings.OutputDevice;
        _currentLatency        = settings.Latency;
        await Task.Run(Stop);
        Start(settings.OutputDevice, settings.Latency);
    }

    public static IEnumerable<string> GetOutputDevices()
    {
        return Audio.GetOutputDevices().Select(o => o.FriendlyName);
    }
}