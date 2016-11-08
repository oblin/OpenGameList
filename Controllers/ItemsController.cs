using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OpenGameList.ViewModels;
using Newtonsoft.Json;
using OpenGameList.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenGameList.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region RESTFul Conventions
        [HttpGet]
        public IActionResult Get()
        {
            return NotFound(new { Error = "not found" });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
                return new JsonResult(Mapper.Map<ItemViewModel>(item), DefaultJsonSettings);
            else
                return NotFound(new { Error = $"Item Id {item.Id} has not been found" });
        }

        [HttpPost, Authorize]
        public IActionResult Add([FromBody]ItemViewModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                var item = Mapper.Map<Item>(model);
                item.CreatedDate = item.LastModifiedDate = DateTime.Now;
                item.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.Items.Add(item);
                _context.SaveChanges();

                return new JsonResult(Mapper.Map<ItemViewModel>(item), DefaultJsonSettings);
            }

            return BadRequest("傳入的資料驗證失敗");
        }

        [HttpPut("{id}"), Authorize]
        public IActionResult Update(int id, [FromBody]ItemViewModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                var item = _context.Items.FirstOrDefault(i => i.Id == id);
                if (item != null)
                {
                    Mapper.Map(model, item, 
                    // 這裡的問題在於 model.ViewCount = 0，因為前端未處理所造成的，但 AutoMapper 還是會直接對應
                    // 但是這裡可以作為一個示範用法，在 BeforeMap 中，事先定應 model.ViewCount  
                    // TODO: 還是建議直接設定 ignore map，因為 view count 跟前端無關
                        opt => opt.BeforeMap((s, d) => {
                            s.ViewCount = s.ViewCount > 0 ? s.ViewCount : d.ViewCount; 
                        }));
                    item.LastModifiedDate = DateTime.Now;
                    _context.SaveChanges();

                    return new JsonResult(Mapper.Map<ItemViewModel>(item), DefaultJsonSettings);
                }
            }

            return NotFound($"Item Id {id} has not been found or validation failed");
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound($"Item Id {id} has not been found or validation failed");
        }

        #endregion
        #region Attribute-based Routing
        [HttpGet("GetLastest")]
        public JsonResult GetLastest()
        {
            return GetLastest(DefaultNumberOfItems);
        }

        // GET: api/items/GetLastest/5
        [HttpGet("GetLastest/{n}")]
        public JsonResult GetLastest(int n)
        {
            if (n > MaxNumberOfItems) n = MaxNumberOfItems;
            var items = _context.Items.OrderByDescending(i => i.CreatedDate).Take(n);
            return new JsonResult(Mapper.Map<IEnumerable<ItemViewModel>>(items), DefaultJsonSettings);
        }

        [HttpGet("GetMostViewed")]
        public IActionResult GetMostViewed()
        {
            return GetMostViewed(DefaultNumberOfItems);
        }

        [HttpGet("GetMostViewed/{n}")]
        public IActionResult GetMostViewed(int n)
        {
            if (n > MaxNumberOfItems) n = MaxNumberOfItems;
            var items = _context.Items.OrderByDescending(i => i.ViewCount).Take(n);
            return new JsonResult(Mapper.Map<IEnumerable<ItemViewModel>>(items), DefaultJsonSettings);
        }

        [HttpGet("GetRandom")]
        public IActionResult GetRandom()
        {
            return GetRandom(DefaultNumberOfItems);
        }

        [HttpGet("GetRandom/{n}")]
        public IActionResult GetRandom(int n)
        {
            if (n > MaxNumberOfItems) n = MaxNumberOfItems;
            var items = _context.Items.OrderBy(i => Guid.NewGuid()).Take(n);
            return new JsonResult(Mapper.Map<IEnumerable<ItemViewModel>>(items), DefaultJsonSettings);
        }
        #endregion

        private List<ItemViewModel> GetSampleItems(int num = 999)
        {
            List<ItemViewModel> lst = new List<ItemViewModel>();
            DateTime date = new DateTime(2015, 12, 31).AddDays(-num);
            for (int id = 1; id <= num; id++)
            {
                lst.Add(new ItemViewModel()
                {
                    Id = id,
                    Title = String.Format("Item {0} Title", id),
                    Description = String.Format("This is a sample description for item {0}: Lorem ipsum dolor sit amet.", id),
                    CreatedDate = date.AddDays(id),
                    LastModifiedDate = date.AddDays(id),
                    ViewCount = num - id
                });
            }
            return lst;
        }

        private JsonSerializerSettings DefaultJsonSettings
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                };
            }
        }

        private int DefaultNumberOfItems => 5;
        private int MaxNumberOfItems => 100;
    }
}
