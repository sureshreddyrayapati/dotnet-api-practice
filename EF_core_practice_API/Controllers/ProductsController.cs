using EF_core_practice_API.Model;
using EF_core_practice_API.Model.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EF_core_practice_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]       
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(AppDbContext context,ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [Route("api/Products/GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll(string? search,
            decimal? maxPrice,
            decimal? minPrice,
            int pageNumber=1,int pageSize=10
            )
        {
            if (pageNumber < 1) 
            { 
                pageNumber = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 1;
            }
            var query=_context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query=query.Where(p=>p.Name.Contains(search));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var totalRecords = await query.CountAsync();
            //page maximum size 
            if (pageSize>50)
                pageSize= 50;

            _logger.LogInformation("fetching All Products");
            //var totalRecords = await _context.Products.CountAsync();
            var products = await query
                .AsNoTracking()
                .OrderBy(p=>p.Id)
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();
            _logger.LogInformation("Fetched {Count} products for page {PageNumber}",products.Count, pageNumber);
            var response = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Data = products

            };
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product=await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            //var products = await _context.Products.AnyAsync(x => x.Name == product.Name);
            //if (products)
            //{
            //    return BadRequest("Product already exist");
            //}
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById),
                  new { id = product.Id }, product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest("ID mismatched");
            var existing_product=await _context.Products.FindAsync(id);
            if (existing_product == null)
                return NotFound();
            existing_product.Name = product.Name;
            existing_product.Price  = product.Price;
            existing_product.Description = product.Description;
            existing_product.Stock = product.Stock;

            await _context.SaveChangesAsync();
            return NoContent();
            
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var product=await _context.Products.FindAsync(id);
            if(product == null)
                return NotFound();
            product.IsDeleted= true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var product = await _context.Products
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();
            product.IsDeleted = false;
            await _context.SaveChangesAsync();
            return Ok("Product Restored");

        }
        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("Testing middleware");
        }

    }
}
