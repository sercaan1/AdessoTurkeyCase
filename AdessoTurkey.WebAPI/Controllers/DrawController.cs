using AdessoTurkey.Application.DTOs;
using AdessoTurkey.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AdessoTurkey.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DrawController : ControllerBase
    {
        private readonly IDrawService _drawService;
        private readonly IValidator<int> _idValidator;

        public DrawController(
            IDrawService drawService,
            IValidator<int> idValidator)
        {
            _drawService = drawService;
            _idValidator = idValidator;
        }

        /// <summary>
        /// Yeni bir kura çeker ve grupları oluşturur
        /// </summary>
        /// <param name="request">Kura çekme isteği (Çeken kişi adı, soyadı ve grup sayısı)</param>
        /// <returns>Oluşturulan gruplar ve takımlar</returns>
        /// <remarks>
        /// Örnek istek:
        /// 
        ///     POST /api/draw
        ///     {
        ///        "drawerFirstName": "Sercan",
        ///        "drawerLastName": "Karakuyu",
        ///        "numberOfGroups": 8
        ///     }
        ///     
        /// Not: numberOfGroups sadece 4 veya 8 olabilir.
        /// - 4 grup: Her grupta 8 takım (her ülkeden 1)
        /// - 8 grup: Her grupta 4 takım (4 ülkeden 1)
        /// </remarks>
        /// <response code="200">Kura başarıyla çekildi</response>
        /// <response code="400">Geçersiz istek (Validasyon hatası)</response>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<DrawResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResponse<DrawResponseDto>>> CreateDraw([FromBody] DrawRequestDto request)
        {
            Log.Information(
                "Kura çekme isteği alındı. Çeken: {DrawerFirstName} {DrawerLastName}, Grup Sayısı: {NumberOfGroups}",
                request.DrawerFirstName,
                request.DrawerLastName,
                request.NumberOfGroups
            );

            var result = await _drawService.ExecuteDrawAsync(request);

            Log.Information(
                "Kura başarıyla çekildi. Draw ID: {DrawId}, Çeken: {DrawerFullName}, Grup Sayısı: {GroupCount}",
                result.Id,
                result.DrawerFullName,
                result.Groups.Count
            );

            return Ok(BaseResponse<DrawResponseDto>.SuccessResult(result, "Kura başarıyla çekildi"));
        }

        /// <summary>
        /// Tüm kuraları listeler (En yeniden eskiye sıralı)
        /// </summary>
        /// <returns>Çekilmiş tüm kuralar</returns>
        /// <response code="200">Kuralar başarıyla listelendi</response>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<DrawResponseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<List<DrawResponseDto>>>> GetAllDraws()
        {
            Log.Information("Tüm kuralar listeleniyor");

            var draws = await _drawService.GetAllDrawsAsync();

            Log.Information("{Count} kura bulundu", draws.Count);

            return Ok(BaseResponse<List<DrawResponseDto>>.SuccessResult(draws, $"{draws.Count} kura bulundu"));
        }

        /// <summary>
        /// Belirli bir kurayı getirir
        /// </summary>
        /// <param name="id">Kura ID (pozitif sayı olmalı)</param>
        /// <returns>Kura detayları (Gruplar ve takımlar dahil)</returns>
        /// <response code="200">Kura başarıyla getirildi</response>
        /// <response code="400">Geçersiz ID (0 veya negatif)</response>
        /// <response code="404">Kura bulunamadı</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<DrawResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseResponse<DrawResponseDto>>> GetDrawById(int id)
        {
            var validationResult = await _idValidator.ValidateAsync(id);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                Log.Warning("Geçersiz ID ile kura getirme isteği: {DrawId}", id);
                return BadRequest(BaseResponse<DrawResponseDto>.FailureResult("Validasyon hatası", errors));
            }

            Log.Information("Kura getiriliyor. ID: {DrawId}", id);

            var draw = await _drawService.GetDrawByIdAsync(id);

            if (draw == null)
            {
                Log.Warning("Kura bulunamadı. ID: {DrawId}", id);
                return NotFound(BaseResponse<DrawResponseDto>.FailureResult(
                    "Kura bulunamadı",
                    $"ID'si {id} olan kura bulunamadı"));
            }

            Log.Information(
                "Kura bulundu. ID: {DrawId}, Çeken: {DrawerFullName}, Tarih: {DrawDate}",
                draw.Id,
                draw.DrawerFullName,
                draw.DrawDate
            );

            return Ok(BaseResponse<DrawResponseDto>.SuccessResult(draw, "Kura başarıyla getirildi"));
        }
    }
}
