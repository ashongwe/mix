// See https://aka.ms/new-console-template for more information
using Mix.VehicleLocator;
using System.Diagnostics;

var dataReader = new DataReader();
var watch = new Stopwatch();
watch.Start();
List<VehicleLocation> vehicleLocations = await dataReader.ReadVehicleLocations();
watch.Stop();
long readTime = watch.ElapsedMilliseconds;
List<Coordinate> coordinates = dataReader.GetLocations();
watch.Restart();
Parallel.ForEach(coordinates, coordinate =>
{    
    VehicleLocation? closestVehicle = null;
    double longestDistance = double.MaxValue;
    Parallel.ForEach(vehicleLocations, (vehicleLocation) =>
    {
        var distanceBetween = vehicleLocation.DistanceTo(coordinate.Latitude, coordinate.Longitude);
        if (distanceBetween < longestDistance)
        {
            closestVehicle = vehicleLocation;
            longestDistance = distanceBetween;            
        }
    });
    if(closestVehicle != null) Console.WriteLine(closestVehicle.Summary());
});
watch.Stop();
long findingClosestTime = watch.ElapsedMilliseconds;
Console.WriteLine($"Data File Read Time: {readTime}ms and Comparing Time: {findingClosestTime}ms and Total Time {readTime + findingClosestTime}ms");
Console.ReadLine();
