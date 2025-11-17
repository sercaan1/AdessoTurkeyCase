using AdessoTurkey.Application.DTOs;
using AdessoTurkey.Application.Interfaces;
using AdessoTurkey.Application.Interfaces.Services;
using AdessoTurkey.Domain.Entities;
using AutoMapper;

namespace AdessoTurkey.Application.Services
{
    public class DrawService : IDrawService
    {
        private const int EXPECTED_COUNTRIES = 8;
        private const int TEAMS_PER_COUNTRY = 4;
        private static readonly string[] GROUP_NAMES = { "A", "B", "C", "D", "E", "F", "G", "H" };
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRandomService _random;

        public DrawService(IUnitOfWork unitOfWork, IMapper mapper, IRandomService random)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _random = random;
        }

        public async Task<DrawResponseDto> ExecuteDrawAsync(DrawRequestDto request)
        {
            var allTeams = await _unitOfWork.TeamRepository.GetAllAsync();
            var teamsByCountry = allTeams.GroupBy(t => t.Country).ToDictionary(g => g.Key, g => g.ToList());

            ValidateTeamData(teamsByCountry);

            var groupDistribution = DistributeTeamsToGroups(teamsByCountry, request.NumberOfGroups);

            var draw = _mapper.Map<Draw>(request);

            draw.Groups = groupDistribution.Select(kvp => new DrawGroup
            {
                GroupName = kvp.Key,
                CreatedAt = DateTime.UtcNow,
                Teams = kvp.Value.Select(team => new DrawTeam
                {
                    TeamId = team.Id,
                    CreatedAt = DateTime.UtcNow
                }).ToList()
            }).ToList();

            await _unitOfWork.DrawRepository.AddAsync(draw);
            await _unitOfWork.SaveChangesAsync();

            return new DrawResponseDto
            {
                Id = draw.Id,
                DrawerFirstName = draw.DrawerFirstName,
                DrawerLastName = draw.DrawerLastName,
                DrawerFullName = $"{draw.DrawerFirstName} {draw.DrawerLastName}",
                NumberOfGroups = draw.NumberOfGroups,
                DrawDate = draw.DrawDate,
                Groups = groupDistribution.Select(kvp => new DrawGroupResponseDto
                {
                    GroupName = kvp.Key,
                    Teams = kvp.Value.Select(team => new DrawTeamResponseDto
                    {
                        Name = team.Name
                    }).ToList()
                }).ToList()
            };
        }

        private static void ValidateTeamData(Dictionary<string, List<Team>> teamsByCountry)
        {
            if (!teamsByCountry.Any())
                throw new InvalidOperationException("Sistemde hiç takım bulunamadı.");

            if (teamsByCountry.Count != EXPECTED_COUNTRIES)
                throw new InvalidOperationException($"Sistemde {EXPECTED_COUNTRIES} ülke olmalı, ancak {teamsByCountry.Count} ülke bulundu.");

            var invalidCountries = teamsByCountry.Where(kvp => kvp.Value.Count != TEAMS_PER_COUNTRY).ToList();
            if (invalidCountries.Any())
            {
                var errors = string.Join(", ", invalidCountries.Select(kvp => $"{kvp.Key}: {kvp.Value.Count} takım"));
                throw new InvalidOperationException($"Her ülkede {TEAMS_PER_COUNTRY} takım olmalı. Hatalı ülkeler: {errors}");
            }
        }

        private Dictionary<string, List<Team>> DistributeTeamsToGroups(
            Dictionary<string, List<Team>> teamsByCountry,
            int numberOfGroups)
        {
            var teamsPerGroup = numberOfGroups == 4 ? 8 : 4;

            var countryTeamQueues = teamsByCountry.ToDictionary(
                kvp => kvp.Key,
                kvp => new Queue<Team>(kvp.Value.OrderBy(_ => _random.Next()))
            );

            var groupNames = GROUP_NAMES.Take(numberOfGroups).ToArray();
            var groupTeams = groupNames.ToDictionary(name => name, _ => new List<Team>());
            var groupCountries = groupNames.ToDictionary(name => name, _ => new HashSet<string>());

            var allCountries = teamsByCountry.Keys.ToArray();
            var totalSlots = numberOfGroups * teamsPerGroup;

            for (int slot = 0; slot < totalSlots; slot++)
            {
                var groupName = groupNames[slot % numberOfGroups];

                var availableCountry = allCountries.FirstOrDefault(country =>
                    !groupCountries[groupName].Contains(country) &&
                    countryTeamQueues[country].Count > 0);

                if (availableCountry == null)
                    throw new InvalidOperationException("Kura çekilemedi: Uygun takım bulunamadı");

                var team = countryTeamQueues[availableCountry].Dequeue();
                groupTeams[groupName].Add(team);
                groupCountries[groupName].Add(availableCountry);
            }

            return groupTeams;
        }

        public async Task<List<DrawResponseDto>> GetAllDrawsAsync()
        {
            var draws = await _unitOfWork.DrawRepository.GetAllAsync();
            return _mapper.Map<List<DrawResponseDto>>(draws);
        }

        public async Task<DrawResponseDto?> GetDrawByIdAsync(int id)
        {
            var draw = await _unitOfWork.DrawRepository.GetByIdAsync(id);
            return draw == null ? null : _mapper.Map<DrawResponseDto>(draw);
        }
    }
}
