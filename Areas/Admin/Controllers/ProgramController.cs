using FBE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBE.ViewModels.Program;
using Microsoft.AspNetCore.Authorization;

namespace FBE.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class ProgramController : Controller
    {
        FBEContext _Db;

        public ProgramController(FBEContext db)
        {
            _Db = db;
        }

        public IActionResult Index()
        {
            var model = _Db.Programs.Include(x=>x.EABD).Where(x=>x.isActive==true).ToList();
            ViewBag.EABD = _Db.EABD.ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.EABD = _Db.EABD.ToList();
            ViewBag.Duzeys = _Db.Program_Duzey.ToList();
            ViewBag.Kadro = _Db.Akademik_Kadro.ToList();
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(ProgramCreate programCreate)
        {

            Programs newProgram = new Programs();
            newProgram.Prog_Ad = programCreate.Prog_Ad;
            newProgram.Prog_Ad_En = programCreate.Prog_Ad_En;
            newProgram.Prog_Ad_Ar = programCreate.Prog_Ad_Ar;
            newProgram.Program_Duzey = _Db.Program_Duzey.Find(programCreate.ProgramDuzey);
            newProgram.EbsId = programCreate.EBSId;
            newProgram.EABD = _Db.EABD.Find(programCreate.ProgramEABD);
            _Db.Programs.Add(newProgram);
            for (int i = 0; i < programCreate.ProgramKadro.Count; i++)
            {
                Programs_Akademik_Kadro pak = new Programs_Akademik_Kadro()
                {
                    Program = newProgram,
                    Akademik_Kadro = _Db.Akademik_Kadro.Find(programCreate.ProgramKadro[i]),
                    isActive = true
                };
                _Db.ProgramAkademik_Kadro.Add(pak);
            }
            newProgram.isActive = true;
            _Db.SaveChanges();

            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var query2 = _Db.Programs.Include(x => x.Program_Duzey).Include(x=>x.EABD).Include(x=>x.Programs_Akademik_Kadro).ThenInclude(x=>x.Akademik_Kadro).Where(x => x.Prog_Id == id).Select(x=> new ProgramEdit() {
                        Prog_Id = x.Prog_Id,
                        Prog_Ad = x.Prog_Ad,
                        Prog_Ad_En = x.Prog_Ad_En,
                        Prog_Ad_Ar = x.Prog_Ad_Ar,
                        ProgramDuzey = x.Program_Duzey.Prog_Duzey,
                        ProgramDuzeyID = x.Program_Duzey.Prog_Duzey_Id,
                        EABD_ID = x.EABD.EABD_Id,
                        EABD = x.EABD.EABD_Ad_Tr,
                        Akademik_Kadro = x.Programs_Akademik_Kadro.Where(x=>x.isActive==true).Select(y=>y.Akademik_Kadro.Sicil_No).ToList()
            });
            var model = query2.FirstOrDefault();
            ViewBag.Duzeyler = _Db.Program_Duzey;
            ViewBag.EABDs = _Db.EABD;
            ViewBag.Kadro = _Db.Akademik_Kadro.Where(x=>model.Akademik_Kadro.Contains(x.Sicil_No)).ToList();
            ViewBag.TumKadro = _Db.Akademik_Kadro.Where(x=>x.EABDId == _Db.EABD.Find(model.EABD_ID));
            return View(model);
        }


        [HttpPost]
        public IActionResult Edit(ProgramEdit dto)
        {
            var program = _Db.Programs.Include(x=>x.Programs_Akademik_Kadro).ThenInclude(x=>x.Akademik_Kadro)
                .Where(x=>x.Prog_Id == dto.Prog_Id).FirstOrDefault();
            program.Prog_Ad = dto.Prog_Ad;
            program.Prog_Ad_En = dto.Prog_Ad_En;
            program.Prog_Ad_Ar = dto.Prog_Ad_Ar;
            program.Program_Duzey = _Db.Program_Duzey.Find(dto.ProgramDuzeyID);
            program.EABD = _Db.EABD.Find(dto.EABD_ID);
            if (dto.Akademik_Kadro != null)
            {
                foreach (var item in program.Programs_Akademik_Kadro.Where(x=>x.isActive == true).ToList())
                {
                    if (!dto.Akademik_Kadro.Contains(item.Akademik_Kadro.Sicil_No))
                    {
                        //_Db.ProgramAkademik_Kadro.Remove(item);
                        item.isActive = false;
                    }
                }
            }
            else
            {
                foreach (var item in program.Programs_Akademik_Kadro.Where(x => x.isActive == true).ToList())
                {
                    item.isActive = false;
                }
            }
            _Db.SaveChanges();
            if (dto.Akademik_Kadro!=null)
            {
                foreach (var item in dto.Akademik_Kadro)
                {
                    foreach (var paks in program.Programs_Akademik_Kadro.Where(x=>x.isActive==false).ToList())
                    {
                        if (paks.Akademik_Kadro.Sicil_No == item)
                        {
                            paks.isActive = true;
                        }
                    }
                    if (!program.Programs_Akademik_Kadro.Where(x=>x.isActive == true).Select(x => x.Akademik_Kadro.Sicil_No).Contains(item))
                    {
                        Programs_Akademik_Kadro pak = new Programs_Akademik_Kadro();
                        pak.Program = program;
                        pak.Akademik_Kadro = _Db.Akademik_Kadro.Find(item);
                        pak.isActive = true;
                        _Db.ProgramAkademik_Kadro.Add(pak);
                    }
                }
            }
            _Db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var program = _Db.Programs.Include(x => x.Programs_Akademik_Kadro).Where(x => x.Prog_Id == id).FirstOrDefault();
            program.isActive = false;
            _Db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult KadroWithEabd(int eabd)
        {
            var kadro = _Db.Akademik_Kadro.Where(x => x.EABDId == _Db.EABD.Find(eabd)).ToList();
            return Json(new { data = kadro });
        }
        
        public JsonResult ProgramKadro(int program)
        {
            List<int> kadro = _Db.Programs.Include(x => x.Programs_Akademik_Kadro).ThenInclude(x => x.Akademik_Kadro)
                .Where(x => x.Prog_Id == program && x.Programs_Akademik_Kadro.Where(y=>y.Program == _Db.Programs.Find(program)).All(x=>x.isActive==true))
                .Select(x => x.Programs_Akademik_Kadro.Select(y => y.Akademik_Kadro.Sicil_No).ToList()).FirstOrDefault();
            List<Akademik_Kadro> kadro1 = null;
            if (kadro!=null)
            {
                kadro1 = _Db.Akademik_Kadro.Where(x => kadro.Contains(x.Sicil_No)).ToList();
            }
            return Json(new { data = kadro1 });
        }

        public IActionResult Detay(int id)
        {
            Program_Detay detay = _Db.Program_Detay.Include(x => x.Program).Where(x => x.Program.Prog_Id == id).FirstOrDefault();
            ProgramDetay dto = new ProgramDetay();
            var prog = _Db.Programs.Where(x => x.Prog_Id == id).FirstOrDefault();
            dto.Program = prog.Prog_Ad;
            dto.Programid = prog.Prog_Id;
            if (detay != null)
            {
                dto.Detay = detay.Program_Detay_;
                dto.DetayEn = detay.Program_Detay_En;
            }
            return View(dto);
        }
        [HttpPost]
        public IActionResult Detay(int id,ProgramDetay dto)
        {
            Program_Detay detay = _Db.Program_Detay.Include(x => x.Program).Where(x => x.Program.Prog_Id == id).FirstOrDefault();
            if (detay!=null)
            {
                if (dto.Detay == null)
                {
                    detay.Program_Detay_ = "";
                }
                else
                {
                    detay.Program_Detay_ = dto.Detay;
                }
            }
            else
            {
                Program_Detay newDetay = new Program_Detay
                {
                    Program = _Db.Programs.Find(id),
                    Program_Detay_ = dto.Detay
                };
                _Db.Program_Detay.Add(newDetay);
            }
            _Db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult getAllPrograms()
        {
            var programs = _Db.Programs.Include(x => x.EABD)
                .Where(x => x.isActive == true)
                .Select(x=>new
                {
                    progId = x.Prog_Id,
                    programName = x.Prog_Ad,
                    programEABDId = x.EABD.EABD_Id,
                    programEABD = x.EABD.EABD_Ad_Tr,
                })
                .ToList();
            return Json(new { data = programs });
        }

    }
}
