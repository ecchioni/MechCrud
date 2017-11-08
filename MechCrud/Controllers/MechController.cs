using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            catch (Exception E)
            {

            }
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                result.Mechs = result.Mechs.Where(m => m.MechName.ToLower().Contains(searchString.ToLower()) 
                                                    || m.MechModel.ToLower().Contains(searchString.ToLower())).ToList();
            }

            #region Sorting
                        
            ViewBag.ModelSortParm = sortOrder == "MechModel" ? "model_desc" : "MechModel";
            ViewBag.NameSortParm = sortOrder == "MechName" ? "name_desc" : "MechName";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.TonnageSortParm = sortOrder == "Tonnage" ? "tonnage_desc" : "Tonnage";
           
            switch (sortOrder)
            {
                case "model_desc":                       
                    result.Mechs = result.Mechs.OrderByDescending(m => m.MechModel).ToList();
                    break;
                case "MechModel":
                    result.Mechs = result.Mechs.OrderBy(m => m.MechModel).ToList();
                    break;                   
                case "name_desc":   
                    result.Mechs = result.Mechs.OrderByDescending(m => m.MechName).ToList();
                    break;
                case "MechName":                   
                    result.Mechs = result.Mechs.OrderBy(m => m.MechName).ToList();
                    break;
                case "Price":
                    result.Mechs = result.Mechs.OrderBy(m => m.Price).ToList();
                    break;
                case "price_desc":
                    result.Mechs = result.Mechs.OrderByDescending(m => m.Price).ToList();
                    break;
                case "Tonnage":
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
        public ActionResult Add()
        {
            BattleMech mech = new BattleMech();
            return View(mech);
        }
        [HttpPost]
        public ActionResult Add(BattleMech mech)
        {
            mech.Id = result.Mechs.Count + 1;
            result.Mechs.Add(mech);
            this.SerializeList();
            return RedirectToAction("Index");
        }
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
        public ActionResult Details(int Id)
        {
            BattleMech mech = GetMech(Id);
            return View(mech);
        }
        public ActionResult Delete(int Id)
        {
            BattleMech mech = GetMech(Id);
            result.Mechs.Remove(mech);
            this.SerializeList();
            return RedirectToAction("Index");
        }
        [NonAction]
        public BattleMech GetMech(int Id)
        {
            return result.Mechs.Where(mech => mech.Id == Id).FirstOrDefault();
        }
        [NonAction]
        public void SerializeList()
        {
            using (FileStream fs = new FileStream(Server.MapPath("~/Content/Data/MechList.xml"), FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MechList));
                serializer.Serialize(fs, result);
            }
        }
    }
}