using FBE.Models;
using FBE.ViewModels.Kadro;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBE.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class KadroController : Controller
    {
        FBEContext _db;

        public KadroController(FBEContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var model = _db.Akademik_Kadro.Include(x=>x.Unvan).Include(x=>x.EABDId).Include(x => x.Programs_Akademik_Kadro).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult UnvanCreate(string unvan)
        {
            Unvan unvan1 = new Unvan() { Unvan_Name = unvan };
            _db.Unvan.Add(unvan1);
            _db.SaveChanges();
            return Json(new { data =  unvan1});
        }

        public IActionResult Create()
        {
            ViewBag.EABD = _db.EABD.ToList();
            ViewBag.Unvan = _db.Unvan.ToList();
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(KadroCreate kadroCreate)
        {
            if (kadroCreate.EABDBaskan)
            {
                var kdr = _db.Akademik_Kadro.Include(x => x.EABDId).ToList();
                foreach (var item in kdr)
                {
                    if (_db.EABD.Find(kadroCreate.EABDId) == item.EABDId)
                    {
                        if (item.EABDBaskan == true)
                        {
                            kadroCreate.EABDBaskan = false;
                        }
                    }
                }
            }
            Akademik_Kadro kadro = new Akademik_Kadro()
            {
                Ad = kadroCreate.Ad,
                Soyad=kadroCreate.Soyad,
                Kullanici_Adi = kadroCreate.Kullanici_Adi,
                EABDId = _db.EABD.Find(kadroCreate.EABDId),
                EABDBaskan = kadroCreate.EABDBaskan,
                Unvan = _db.Unvan.Find(kadroCreate.Unvan)
            };
            _db.Akademik_Kadro.Add(kadro);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Akademik_Kadro kadro = _db.Akademik_Kadro.Include(x=>x.EABDId).Include(x=>x.Unvan).Where(x=>x.Sicil_No == id).FirstOrDefault();
            KadroEdit dto = new KadroEdit()
            {
                Ad = kadro.Ad,
                Soyad = kadro.Soyad,
                Kullanici_Adi = kadro.Kullanici_Adi,
                EABDId = kadro.EABDId.EABD_Id,
                EABDBaskan = kadro.EABDBaskan,
                Unvan = kadro.Unvan.Unvan_Id,
                Programs = _db.ProgramAkademik_Kadro.Where(x => x.Akademik_Kadro == kadro).Select(x => x.Program).ToList()
        };
            ViewBag.EABD = _db.EABD.ToList();
            ViewBag.Unvan = _db.Unvan.ToList();
            return View(dto);
        }

        [HttpPost]
        public IActionResult Edit(int id,KadroEdit dto)
        {
            if (dto.EABDBaskan)
            {
                var kdr = _db.Akademik_Kadro.Include(x => x.EABDId).ToList();
                foreach (var item in kdr)
                {
                    if (_db.EABD.Find(dto.EABDId) == item.EABDId)
                    {
                        if (item.EABDBaskan == true)
                        {
                            dto.EABDBaskan = false;
                        }
                    }
                }
            }
            Akademik_Kadro kadro = _db.Akademik_Kadro.Find(id);
            kadro.Ad = dto.Ad;
            kadro.Soyad = dto.Soyad;
            kadro.Kullanici_Adi = dto.Kullanici_Adi;
            kadro.EABDId = _db.EABD.Find(dto.EABDId);
            kadro.EABDBaskan = dto.EABDBaskan;
            kadro.Unvan = _db.Unvan.Find(dto.Unvan);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
         public IActionResult Delete(int id)
        {
            _db.Akademik_Kadro.Remove(_db.Akademik_Kadro.Find(id));
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Yetki(int id,int kadroid)
        {
            var program = _db.ProgramAkademik_Kadro
                .Where(x => x.Program == _db.Programs.Find(id) && x.Akademik_Kadro == _db.Akademik_Kadro.Find(kadroid) && x.isActive == true)
                .Select(x => new
                {
                    Yurutme = x.Yurutme_Kurulu,
                    Yeterlilik = x.Yeterlilik_Kurulu,
                    Koordinator = x.Koordinator
                })
                .FirstOrDefault();
            return Json(new {data = program });
        }

        [HttpPost]
        public IActionResult Yetki(int id,int kadroid,bool yurutme,bool yeterlilik,bool koordinator)
        {
            var program = _db.Programs.Where(x => x.Prog_Id == id).FirstOrDefault();
            var progkadro = _db.ProgramAkademik_Kadro.Where(x => x.Akademik_Kadro == _db.Akademik_Kadro.Find(kadroid) && x.Program == program && x.isActive == true).FirstOrDefault();
            if (progkadro!=null)
            {
                progkadro.Yurutme_Kurulu = yurutme;
                progkadro.Yeterlilik_Kurulu = yeterlilik;
                progkadro.Koordinator = koordinator;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public JsonResult getAllKadros()
        {
            var kadros = _db.Akademik_Kadro.Include(x => x.EABDId)
                
                .Select(x => new
                {
                    sicilNo = x.Sicil_No,
                    kadroName = x.Ad + " "+ x.Soyad,
                    kullanici_adi=x.Kullanici_Adi,
                    EABDId=x.EABDId.EABD_Ad_Tr,
                    EABDbaskan=x.EABDBaskan,
                    unvanId=x.Unvan.Unvan_Name

                })
                .ToList();
            return Json(new { data = kadros });
        }
    }
}
