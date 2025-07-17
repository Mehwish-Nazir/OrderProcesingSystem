using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.Services;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.DTO;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using OrderProcessingBackEnd.AutoMapper;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
namespace OrderProcessingBackEnd.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customers> _customerRepository;
        private readonly IMapper _mapper;
        //private readonly OrderProcessingDbContext _context;  //db context will not be used here for clean approach as it is used in geenral reposiory 

        //here we use the ISERVICE method name
        public CustomerService(IRepository<Customers> customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CustomerDto>> GetCustomersAsync(
            Expression<Func<Customers, bool>> filter = null,
            Func<IQueryable<Customers>, IOrderedQueryable<Customers>> OrderBy = null,
            List<Expression<Func<Customers, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            var customers = await _customerRepository.GetAllAsync(
                filter: filter,
                orderBy: OrderBy,
                includes: includes,
                page: page,
                pageSize: pageSize
                );
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<IEnumerable<Customers>> GetCustomersWithUsersAsync(
            Expression<Func<Customers, bool>> filter = null,
            Func<IQueryable<Customers>, IOrderedQueryable<Customers>> OrderBy = null,
            List<Expression<Func<Customers, object>>> includes = null,
            int? page = null,
            int? pageSize = null

            )
        {
            //here we use the Irepo method name 
            //'includes' is the paramneter we use in IRepo file to include the data of Users table
            includes ??= new List<Expression<Func<Customers, object>>>();
            includes.Add(c => c.User);
            var customers = await _customerRepository.GetAllAsync(filter: filter,
               orderBy: OrderBy,
               includes: includes,
               page: page,
               pageSize: pageSize
               ); 
            return customers;

            //var customer = _mapper.Map<Customers>(customerDto); // Used in POST/PUT (write)
            //var dto = _mapper.Map<CustomerDto>(customer); // Used in GET (read)


            /*
             * steps to map customer to DTO 
             * 1.create maping profile that inherit from Profile 
             * using AutoMapper;
                using OrderProcessingBackEnd.Models;
                 using OrderProcessingBackEnd.DTO;

                public class MappingProfile : Profile
                {
                       public MappingProfile()
                     {
                       CreateMap<Customers, CustomerDto>();
                  // Optionally: CreateMap<CustomerDto, Customers>(); // if needed in reverse
                 }
                }
            2️. Register AutoMapper in Program.cs
            builder.Services.AddAutoMapper(typeof(Program)); // Or typeof(MappingProfile).Assembly
            3. Inject IMapper in Your Service
            
            private readonly IMapper _mapper;
            public CustomerService(IRepository<Customers> customerRepository, IMapper mapper)
            {
             _customerRepository = customerRepository;
              _mapper = mapper;
            }
            4.  Use AutoMapper in Your Method ✅
             var customers = await _customerRepository.GetAllAsync(filter, orderBy, page, pageSize);
             return _mapper.Map<IEnumerable<CustomerDto>>(customers);

             */


        }
        public async Task<CustomerDto> getCustomerById(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (id <= 0)
            {
                throw new ArgumentException("Invalid customr ID , Id must be greate than 0");
            }
            if (customer == null)
            {
                return null;
            }

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> AddCustomers(CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }
            var exists = await _customerRepository.CustomerExistsByEmailAsync(customerDto.Email);
            if (exists)
            {
                throw new InvalidOperationException("A customer with this email already exists.");

            }

            /*//if Id is AutoMapAttribute genrared then don't check customer exitnse based on id'
                     var existingCustomer = await _customerRepository.GetByIdAsync(customerDto.CustomerID);
                     if (existingCustomer != null)
                     {
                         throw new InvalidOperationException($"Customer with id {customerDto.CustomerID} already exists");
                     }*/
            //afore saving map Dto to Model
            //DTO to Entity:


            //Ensures that the client's input is converted into a format that your application and database can understand.
            Customers customer;
            try
            {
                //check that Model and DTO match or not 
                customer = _mapper.Map<Customers>(customerDto);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Mapping failed: " + ex.Message);
            }
            // var customer = _mapper.Map<Customers>(customerDto); //this var 'customer' act as Customers model
            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveAsync();
            //After adding 
            //Convert Entity to Dto:
            var resultDto = _mapper.Map<CustomerDto>(customer);
            return resultDto;
        }
       



    }
}
