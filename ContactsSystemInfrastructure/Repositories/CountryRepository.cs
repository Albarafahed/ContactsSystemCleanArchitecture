using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsSystemInfrastructure.Repositories
{
    public class CountryRepository(ContactsDbContext context) : ICountryRepository
    {
       public async Task<Country?> GetByIdAsync(int id)=> 
            await context.Countries.AsNoTracking().SingleOrDefaultAsync(c=> c.CountryId == id);
        public async Task<Country?> GetByIdForUpdateAsync(int id) => 
            await context.Countries.FindAsync(id);
        public async Task<Country?> GetByNameAsync(string name)=> 
            await context.Countries.AsNoTracking().SingleOrDefaultAsync(c => c.CountryName == name);
       public async Task<List<Country>> GetAllAsync()=>
            await context.Countries.AsNoTracking().ToListAsync();
        public async Task<bool> AddAsync(Country country)
        {
            await context.Countries.AddAsync(country);
            return await context.SaveChangesAsync() > 0;

        }
        public async Task<bool> SaveChangesAsync() =>
            await context.SaveChangesAsync() > 0;
       
        public async Task<bool> DeleteAsync(int id)=>
            await context.Countries.Where(c => c.CountryId == id).ExecuteDeleteAsync() > 0;
        public async Task<bool> ExistsByIdAsync(int id)=>
            await context.Countries.AnyAsync(c => c.CountryId == id);


        public async Task<bool> ExistsByNameAsync(string name) =>
                    await context.Countries.AnyAsync(c => c.CountryName == name);
    }
}
