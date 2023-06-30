using System.Text;
namespace Mix.VehicleLocator
{
    internal class VehicleLocation: IComparable
    {
        internal int VehicleId { get; private set; }
        internal string VehicleRegistration { get; private set; }
        internal float Latitude { get; private set; }
        internal float Longitude { get; private set; }
        internal DateTime RecordTimeUtc { get; private set; }
        internal VehicleLocation(int vehicleId, string vehicleRegistration, float latitude, float longitude, DateTime recordTimeUtc)
        {
            VehicleId = vehicleId;
            VehicleRegistration = vehicleRegistration;
            Latitude = latitude;
            Longitude = longitude;
            RecordTimeUtc = recordTimeUtc;
        }
        internal static VehicleLocation GetVehicleLocation(byte[] buffer, ref int offset)
        {
            int vehicleId = BitConverter.ToInt32(buffer, offset);
            Interlocked.Add(ref offset, 4);
            var registrationNumberBuilder = new StringBuilder();
            while (buffer[offset] != 0)
            {
                registrationNumberBuilder.Append((char)buffer[offset]);
                Interlocked.Add(ref offset, 1);
            }
            string registrationNumber = registrationNumberBuilder.ToString();
            Interlocked.Add(ref offset, 1);
            float latitude = BitConverter.ToSingle(buffer, offset);
            Interlocked.Add(ref offset, 4);
            float longitude = BitConverter.ToSingle(buffer, offset);
            Interlocked.Add(ref offset, 4);
            ulong recordedTimeSeconds = BitConverter.ToUInt64(buffer, offset);
            DateTime recordedTimeUtc = EpochHelper.ToUtcDateTime(recordedTimeSeconds);
            Interlocked.Add(ref offset, 8);
            return new VehicleLocation(vehicleId, registrationNumber, latitude, longitude, recordedTimeUtc);
        }
        internal int DistanceTo(double latitude, double longitude)
        {

            if (double.IsNaN(Latitude) || double.IsNaN(Longitude) || double.IsNaN(latitude) || double.IsNaN(longitude))
            {
                throw new ArgumentException("Argument latitude or longitude is not a number");
            }
            double num = Latitude * (Math.PI / 180.0);
            double num2 = Longitude * (Math.PI / 180.0);
            double num3 = latitude * (Math.PI / 180.0);
            double num4 = longitude * (Math.PI / 180.0) - num2;
            double num5 = Math.Pow(Math.Sin((num3 - num) / 2.0), 2.0) + Math.Cos(num) * Math.Cos(num3) * Math.Pow(Math.Sin(num4 / 2.0), 2.0);
            return (int)( 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(num5), Math.Sqrt(1.0 - num5))));
        }
        internal string Summary()
        {
            return $"Vehicle ID: {VehicleId} Registration: {VehicleRegistration} " +
                $"Latitude: {Latitude} Longitude: {Longitude} Recorded Time: {RecordTimeUtc}";
        }

        public int CompareTo(object? obj)
        {
            if (obj == null) return int.MaxValue;
            VehicleLocation? coordinate = obj as VehicleLocation;
            if (coordinate == null) return int.MaxValue;
            return DistanceTo(coordinate.Latitude, coordinate.Longitude);
        }
    }
}
