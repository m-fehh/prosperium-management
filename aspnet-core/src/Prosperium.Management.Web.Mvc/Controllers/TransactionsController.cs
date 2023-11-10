﻿using Microsoft.AspNetCore.Mvc;
using Prosperium.Management.Controllers;
using Prosperium.Management.OpenAPI.V1.Categories;
using Prosperium.Management.OpenAPI.V1.Subcategories;
using Prosperium.Management.OpenAPI.V1.Subcategories.Dto;
using Prosperium.Management.OpenAPI.V1.Transactions;
using Prosperium.Management.Web.Models.Transactions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prosperium.Management.Web.Controllers
{
    [Route("App/Transactions")]
    public class TransactionsController : ManagementControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;
        private readonly ISubcategoryAppService _subcategoryAppService;

        public TransactionsController(ICategoryAppService categoryAppService, ISubcategoryAppService subcategoryAppService)
        {
            _categoryAppService = categoryAppService;
            _subcategoryAppService = subcategoryAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("SelectCategory")]
        public async Task<ActionResult> SelectCategory(string tipoTransacao)
        {
            TransactionConsts.TransactionType transactionType = TransactionConsts.TransactionType.Gastos; // Inicializando com um valor padrão

            switch (tipoTransacao)
            {
                case "gasto":
                    transactionType = TransactionConsts.TransactionType.Gastos;
                    break;
                case "ganho":
                    transactionType = TransactionConsts.TransactionType.Ganhos;
                    break;
                default:
                    transactionType = TransactionConsts.TransactionType.Gastos;
                    break;
            }

            var allCategories = await _categoryAppService.GetAllAsync(transactionType);

            var model = new SelectCategoryModalViewModel
            {
                Categories = allCategories,
            };

            return PartialView("_SelectCategoryModal", model);
        }

        [HttpGet("SelectSubcategory")]
        public async Task<ActionResult<List<SubcategoryDto>>> SelectSubcategory(long categoryId)
        {
            return await _subcategoryAppService.GetByCategoryIdAsync(categoryId);
        }
    }

    //public async Task<ActionResult> SelectAccount()
    //{
    //    var allAccounts = await _transactionAppService.

    //    return PartialView("_SelectAccountModal");
    //}
}
