using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.Repository;
using OrderProcessingBackEnd.Services;
using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.DTO;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
namespace OrderProcessingBackEnd.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetCustomersAsync(
            Expression<Func<Customers, bool>> filter = null,
            Func<IQueryable<Customers>, IOrderedQueryable<Customers>> OrderBy = null,
            List<Expression<Func<Customers, object>>> includes = null,
           int? page = null,
           int? pageSize = null
            );
        //method to get customers with users table detail
        Task<IEnumerable<Customers>> GetCustomersWithUsersAsync(
          Expression<Func<Customers, bool>> filter = null,
          Func<IQueryable<Customers>, IOrderedQueryable<Customers>> OrderBy = null,
          List<Expression<Func<Customers, object>>> includes = null,
          int? page = null,
          int? pageSize = null
          );

        Task<CustomerDto> AddCustomers(CustomerDto customerDto);
        Task<CustomerDto> getCustomerById(int id);
        


    }
}
