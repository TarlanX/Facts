using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Base;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Teagle.Facts.Web.Data;
using Teagle.Facts.Web.ViewModels;

namespace Teagle.Facts.Web.Controllers.Facts.Queries
{
    public class FactGetPagedRequest : OperationResultRequestBase<IPagedList<FactViewModel>>
    {
        public FactGetPagedRequest(int pageIndex, string? tag, string? search)
        {
            PageIndex = pageIndex;
            Tag = tag;
            Search = search;
        }

        public int PageIndex { get;}
        public int PageSize => 20;

        public string? Tag { get; }

        public string? Search { get; }
    }
    public class FactGetPagedHandler: OperationResultRequestHandlerBase<FactGetPagedRequest, IPagedList<FactViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public FactGetPagedHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public override async Task<OperationResult<IPagedList<FactViewModel>>> Handle(FactGetPagedRequest request, CancellationToken cancellationToken)
        {
            var operationResult = OperationResult.CreateResult<IPagedList<FactViewModel>>();

            var items = await _unitOfWork.GetRepository<Fact>()
                .GetPagedListAsync(
                    include: i => i.Include(x => x.Tags),
                    orderBy: o => o.OrderByDescending(x => x.CreatedAt),
                    pageIndex: request.PageIndex,
                    pageSize:request.PageSize,
                    cancellationToken: cancellationToken
                    );

            var mapped = _mapper.Map<IPagedList<FactViewModel>>(items);

            operationResult.Result = mapped;
            operationResult.AddSuccess("Succes");

            return operationResult;
        }
    }
}