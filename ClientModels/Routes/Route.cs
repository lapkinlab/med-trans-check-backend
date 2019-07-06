using ClientModels.Checkpoints;

namespace ClientModels.Routes
{
    public class Route
    {
        public string Id { get; set; }
        public string FromPlace { get; set; }
        public string ToPlace { get; set; }
        public Checkpoint[] Checkpoints { get; set; }
    }
}