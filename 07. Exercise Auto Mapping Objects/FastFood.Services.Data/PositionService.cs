

using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.Data
{
    using AutoMapper;

    using Models;
    using FastFood.Data;
    using Web.ViewModels.Positions;

    public class PositionService : IPositionService
    {
        private readonly IMapper mapper;
        private readonly FastFoodContext context;

        public PositionService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task CreateAsync(CreatePositionInputModel inputModel)
        {
            Position position = this.mapper.Map<Position>(inputModel);
            await this.context.Positions.AddAsync(position);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PositionsAllViewModel>> GetAllAsync()
            => await this.context.Positions
                .ProjectTo<PositionsAllViewModel>(this.mapper.ConfigurationProvider)
                .ToArrayAsync();
    }
}
