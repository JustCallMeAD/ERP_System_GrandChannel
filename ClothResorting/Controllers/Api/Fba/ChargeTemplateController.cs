﻿using AutoMapper;
using ClothResorting.Dtos.Fba;
using ClothResorting.Models;
using ClothResorting.Models.FBAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace ClothResorting.Controllers.Api.Fba
{
    public class ChargeTemplateController : ApiController
    {
        private FBADbContext _context;

        public ChargeTemplateController()
        {
            _context = new FBADbContext();
        }

        // GET /api/fba/chargetemplate/
        [HttpGet]
        public IHttpActionResult GetAllTemplates()
        {
            var templatesDto = _context.ChargeTemplates
                .Where(x => x.Id > 0)
                .Select(Mapper.Map<ChargeTemplate, ChargeTemplateDto>);

            return Ok(templatesDto);
        }

        // POST /api/fba/chargetemplate/?templateName={templateName}&customer={customer}&timeUnit={timeUnit}&currency={currency}
        [HttpPost]
        public IHttpActionResult CreateNewTemplate([FromUri]string templateName, [FromUri]string customer, [FromUri]string timeUnit, [FromUri]string currency)
        {
            var newTemplate = new ChargeTemplate {
                TemplateName = templateName,
                Customer = customer,
                TimeUnit = timeUnit,
                Currency = currency
            };

            _context.ChargeTemplates.Add(newTemplate);
            _context.SaveChanges();

            var sample = _context.ChargeTemplates
                .OrderByDescending(x => x.Id)
                .First();

            return Created(Request.RequestUri + "/" + sample.Id, Mapper.Map<ChargeTemplate, ChargeTemplateDto>(sample));
        }

        // DELETE /api/fba/chargetemplate/?templateId={templateId}
        [HttpDelete]
        public void DeleteTemplate([FromUri]int templateId)
        {
            var methodsInDb = _context.ChargeMethods
                .Include(x => x.ChargeTemplate)
                .Where(x => x.ChargeTemplate.Id == templateId);

            _context.ChargeMethods.RemoveRange(methodsInDb);
            _context.ChargeTemplates.Remove(_context.ChargeTemplates.Find(templateId));
            _context.SaveChanges();
        }
    }
}
