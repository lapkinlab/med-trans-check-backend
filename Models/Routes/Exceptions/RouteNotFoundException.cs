using System;

namespace Models.Routes.Exceptions
{
    public class RouteNotFoundException : Exception
    {
        public RouteNotFoundException(string routeId)
            : base($"Route \"{routeId}\" not found.")
        {
        
        }
    }
}