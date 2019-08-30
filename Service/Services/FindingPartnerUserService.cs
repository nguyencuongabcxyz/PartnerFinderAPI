using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Data.Models;
using Data.Repositories;
using Service.Extensions;
using Service.Models;

namespace Service.Services
{
    public interface IFindingPartnerUserService
    {
        Task<IEnumerable<FindingPartnerUserDto>> GetAll();
        Task<IEnumerable<FindingPartnerUserDto>> GetForPagination(int index, int size = 4);
        Task<int> Count();
    }
    public class FindingPartnerUserService : IFindingPartnerUserService
    {
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IFindingPartnerUserRepository _findingPartnerUserRepo;
        private readonly IMapper _mapper;

        public FindingPartnerUserService(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _userInformationRepo = repositoryFactory.CreateUserInformationRepo();
            _findingPartnerUserRepo = repositoryFactory.CreateFindingPartnerUserRepo();
            _mapper = mapper;
        }

        public async Task<int> Count()
        {
            var findPartnerUsers = await _findingPartnerUserRepo.GetManyByCondition(f => f.IsDeleted != true);
            return findPartnerUsers.Count();
        }

        public async Task<IEnumerable<FindingPartnerUserDto>> GetAll()
        {
            var findingPartnerPosts = await _findingPartnerUserRepo.GetAll();
            return await MapModelToDtoModel(findingPartnerPosts);
        }

        public async Task<IEnumerable<FindingPartnerUserDto>> GetForPagination(int index, int size = 4)
        {
            var findingPartnerPosts =
                await _findingPartnerUserRepo.GetRangeWithCondition(index, size, f => f.IsDeleted != true);
            return await MapModelToDtoModel(findingPartnerPosts);
        }

        private async Task<IEnumerable<FindingPartnerUserDto>> MapModelToDtoModel(
            IEnumerable<FindingPartnerUser> findingPartnerPosts)
        {
            var findingPartnerUserDtoList = new List<FindingPartnerUserDto>();
            foreach (var item in findingPartnerPosts)
            {
                try
                {
                    var userInfo = await _userInformationRepo.GetOne(item.UserId);
                    var findingPartnerUserDto = _mapper.Map<FindingPartnerUserDto>(item)
                        .Map(userInfo, _mapper);
                    findingPartnerUserDtoList.Add(findingPartnerUserDto);
                }
                catch (ObjectNotFoundException e)
                {
                    //TODO: log that exception when implement logger
                }
            }
            return findingPartnerUserDtoList.OrderByDescending(f => f.PostedDate);
        }

    }
}
