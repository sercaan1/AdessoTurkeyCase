using AdessoTurkey.Application.Interfaces.Services;

namespace AdessoTurkey.Application.Services
{
    public class RandomService : IRandomService
    {
        private readonly Random _random = new();

        public int Next() => _random.Next();
        public int Next(int maxValue) => _random.Next(maxValue);
        public int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);
    }
}
