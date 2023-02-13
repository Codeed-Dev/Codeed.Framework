using AutoMapper;
using System;

namespace Codeed.Framework.AspNet.Maps
{
    public class DateTimeMaps : Profile
    {
        public DateTimeMaps()
        {
            CreateMap<DateOnly, DateTime>()
                .ConvertUsing(dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue));

            CreateMap<DateOnly?, DateTime?>()
                .ConvertUsing(dateOnly => dateOnly.HasValue ? dateOnly.Value.ToDateTime(TimeOnly.MinValue) : null);

            CreateMap<DateOnly, DateTimeOffset>()
                .ConvertUsing(dateOnly => new DateTimeOffset(dateOnly.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero));

            CreateMap<DateOnly?, DateTimeOffset?>()
                .ConvertUsing(dateOnly => dateOnly.HasValue ? new DateTimeOffset(dateOnly.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero) : null);

        }
    }
}
