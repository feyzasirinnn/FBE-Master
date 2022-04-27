    using FBE.Controllers;
using FBE.Models;
using FBE.ViewModels; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FBE.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class AnnouncementController : BaseController
    {
        public AnnouncementController(FBEContext _db) : base(_db)
        {

        }

        public IActionResult Index(string Search = null)
        {
            var model = new AnnounceIndex();
            var query = db.Announce.AsQueryable();
            if (!string.IsNullOrEmpty(Search))
            {
                query = query.Where(t =>
                t.Title.Contains(Search));
            }
            model.Announcements = query.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new AnnouncementCreate(); 
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(AnnouncementCreate model)
        {
            if (ModelState.IsValid)
            {
                var announce = new Announce()
                {
                    Title = model.Title,
                    Description = model.Description,
                    TitleEng = model.TitleEng,
                    DescriptionEng = model.DescriptionEng,
                    Enable = model.Enable,
                    Deleted = model.Deleted,
                    Date = model.Date, 
                    IsImportant=model.IsImportant
                };
                db.Announce.Add(announce);
                db.SaveChanges();
                return RedirectToAction("Index");
            } 
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {

            var query = from p in db.Announce
                        where p.Id == id
                        select new AnnouncementEdit()
                        { 
                            Id = p.Id,
                            Title = p.Title,
                            Description = p.Description,
                            TitleEng = p.TitleEng,
                            DescriptionEng = p.DescriptionEng,
                            Enable = p.Enable,
                            Deleted = p.Deleted,
                            Date = p.Date, 
                            IsImportant=p.IsImportant
                            
                        };
            var model = query.FirstOrDefault(); 
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(AnnouncementEdit model)
        {
            if (ModelState.IsValid)
            {
                Announce announce = db.Announce.FirstOrDefault(t => t.Id == model.Id);
                announce.Title = model.Title;
                announce.Description = model.Description;
                announce.TitleEng = model.TitleEng;
                announce.DescriptionEng = model.DescriptionEng;
                announce.Date = model.Date;
                announce.Deleted = model.Deleted;
                announce.Enable = model.Enable; 
                announce.IsImportant = model.IsImportant;

                db.SaveChanges();
                return RedirectToAction("Index");
            } 
            return View(model);
        }
    }
}