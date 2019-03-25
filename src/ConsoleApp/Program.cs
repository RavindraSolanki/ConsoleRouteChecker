using System;
using System.Linq;
using RouteChecker.RoadServices;

namespace RouteChecker.ConsoleApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var roadId = args[0];

            var roadStatusService = new RoadStatusService();

            try
            {
                var roadStatus = roadStatusService.GetStatus(roadId).Result;

                Console.WriteLine($"The status of the {roadStatus.DisplayName} is as follows");
                Console.WriteLine($"\tRoad Status is {roadStatus.StatusSeverity}");
                Console.WriteLine($"\tRoad Status Description is {roadStatus.StatusSeverityDescription}");

                return 0;
            }
            catch (AggregateException aggrEx)
            {
                
                if (aggrEx.InnerExceptions.Any(_ => _ is RoadNotFoundException))
                {
                    Console.WriteLine($"{roadId} is not a valid road");
                }
                else
                {
                    throw aggrEx;
                }

                return 1;
            }
        }
    }
}
