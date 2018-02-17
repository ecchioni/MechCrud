using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MechCrud.Models;
using System.Xml.Serialization;
using System.IO;

namespace MechCrud.Controllers
{
    [HandleError]
    public class MechController : Controller
    {               
        public static MechList result; 
        [XmlRoot("MechList")]
        public class MechList
        {
            [XmlElement("Mech")]
            public List<BattleMech> Mechs { get; set; }
        }     
       
        // GET: Mech
        public ActionResult Index(string sortOrder, string searchString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MechList));
            try
            {
                using (FileStream fs = new FileStream(Server.MapPath("~/Content/Data/MechList.xml"), FileMode.Open))
                {
                    result = (MechList)serializer.Deserialize(fs);
                }
            }
            catch (FileNotFoundException E)
            {

            }
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                result.Mechs = result.Mechs.Where(m => m.MechName.ToLower().Contains(searchString.ToLower()) 
                                                    || m.MechModel.ToLower().Contains(searchString.ToLower())).ToList();
            }

            #region Sorting
                        
            ViewBag.ModelSortParm = sortOrder == "mechmodel" ? "model_desc" : "mechmodel";
            ViewBag.NameSortParm = sortOrder == "mechname" ? "name_desc" : "mechname";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.TonnageSortParm = sortOrder == "tonnage" ? "tonnage_desc" : "tonnage";
           
            switch (sortOrder)
            {
                case "model_desc":                       
                    result.Mechs = result.Mechs.OrderByDescending(m => m.MechModel).ToList();
                    break;
                case "mechmodel":
                    result.Mechs = result.Mechs.OrderBy(m => m.MechModel).ToList();
                    break;                   
                case "name_desc":   
                    result.Mechs = result.Mechs.OrderByDescending(m => m.MechName).ToList();
                    break;
                case "mechname":                   
                    result.Mechs = result.Mechs.OrderBy(m => m.MechName).ToList();
                    break;
                case "price":
                    result.Mechs = result.Mechs.OrderBy(m => m.Price).ToList();
                    break;
                case "price_desc":
                    result.Mechs = result.Mechs.OrderByDescending(m => m.Price).ToList();
                    break;
                case "tonnage":
                    result.Mechs = result.Mechs.OrderBy(m => m.Tonnage).ToList();
                    break;
                case "tonnage_desc":
                    result.Mechs = result.Mechs.OrderByDescending(m => m.Tonnage).ToList();
                    break;
            }
            #endregion
            ViewBag.Mechs = result.Mechs.Count;
            return View(result.Mechs);
        }
        #region Add
        public ActionResult Add()
        {
            BattleMech mech = new BattleMech();
            return View(mech);
        }
        [HttpPost]
        public ActionResult Add(BattleMech mech)
        {
            if (ModelState.IsValid)
            {
                mech.Id = result.Mechs.Count + 1;
                result.Mechs.Add(mech);
                this.SerializeList();
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Edit
        public ActionResult Edit(int id)
        {
            var mech = this.GetMech(id);
            return View(mech);
        }
        [HttpPost]
        public ActionResult Edit(BattleMech mech )
        {           
                BattleMech oldData = this.GetMech(mech.Id);
                           oldData.MechModel = mech.MechModel;
                           oldData.MechName = mech.MechName;
                           oldData.Price = mech.Price;
                           oldData.LA = mech.LA;
                           oldData.LT = mech.LT;
                           oldData.Heatsinks = mech.Heatsinks;
                           oldData.Head = mech.Head;
                           oldData.RA = mech.RA;
                           oldData.RT = mech.RT;
                           oldData.CT = mech.CT;
                this.SerializeList();
           
            return RedirectToAction("Index");
        }
        #endregion
        #region Details
        public ActionResult Details(int Id)
        {
            BattleMech mech = GetMech(Id);
            return View(mech);
        }
        #endregion
        #region Delete
        public ActionResult Delete(int Id)
        {
            BattleMech mech = GetMech(Id);
            result.Mechs.Remove(mech);
            this.SerializeList();
            return RedirectToAction("Index");
        }
        #endregion
        #region Helpers
        [NonAction]
        public BattleMech GetMech(int Id)
        {
            return result.Mechs.Where(mech => mech.Id == Id).FirstOrDefault();
        }
        [NonAction]
        public void SerializeList()
        {
            try
            {
                using (FileStream fs = new FileStream(Server.MapPath("~/Content/Data/MechList.xml"), FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(MechList));
                    serializer.Serialize(fs, result);
                }
            }
            catch(FileNotFoundException FnF)
            {
                
            }
        }
        #endregion
    }
}