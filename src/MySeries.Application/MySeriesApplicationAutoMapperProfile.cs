using AutoMapper;
using MySeries.Books;
using MySeries.Notifications;
using MySeries.Series;
using MySeries.SerieService;

namespace MySeries;

public class MySeriesApplicationAutoMapperProfile : Profile
{
    public MySeriesApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Notification, NotificationDto>();
        CreateMap<NotificationDto, Notification>();
        CreateMap<Serie, SerieDto>();
        CreateMap<SerieDto, Serie>();

    }
}
