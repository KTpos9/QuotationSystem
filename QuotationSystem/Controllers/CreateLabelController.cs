using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Data.Repositories.Interfaces;
using QuotationSystem.Data.Sessions;
using QuotationSystem.Models.CreateLabel;
using QuotationSystem.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using Zero.Security;

namespace QuotationSystem.Controllers
{
    [AllowAnonymous]
    public class CreateLabelController : Controller
    { 

        private static GeneratedLabelModel generatedLabelModel;
        private readonly IRunningNoRepository runningNoRepository;
        private readonly ISessionContext sessionContext;
        private readonly IItemRepository itemRepository;


        public CreateLabelController(IRunningNoRepository runningNoRepository, ISessionContext sessionContext, IItemRepository itemRepository)
        {
            this.runningNoRepository = runningNoRepository;
            this.sessionContext = sessionContext;
            this.itemRepository = itemRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GenerateLabel(CreateLabelModel model)
        {
            generatedLabelModel = new GeneratedLabelModel
            {
                Model = model,
                Labels = generateLabelNumberList(model.LabelQty,
                                                          runningNoRepository.GetLastRunningDate(),
                                                          getCurrentDate(),
                                                          runningNoRepository.GetLastRunningNo()),
                CurrentUser = sessionContext.CurrentUser.Id
            };
            
            return RedirectToAction("GeneratedLabel", "CreateLabel");
        }

        public IActionResult GeneratedLabel()
        {
            return View(generatedLabelModel);
        }

        private string getCurrentDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        private List<string> generateLabelNumberList(int labelQty, string lastRunningDate, string currentDate, string lastRunningNo) {
            if(lastRunningNo == null)
            {
                runningNoRepository.insert(new TRunningNo
                {
                    TypeNo = "label",
                    TypeName = null,
                    RunningDate = currentDate,
                    RunningNo = labelQty.ToString("00000"),
                });
                return generateListIncrement(new List<int> { 1 }, 1, labelQty).Select(i => currentDate + i).ToList();
            }
            if (lastRunningDate == currentDate)
            {
                runningNoRepository.Update(currentDate, labelQty);
                return generateListIncrement(new List<int> { Int32.Parse(lastRunningNo) + 1 }, 1, labelQty - 1).Select(i => currentDate + i).ToList();
            }
            else
            {
                runningNoRepository.insert(new TRunningNo
                {
                    TypeNo = "label",
                    TypeName = null,
                    RunningDate = currentDate,
                    RunningNo = lastRunningNo,
                });
                return generateListIncrement(new List<int> { 1 }, 1, labelQty - 1).Select(i => currentDate + i).ToList();
            }
        }

        private List<string> generateListIncrement(List<int> accumulator, int incrementBy, int repeatCount)
        {
            if(repeatCount == 0)
            {
                return accumulator.Select(x => x.ToString("00000")).ToList();
            }
            accumulator.Add(accumulator.Last() + incrementBy);
            return generateListIncrement(accumulator, incrementBy, repeatCount - 1);
        }

        public JsonResult GetItemDetailById(string itemCode) => Json(itemRepository.GetItemById(itemCode));

    }
}
