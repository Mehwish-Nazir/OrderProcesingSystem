
using Microsoft.EntityFrameworkCore;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace OrderProcessingBackEnd.Repository
{
    public interface IRepository<T> where T : class
    {
        //Task<IEnumerable<T>> GetAllAsync();   //<IEnumerable> is used to return collection of objects 
        //profesional way to write get repo function
        Task<IEnumerable<T>> GetAllAsync(
    Expression<Func<T, bool>> filter = null,    //filter keyword is used to filter data using where clause &&=null mens if not applying filtering it will return data 
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    List<Expression<Func<T, object>>> includes = null,// it include the data of another table
    int? page = null,
    int? pageSize = null
);
        /*
          This is a generic delegate type in C# called Func.
        Func<in TInput, out TResult>  map on ***Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,***
        IQueryable<T> = Input type
        IOrderedQueryable<T> = Return type (output)
        q => q.OrderBy(x => x.Name)
        OrderBy(x => x.Name))
      //  📌 It’s actually a Func<T, TKey> — where T is the entity, and TKey is the sort key.
        OrderBy(x => x.Name))
        //delwgate 
        Func<int, int, int> add = (a, b) => a + b;
        int result = add(5, 3);  // result = 8
This is equivalent to defining a delegate for (int, int) => int.

         */
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);  //update and delete can be task not Task<T> bcz in real world they doen't return 
        Task SaveAsync();   //save changes in database  
        Task<bool> CustomerExistsByEmailAsync(string email);


        Task<bool> GetByName(string name);

        //These method must be written to sepearte ategoryRepository file professionally
        //****Category Repo *********
        //To get Category with related products
        Task<TDto> GetEntityWithRelatedDataAsync<TDto>(
     int id,
     Expression<Func<T, bool>> filter = null,
     List<Expression<Func<T, object>>> includes = null);


        //can be different file in large application 
        Task<bool> CheckCategoryExistenceByID(int id);
        //***********Product Repo ***********
        Task<bool> ProductExistsAsync(string productName, int categoryId);
        Task<List<Product>> GetProductByCategoryID(int categoryID);

        //Getting Profile nam of user "Customer/Admin"
        Task<UserProfileDTO> GetDisplayNameWithRoleByUserIdAsync(int userId);

    }



}
