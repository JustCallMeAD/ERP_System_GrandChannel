﻿using ClothResorting.Helpers;
using ClothResorting.Models;
using ClothResorting.Models.FBAModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ClothResorting.Helpers.FBAHelper;
using ClothResorting.Models.FBAModels.StaticModels;
using ClothResorting.Models.StaticClass;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Globalization;
using ClothResorting.Helpers.DPHelper;
using AutoMapper;
using Newtonsoft.Json;

namespace ClothResorting.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.CanOperateAsT3) || User.IsInRole(RoleName.CanOperateAsT4) || User.IsInRole(RoleName.CanOperateAsT5) || User.IsInRole(RoleName.CanDeleteEverything))
                return View();
            else if (User.IsInRole(RoleName.CanOperateAsT2))
                return View("~/Views/Warehouse/Index.cshtml");
            else if (User.IsInRole(RoleName.CanViewAsClientOnly))
                return View("FBAClientIndex");
            else
                return RedirectToAction("Denied", "Home");
        }

        public ActionResult Test()
        {
            //var cleaner = new BillCleaner(@"D:\ToRemoteServer\Bill(1).xlsx");

            //var path = cleaner.ClearBills();

            //var pickDetailsInDb = _context.FBAPickDetails
            //    .Include(x => x.FBACartonLocation.FBAOrderDetail.FBAMasterOrder)
            //    .Include(x => x.FBAPalletLocation.FBAMasterOrder)
            //    .Where(x => x.Id > 0);

            //foreach(var p in pickDetailsInDb)
            //{
            //    if (p.FBAPalletLocation != null)
            //    {
            //        p.InboundDate = p.FBAPalletLocation.FBAMasterOrder.InboundDate;
            //    }
            //    else if (p.FBACartonLocation != null)
            //    {
            //        p.InboundDate = p.FBACartonLocation.FBAOrderDetail.FBAMasterOrder.InboundDate;
            //    }
            //}

            //var pickDetailCartonsInDb = _context.FBAPickDetailCartons
            //    .Include(x => x.FBACartonLocation.FBAOrderDetail.FBAMasterOrder)
            //    .Include(x => x.FBAPickDetail)
            //    .Where(x => x.Id > 0);

            //foreach(var p in pickDetailCartonsInDb)
            //{
            //    p.FBAPickDetail.InboundDate = p.FBACartonLocation.FBAOrderDetail.FBAMasterOrder.InboundDate;
            //}

            //_context.SaveChanges();

            var container = _context.Containers.First();
            var oldValueStr = JsonConvert.SerializeObject(container);

            container.ReceiptNumber = "99999";

            var logger = new Logger(_context);
            //await logger.AddCreatedLog<Container>(oldValueStr, container, "NA", "Ex");

            var name = _context.GetTableName<Container>();

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Denied()
        {
            return View();
        }

        private string UnifyTime(string dateString)
        {
            var dateArray = dateString.Split('/');
            var MM = "0";
            var dd = "0";
            var yyyy = "20";

            if (dateArray[0].Length == 1)
            {
                MM = MM + dateArray[0].ToString();
            }
            else
            {
                MM = dateArray[0].ToString();
            }

            if (dateArray[1].Length == 1)
            {
                dd = dd + dateArray[1].ToString();
            }
            else
            {
                dd = dateArray[1].ToString();
            }

            if (dateArray[2].Length == 2)
            {
                yyyy = yyyy + dateArray[2].ToString();
            }
            else if (dateArray[2].Length != 4)
            {
                yyyy = yyyy + dateArray[2][0].ToString() + dateArray[2][1].ToString();
            }
            else
            {
                yyyy = dateArray[2].ToString();
            }

            return yyyy + "-" + MM + "-" + dd;
        }
    }
}