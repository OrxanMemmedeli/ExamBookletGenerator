namespace EBC.Core;

public static class ServiceOptions
{
    public static bool UseRateLimiting { get; private set; } = false;
    public static bool UseHangfire { get; private set; } = false;
    public static bool UseWatchDog { get; private set; } = false;
    public static bool UseHealthChecks { get; private set; } = false;
    public static bool UseMiniProfiler { get; private set; } = false;
    public static bool UseBackgroundService { get; private set; } = false;


    // ServiceOptions təyin etmək üçün statik bir metod yaradılır
    public static void Configure(bool useRateLimiting, bool useHangfire, bool useWatchDog, bool useHealthChecks, bool useMiniProfiler, bool useBackgroundService)
    {
        UseRateLimiting = useRateLimiting;
        UseHangfire = useHangfire;
        UseWatchDog = useWatchDog;
        UseHealthChecks = useHealthChecks;
        UseMiniProfiler = useMiniProfiler;
        UseBackgroundService = useBackgroundService;
    }
}
