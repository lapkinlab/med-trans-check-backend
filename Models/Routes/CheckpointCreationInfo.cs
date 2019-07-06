using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Routes
{
    public class RouteCreationInfo
    {
        public string FromPlace { get; }
        public string ToPlace { get; }
        public IReadOnlyList<Guid> Checkpoints { get; }
        public RouteCreationInfo(string fromPlace, string toPlace, IEnumerable<Guid> checkpoints)
        {
            FromPlace = fromPlace ?? throw new ArgumentNullException(nameof(fromPlace));
            ToPlace = toPlace ?? throw new ArgumentNullException(nameof(fromPlace));
            Checkpoints = checkpoints?.ToArray() ?? throw new ArgumentNullException(nameof(checkpoints));
        }
    }
}