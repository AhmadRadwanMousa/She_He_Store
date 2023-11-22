using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using She_He_Store.Helpers;
using She_He_Store.Models;

namespace She_He_Store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webhost;

        public ProductsController(ModelContext context, IWebHostEnvironment webhost)
        {
            _context = context;
            _webhost = webhost;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Products.Include(p => p.Category);
            return View(await modelContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Productid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Select(temp => new SelectListItem { Text = temp.Categoryname, Value = temp.Categoryid.ToString() });
            List<SelectListItem> selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text ="Exist",Value="true"},
                new SelectListItem { Text = "Not Exist",Value ="false"},
            };
            ViewBag.IsAvailable = selectList; 
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                        string? imagefile = await ImageUploader.UploadeImage(_webhost, product.ImageFile);
                        if (imagefile != null)
                        {
                        
                            product.Productpicture = imagefile;
                            _context.Products.Add(product);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        return View();
                }
            }
            List<SelectListItem> selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text ="Exist",Value="true"},
                new SelectListItem { Text = "Not Exist",Value ="false"},
            };
            ViewBag.IsAvailable = selectList;
            ViewBag.Categories = _context.Categories.Select(temp => new SelectListItem {Text=temp.Categoryname,Value=temp.Categoryid.ToString()});
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            List<SelectListItem> selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text ="Exist",Value="true"},
                new SelectListItem { Text = "Not Exist",Value ="false"}, 
            };
            ViewBag.IsAvailable = selectList;
            ViewBag.Categories = _context.Categories.Select(temp => new SelectListItem { Text = temp.Categoryname, Value = temp.Categoryid.ToString() });
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id,Product product)
        {
            if (id != product.Productid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (product.ImageFile != null)
                    {
                        string? imagefile = await ImageUploader.UploadeImage(_webhost, product.ImageFile);
                        if (imagefile != null)
                        {

                            product.Productpicture = imagefile;
                            _context.Update(product);
                            await _context.SaveChangesAsync();
                        }
                    }

                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Productid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            List<SelectListItem> selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text ="Exist",Value="true"},
                new SelectListItem { Text = "Not Exist",Value ="false"},
            };
            ViewBag.IsAvailable = selectList;
            ViewBag.Categories = _context.Categories.Select(temp => new SelectListItem { Text = temp.Categoryname, Value = temp.Categoryid.ToString() });
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Productid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ModelContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(decimal id)
        {
          return (_context.Products?.Any(e => e.Productid == id)).GetValueOrDefault();
        }
    }
}
