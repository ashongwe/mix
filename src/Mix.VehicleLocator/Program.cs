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
foreach(Coordinate coordinate in coordinates)
{
    VehicleLocation closestVehicle = vehicleLocations.OrderBy(v => v.DistanceTo(coordinate.Latitude, coordinate.Longitude)).First();
    Console.WriteLine(closestVehicle.Summary());
}
watch.Stop();
long findingClosestTime = watch.ElapsedMilliseconds;
Console.WriteLine($"Data File Read Time: {readTime}ms and Comparing Time: {findingClosestTime}ms and Total Time {readTime + findingClosestTime}ms");
Console.ReadLine();
