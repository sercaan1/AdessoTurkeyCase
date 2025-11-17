namespace AdessoTurkey.Application.Interfaces.Services
{
    public interface IRandomService
    {
        int Next();
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }
}
