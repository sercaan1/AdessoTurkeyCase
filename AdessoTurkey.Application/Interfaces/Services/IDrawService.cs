using AdessoTurkey.Application.DTOs;
using AdessoTurkey.Domain.Entities;

namespace AdessoTurkey.Application.Interfaces.Services
{
    public interface IDrawService
    {
        Task<DrawResponseDto> ExecuteDrawAsync(DrawRequestDto request);
        Task<List<DrawResponseDto>> GetAllDrawsAsync();
        Task<DrawResponseDto?> GetDrawByIdAsync(int id);
    }
}
