using AutoMapper;
using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Repository;
namespace OrderProcessingBackEnd.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customers, CustomerDto>().ReverseMap(); //reverse map wil map model -Dto and DTO ->Model
            CreateMap<Orders, OrdersDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryWithProductDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryWithProductsDTO>().ReverseMap();
            //CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, CreateNewProductDTO>().ReverseMap();
            CreateMap<Product, ProductWithCategoryDTO>().ReverseMap();
            CreateMap<ProductWithCategoryDTO, Product>();
        //   .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryDTO));

            /*💡 Professional Approach
             
                Keep your DTOs slim and focused (e.g., don't expose sensitive or unnecessary DB fields).

              Then use a centralized MappingProfile like this:


              CreateMap<Customers, CustomerDto>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // optional if names match
               .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => src.Address + ", " + src.City)); // if needed

               CreateMap<CustomerDto, Customers>(); // ReverseMap also works
             */

            // Optionally: CreateMap<CustomerDto, Customers>(); // if needed in reverse
            //Use following 'var' in Service 
            //var customer = _mapper.Map<Customers>(customerDto); // Used in POST/PUT (write)
            //var dto = _mapper.Map<CustomerDto>(customer); // Used in GET (read)



            //1.Register automapper in program.cs file 
            // builder.Services.AddAutoMapper(typeof(Program)); // Or typeof(MappingProfile).Assembly
            //2.Add this in Service 
            /* private readonly IMapper _mapper;

          public CustomerService(IRepository<Customers> customerRepository, IMapper mapper)
          {
              _customerRepository = customerRepository;
              _mapper = mapper;
          }

      }*/
        }
    }
}
