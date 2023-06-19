namespace Mix.VehicleLocator
{
    internal class DataReader
    {
        private readonly string _dataFilePath;
        internal DataReader()
        {
            string assemblyLocation = typeof(DataReader).Assembly.Location;
            _dataFilePath = Path.Combine(assemblyLocation.Substring(0, assemblyLocation.LastIndexOf("\\")), "VehiclePositions.dat");
        }
        internal async Task<byte[]?> ReadData()
        {
            if (!File.Exists(_dataFilePath))
            {
                Console.WriteLine("Could not find data file.");
                return null;
            }
            return await File.ReadAllBytesAsync(_dataFilePath);
        }
       
        internal  async Task<List<VehicleLocation>> ReadVehicleLocations()
        {
            byte[]? buffer = await  ReadData();
            if (buffer == null) return new();
            int offset = 0;
            var vehicleLocations = new List<VehicleLocation>();
            while(offset < buffer.Length)
            {
                vehicleLocations.Add(VehicleLocation.GetVehicleLocation(buffer, ref offset));
            }
            return vehicleLocations;
        }

        internal List<Coordinate> GetLocations()
        {
            return new List<Coordinate>
            {
                new Coordinate(34.544909,-102.100843),
                new Coordinate(32.345544,-99.123124),
                new Coordinate(33.234235,-100.214124),
                new Coordinate(35.195739,-95.348899),
                new Coordinate(31.895839,-97.789573),
                new Coordinate(32.895839,-101.789573),
                new Coordinate(34.115839,-100.225732),
                new Coordinate(32.335839,-99.992232),
                new Coordinate(33.535339,-94.792232),
                new Coordinate(32.234235,-100.222222)
            };
        }
    }
}
