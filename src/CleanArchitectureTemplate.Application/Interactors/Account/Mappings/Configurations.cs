using AutoMapper;
using CleanArchitectureTemplate.Application.Interactors.Account.Outputs;
using CleanArchitectureTemplate.Domain.Customers;

namespace CleanArchitectureTemplate.Application.Interactors.Account.Mappings
{
    public class Configurations : Profile
    {
        public Configurations()
        {
            CreateMap<Customer, AuthenticationOutput>();
            CreateMap<Customer, RegistrationOutput>();
            CreateMap<Customer, ProfileOutput>();
        }
    }
}
