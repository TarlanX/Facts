﻿using System.Collections.Generic;
using AutoMapper;
using Calabonga.UnitOfWork;

namespace Teagle.Facts.Web.Infrastructure.Mappers.Base
{
    public class PageListConverter<TSource, TDestination> : ITypeConverter<IPagedList<TSource>, IPagedList<TDestination>>
    {
        public IPagedList<TDestination> Convert(IPagedList<TSource> source, IPagedList<TDestination> destination, ResolutionContext context)
        {
            return source == null
                ? PagedList.Empty<TDestination>()
                : PagedList.From(source, x => context.Mapper.Map<IEnumerable<TDestination>>(x));
        }
    }
}