using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Data.Models;
using Data.Repositories;
using Service.Extensions;
using Service.Models;

namespace Service.Services
{
    public interface IFindingPartnerUserService
    {
        Task<IEnumerable<FindingPartnerUserDto>> GetForPagination(string userId, int index, int size = 6);

        Task<IEnumerable<FindingPartnerUserDto>> GetForPaginationWithGivenUsers(IEnumerable<UserInformation> users, int index, int size = 6);
        Task<int> Count();
        Task<int> CountWithGivenUsers(IEnumerable<UserInformation> users);
    }
    public class FindingPartnerUserService : IFindingPartnerUserService
    {
        private readonly IUserInformationRepository _userInformationRepo;
        private readonly IFindingPartnerUserRepository _findingPartnerUserRepo;
        private readonly IMapper _mapper;

        public FindingPartnerUserService(IUserInformationRepository userInformationRepo, IFindingPartnerUserRepository findingPartnerUserRepo, IMapper mapper)
        {
            _userInformationRepo = userInformationRepo;
            _findingPartnerUserRepo = findingPartnerUserRepo;
            _mapper = mapper;
        }

        public async Task<int> Count()
        {
            return await _findingPartnerUserRepo.Count(f => f.IsDeleted != true);
        }

        public async Task<int> CountWithGivenUsers(IEnumerable<UserInformation> users)
        {
            var userIds = users.Select(user => user.UserId).ToList();
            Expression<Func<FindingPartnerUser, bool>> condition = (f) => f.IsDeleted != true && userIds.Contains(f.UserId);
            return await _findingPartnerUserRepo.Count(condition);
        }

        public async Task<IEnumerable<FindingPartnerUserDto>> GetForPagination(string userId, int index, int size = 6)
        {
            var findingPartnerPosts =
                await _findingPartnerUserRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending,
                    f => f.PostedDate, f => f.IsDeleted != true && f.UserId != userId);
            return await MapModelToDtoModel(findingPartnerPosts);
        }

        public async Task<IEnumerable<FindingPartnerUserDto>> GetForPaginationWithGivenUsers(IEnumerable<UserInformation> users, int index, int size = 6)
        {
            var userIds = users.Select(user => user.UserId).ToList();
            Expression<Func<FindingPartnerUser, bool>> condition = (f) => f.IsDeleted != true && userIds.Contains(f.UserId);

            var findingPartnerPosts = await _findingPartnerUserRepo.OrderAndGetRange(index, size, OrderType.OrderByDescending,
                f => f.PostedDate, condition );

            return await MapModelToDtoModel(findingPartnerPosts);
        }

        private async Task<IEnumerable<FindingPartnerUserDto>> MapModelToDtoModel(
            IEnumerable<FindingPartnerUser> findingPartnerPosts)
        {
            var findingPartnerUserDtoList = new List<FindingPartnerUserDto>();
            foreach (var item in findingPartnerPosts)
            {
                var userInfo = await _userInformationRepo.GetOne(item.UserId);
                var findingPartnerUserDto = _mapper.Map<FindingPartnerUserDto>(item)
                        .Map(userInfo, _mapper);
                findingPartnerUserDtoList.Add(findingPartnerUserDto);
            }
            return findingPartnerUserDtoList.OrderByDescending(f => f.PostedDate);
        }

    }
}
