using System;
using AutoMapper;
using Ordering_App.Models.Domain;
using Ordering_App.Models.DTOs;

namespace Ordering_App.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        //Employee mappings
        CreateMap<AddEmployeeDTO, Employee>().ReverseMap();
        CreateMap<UpdateEmployeeDTO, Employee>().ReverseMap();

        //Restaurant mappings
        CreateMap<AddRestaurantDTO, Restaurant>().ReverseMap();

        //MenuItem mappings
        CreateMap<AddMenuItemDTO, MenuItem>().ReverseMap();
    }
}
