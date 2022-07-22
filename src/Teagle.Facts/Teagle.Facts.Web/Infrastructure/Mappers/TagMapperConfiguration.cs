using Teagle.Facts.Web.Data;
using Teagle.Facts.Web.Infrastructure.Mappers.Base;
using Teagle.Facts.Web.ViewModels;

namespace Teagle.Facts.Web.Infrastructure.Mappers
{
    public class TagMapperConfiguration: MapperConfigurationBase
    {
        public TagMapperConfiguration()
        {
            CreateMap<Tag, TagViewModel>();
            CreateMap<TagUpdateViewModel, Tag>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.Facts, o => o.Ignore());
     
            CreateMap<Tag, TagUpdateViewModel> ();
        }
    }
}