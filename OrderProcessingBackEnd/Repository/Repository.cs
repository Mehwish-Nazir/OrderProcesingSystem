using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OrderProcessingBackEnd.AutoMapper;
using OrderProcessingBackEnd.Data;
using OrderProcessingBackEnd.DTO;
using OrderProcessingBackEnd.Models;
using OrderProcessingBackEnd.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
//using static OrderProcessingBackEnd.Repository.IRepository<T>;

//We will use this repo for all our DB tales(Cusomers, Orders,Trsanction) for our CRUD methods 
//I want some cutome methods we willl create our seperate repo file and define 
//our method into it not all CRUD again 
//this is called 'Hybrid appraoch'
//****************Example********************
//    public async Task<IEnumerable<Customers>> GetCustomersWithOrdersAsync()
//we will define separte Icustomer repo and customer repo file for this 
/*
      ***********ICusomerRep.cs*********************** 
public interface ICustomerRepository : IRepository<Customers>
{
    Task<IEnumerable<Customers>> GetCustomersWithOrdersAsync();
}

 *****************Customer Repo.cs****************
public class CustomerRepository : Repository<Customers>, ICustomerRepository
{
    private readonly OrderProcessingDbContext _context;

    public CustomerRepository(OrderProcessingDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customers>> GetCustomersWithOrdersAsync()
    {
        return await _context.Customers.Include(c => c.Orders).ToListAsync();   //linq query 
    }
}


 */

namespace OrderProcessingBackEnd.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly OrderProcessingDbContext _context;
        private readonly DbSet<T> _dbset;
        private readonly IMapper _mapper;
        public Repository(OrderProcessingDbContext context, IMapper mapper)
        {
            _context = context;
            _dbset = _context.Set<T>();
            _mapper = mapper;
        }

        //implemetation of all the methods we define in IRepo file 
        /* public async Task<IEnumerable<T>> GetAllAsync()
          {
            return await _dbset.ToListAsync();
           }
         //public async Task<IEnumerable<T>> GetAllAsync() => await _dbset.ToListAsync();
        */
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
               List<Expression<Func<T, object>>> includes = null,//used to include parent table data or another table data 
            int? page = null,
            int? pageSize = null
         )
        {
            IQueryable<T> query = _dbset;

            if (filter != null)
                query = query.Where(filter);  // WHERE clause

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                    //.Include is used to includ data of another table 
                }


            }
            if (orderBy != null)
                query = orderBy(query);       // ORDER BY clause

            if (page.HasValue && pageSize.HasValue)
                query = query.Skip(((int)page - 1) * (int)pageSize).Take((int)pageSize);  // Pagination
           // Page 1: Skip 0 records → (1 - 1) * 2 = 0            //
           
            return await query.ToListAsync(); // Execute SQL and return
        }


        public async Task<T> GetByIdAsync(int id)
         {
             return await _dbset.FindAsync(id);
         }
        /*public async Task<T> GetById(int id) => await _dbset.FindAsync(id);*/

        public async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
            await _context.SaveChangesAsync();

        }
        //public async Task AddAsync(T entity) => await _dbset.AddAsync(entity);

        public async Task UpdateAsync(T entity)
        {
            _dbset.Update(entity);
            await _context.SaveChangesAsync();
        }
        // public async Task UpdateAsync(T entity) => await _dbset.Update(entity);  //this inline exprssion will prodcue erro
        //    public void Update(T entity) => _dbSet.Update(entity);  //it will not produce errro bcz of 'void'



        public async Task DeleteAsync(T entity)
        {
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CustomerExistsByEmailAsync(string email)
        {
            return await _context.Customers.AnyAsync(c => c.Email == email);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> GetByName(string name)
        {
            return await _context.Category.AnyAsync(c => c.CategoryName.ToLower() == name.ToLower());
        }


        /* public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? page = null, int? pageSize = null)
         {
             throw new NotImplementedException();
         }
        */
        // public async Task DeleteAsync(T entity) => await _dbset.Remove(entity);

        /*public async Task SaveAync() => await _context.SaveChangesAsync();*/


        // ******For Category********
        public async Task<TDto> GetEntityWithRelatedDataAsync<TDto>(
      int id,
      Expression<Func<T, bool>> filter = null,
      List<Expression<Func<T, object>>> includes = null)
        {
            IQueryable<T> query = _dbset.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "CategoryID") == id);

            return _mapper.Map<TDto>(entity);
        }
        public async Task<bool> CheckCategoryExistenceByID(int id)
        {
            return await _context.Category.AnyAsync(c => c.CategoryID == id);
        }

        //**********Product Repo function****************
        public async Task<bool> ProductExistsAsync(string productName, int categoryId)
        {
            return await _context.Product.AnyAsync(p =>
                p.ProductName.ToLower().Trim() == productName.ToLower().Trim() &&
                p.CategoryID == categoryId);
        }

        public async Task<List<Product>> GetProductByCategoryID(int categoryID) 
        {
            var products = await _context.Product
                         .Where(p => p.CategoryID == categoryID)
                         .ToListAsync();
            return products;
        }

        //getting profile 
        public async Task<UserProfileDTO> GetDisplayNameWithRoleByUserIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
            {
                return new UserProfileDTO
                {
                    UserId = userId,
                    DisplayName = "Unknown User",
                    Role = "Unknown"
                };
            }

            string displayName;

            if (user.Role == "Customer")
            {
                //  If Customer entity is linked → use FirstName + LastName
                if (user.Customer != null)
                {
                    displayName = $"{user.Customer.FirstName} {user.Customer.LastName}";
                }
                else
                {
                    //  If role is "Customer" but no Customer entity → fallback to Username
                    displayName = user.Username;
                }
            }
            else
            { 
                //  Admin or any other role → just return Username
                displayName = user.Username;
            }

            return new UserProfileDTO
            {
                UserId = user.UserID,
                DisplayName = displayName,
                Role = user.Role
            };
        }

    }
}
