namespace Mix.VehicleLocator
{
    internal static class EpochHelper
    {
        internal static DateTime ToUtcDateTime(ulong seconds)
        {
            return DateTime.UnixEpoch.AddSeconds(seconds);
        }
    }
}
