﻿using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class UniversityService : IUniversityService<University>
    {
        private readonly IUserRepository<DbUser, UserDTO> _userRepository;
        private readonly IUniversityRepository<University, UniversityDTO> _universityRepository;
        private readonly IRepository<SpecialityToUniversity, SpecialityToUniversityDTO> _specialtyRepository;
        private readonly IRepository<DirectionToUniversity, DirectionToUniversityDTO> _directionRepository;
        private readonly IGraduateRepository<Graduate, GraduateDTO> _graduateRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;

        public UniversityService(
            IUserRepository<DbUser, UserDTO> userRepository,
            IUniversityRepository<University, UniversityDTO> universityRepository,
            IRepository<SpecialityToUniversity, SpecialityToUniversityDTO> specialtyRepository,
            IRepository<DirectionToUniversity, DirectionToUniversityDTO> directionRepository,
            IGraduateRepository<Graduate, GraduateDTO> graduateRepository,
            IMapper mapper,
            IPaginationService paginationService)
        {
            _userRepository = userRepository;
            _universityRepository = universityRepository;
            _specialtyRepository = specialtyRepository;
            _directionRepository = directionRepository;
            _graduateRepository = graduateRepository;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public void Dispose() => _universityRepository.Dispose();

        public async Task<IEnumerable<UniversityResponseApiModel>> GetUniversitiesByFilter(FilterApiModel filterModel)
        {
            // Filtered list of universities 
            var filteredUniversities = await _universityRepository.GetAll();

            if (filterModel.UniversityName != string.Empty && filterModel.UniversityName != null)
            {
                filteredUniversities = filteredUniversities.Where(x => x.Name == filterModel.UniversityName);
            }

            if (filterModel.UniversityAbbreviation != string.Empty && filterModel.UniversityAbbreviation != null)
            {
                filteredUniversities = filteredUniversities.Where(x => x.Abbreviation == filterModel.UniversityAbbreviation);
            }

            // Check if we must filter universities by direction name
            if (filterModel.DirectionName != string.Empty && filterModel.DirectionName != null)
            {
                // Get all directions by name
                var directions = await _directionRepository.Find(x => x.Direction.Name == filterModel.DirectionName);

                // Get universities by these directions
                filteredUniversities = directions.Select(x => x.University).ToList();
            }

            if (filterModel.SpecialityName != string.Empty && filterModel.SpecialityName != null)
            {
                // Get all specialities by name
                var specialities = await _specialtyRepository.Find(x => x.Speciality.Name == filterModel.SpecialityName);

                filteredUniversities = filteredUniversities.Where(x => specialities.Any(y => y.UniversityId == x.Id));
            }

            return _mapper.Map<IEnumerable<UniversityResponseApiModel>>(filteredUniversities.Distinct().ToList());
        }

        public async Task<UniversityResponseApiModel> GetUniversityById(string universityId, string userId = null)
        {
            var university = await _universityRepository.Get(universityId);

            if (university == null)
                throw new NotFoundException("Університету з таким id не існує.");

            var favoriteUniversities = await _universityRepository.GetFavoritesByUserId(userId);
            university.IsFavorite = favoriteUniversities.Where(fu => fu.Id == university.Id).Count() > 0;

            return _mapper.Map<UniversityResponseApiModel>(university);
        }

        public async Task<PageResponseApiModel<UniversityResponseApiModel>> GetUniversitiesPage(
            FilterApiModel filterModel,
            PageApiModel pageModel,
            string userId = null)
        {
            var universities = _mapper.Map<IEnumerable<UniversityResponseApiModel>>(await GetUniversitiesByFilter(filterModel));
            var result = new PageResponseApiModel<UniversityResponseApiModel>();

            if (universities == null || universities.Count() == 0)
                throw new NotFoundException("Університети не було знайдено.");

            try
            {
                result = _paginationService.GetPageFromCollection(universities, pageModel);
            }
            catch
            {
                throw;
            }

            // Set the value to favorites
            var favoriteUniversities = await _universityRepository.GetFavoritesByUserId(userId);
            foreach (var university in result.ResponseList)
            {
                university.IsFavorite = favoriteUniversities.Where(fu => fu.Id == university.Id).Count() > 0;
            }

            return result;
        }

        public async Task<IEnumerable<UniversityResponseApiModel>> GetFavoriteUniversities(string userId)
        {
            var favoriteUnivesistes = await _universityRepository.GetFavoritesByUserId(userId);
            if (favoriteUnivesistes == null || favoriteUnivesistes.Count() == 0)
            {
                throw new NotFoundException("Немає вибраних університетів.");
            }

            foreach (var university in favoriteUnivesistes)
            {
                university.IsFavorite = true;
            }

            return _mapper.Map<IEnumerable<UniversityResponseApiModel>>(favoriteUnivesistes);
        }

        public async Task<IEnumerable<string>> GetUniversityAbbreviations(FilterApiModel filterModel)
        {
            var university = await GetUniversitiesByFilter(filterModel);

            if (university == null || university.Count() == 0)
                throw new NotFoundException("Університети не було знайдено");

            return university
                .Select(u => u.Abbreviation)
                .Where(a => a != null)
                .OrderBy(a => a);
        }

        public async Task AddUniversityToFavorite(string universityId, string userId)
        {
            var favorites = await  _universityRepository.GetFavoritesByUserId(userId);
            var university = await _universityRepository.Get(universityId);
            var graduate = await _graduateRepository.GetByUserId(userId);

            if (favorites.Where(f => f.Id == universityId).Count() > 0)
                throw new BadRequestException("Даний університет вже додано до улюблених"); 

            if (university == null)
                throw new BadRequestException("Університет не було знайдено");

            if (graduate == null)
                throw new BadRequestException("Випускника не було знайдено");

            await _universityRepository.AddFavorite(new UniversityToGraduate
            {
                UniversityId = university.Id,
                GraduateId = graduate.Id
            });
        }

        public async Task DeleteUniversityFromFavorite(string universityId, string userId)
        {
            var favorites = await _universityRepository.GetFavoritesByUserId(userId);
            var university = await _universityRepository.Get(universityId);
            var graduate = await _graduateRepository.GetByUserId(userId);

            if (favorites.Where(f => f.Id == universityId).Count() == 0)
                throw new BadRequestException("Даний університет не було додано до улюблених");

            if (university == null)
                throw new BadRequestException("Університет не було знайдено");

            if (graduate == null)
                throw new BadRequestException("Випускника не було знайдено");

            await _universityRepository.RemoveFavorite(new UniversityToGraduate
            {
                UniversityId = university.Id,
                GraduateId = graduate.Id
            });
        }
    }
}
