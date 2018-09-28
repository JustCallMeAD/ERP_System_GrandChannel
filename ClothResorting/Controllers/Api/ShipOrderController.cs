﻿using AutoMapper;
using ClothResorting.Dtos;
using ClothResorting.Models;
using ClothResorting.Models.ApiTransformModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity;
using System.Web.Http;
using System.Web;
using ClothResorting.Helpers;
using ClothResorting.Models.StaticClass;

namespace ClothResorting.Controllers.Api
{
    public class ShipOrderController : ApiController
    {
        private ApplicationDbContext _context;
        private string _userName;
        private ShipOrderManager _manager;

        public ShipOrderController()
        {
            _context = new ApplicationDbContext();
            _userName = HttpContext.Current.User.Identity.Name.Split('@')[0];
            _manager = new ShipOrderManager();
        }

        // GET /api/ShipOrder/ 查询
        [HttpGet]
        public IHttpActionResult GetAllShipOrder()
        {
            var resultDto = _context.ShipOrders.OrderByDescending(x => x.Id)
                .Where(x => x.Id > 0).Select(Mapper.Map<ShipOrder, ShipOrderDto>);

            return Ok(resultDto);
        }

        // POST /api/ShipOrder/ 新建订单
        [HttpPost]
        public IHttpActionResult CreateNewShipOrder([FromBody]PickTiketsRangeJsonObj obj)
        {
            _context.ShipOrders.Add(new ShipOrder {
                OrderPurchaseOrder = obj.OrderPurchaseOrder,
                Customer = obj.Customer,
                Address = obj.Address,
                PickTicketsRange = obj.PickTicketsRange,
                CreateDate = DateTime.Now.ToString("MM/dd/yyyy"),
                PickDate = Status.Unassigned,
                PickingMan = Status.Unassigned,
                Status = Status.NewCreated,
                Operator = _userName,
                Vendor = obj.Vendor,
                ShippingMan = Status.Unassigned,
                OrderType = obj.OrderType
            });

            _context.SaveChanges();

            var result = _context.ShipOrders.OrderByDescending(x => x.Id).First();
            var resultDto = Mapper.Map<ShipOrder, ShipOrderDto>(result);

            return Created(Request.RequestUri + "/" + result.Id, resultDto);
        }

        // PUT /api/ShipOrder/?shipOrderId={ishipOrderId} 发货订单
        [HttpPut]
        public void ShipShipOrder([FromUri]int shipOrderId)
        {
            _manager.ConfirmAndShip(shipOrderId);
        }
        
        // PUT /api/shiporder/?onChangeShipOrderId={id} 改变订单状态
        [HttpPut]
        public void ChangeStatus([FromUri]int onChangeshipOrderId)
        {
            var shipOrderInDb = _context.ShipOrders.Find(onChangeshipOrderId);
            if (shipOrderInDb.Status == Status.Picking)
            {
                shipOrderInDb.Status = Status.Ready;
            }
            else if (shipOrderInDb.Status == Status.Ready)
            {
                shipOrderInDb.Status = Status.Picking;
            }
            _context.SaveChanges();
        }

        // DELETE /api/shipOrder/?shipOrderId={ishipOrderId} 取消订单
        [HttpDelete]
        public void CancelShipOrder([FromUri]int shipOrderId)
        {
            _manager.CancelShipOrder(shipOrderId);
        }
    }
}
