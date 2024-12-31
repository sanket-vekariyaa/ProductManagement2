using Microsoft.EntityFrameworkCore;
using ProductManagement.Buisness;
using ProductManagement.Data;
using ProductManagement.Model;
using ProductManagement.Providers;

namespace ProductManagement.Business
{
    public class ProductService : GlobalVariables
    {
        DefaultContext _context = new DefaultContext(new Connection());
        public async Task<Response> Save(Products data)
        {
            Response response = new() { Status = (byte)StatusFlags.Success };
            try
            {
                if (data.Id == 0 && !await _context.Product.AsNoTracking().AnyAsync(d => d.ProductCode == data.ProductCode)) { await _context.Product.AddAsync(data); }
                else if (data.Id != 0 && !await _context.Product.AsNoTracking().AnyAsync(d => d.ProductCode == data.ProductCode && d.Id != data.Id)) { _context.Product.Update(data); }
                else { response.Status = (byte)StatusFlags.AlreadyExists; }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {response.Status = (byte)StatusFlags.Failed; response.DetailedError = Convert.ToString(ex); }
            return response;
        }

        public async Task<Response> Delete(int id)
        {
            Response response = new() { Status = (byte)StatusFlags.Success };
            try
            {
                Products data = await _context.Product.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null) { _context.Product.Remove(data); await _context.SaveChangesAsync(); }
                else { response.Status = (byte)StatusFlags.Failed; }
            }
            catch (Exception ex) { response.Status = (byte)StatusFlags.Failed; response.DetailedError = Convert.ToString(ex); }
            return response;
        }

        public async Task<List<Category>> SaveProductCategories()
        {
            if(!_context.Category.Any()) // we will move this code into seed method
            {
                _context.Category.Add(new Category { Name = "Mens's Fashion"});
                _context.Category.Add(new Category { Name = "Women's Fashion"});
                await _context.SaveChangesAsync();
            }
            return await _context.Category.ToListAsync();
        }
    }

}
