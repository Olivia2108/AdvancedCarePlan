using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using WebUI.Helpers.Interface;
using WebUI.Models;
using WebUI.Models.Dto;
using WebUI.Models.ViewModels;
using static Domain.Enums.GeneralEnums;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientHelper _clientHelper;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _accessor;

        public HomeController(ILogger<HomeController> logger, 
            IClientHelper clientHelper, 
            IConfiguration config,
            IHttpContextAccessor accessor)
        {
            _logger = logger;
            _clientHelper = clientHelper;
            _config = config;
            _accessor = accessor;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new CarePlanWebDto();
            try
            {
                var baseUrl = _config.GetValue<string>("API:BaseUrl");
                var url = _config.GetValue<string>("API:GetAll");
                var response = await _clientHelper.GetAsync(baseUrl, url, "");

                switch (response.Success)
                {
                    case false: 
                        _accessor.HttpContext.Session.SetString("SuccessMsg", response.Message);
                        break;
                }

                var json = JsonConvert.SerializeObject(response.Data);
                var plans = JsonConvert.DeserializeObject<List<CarePlanWebVM>>(json);

                Enum.GetValues(typeof(Titles)).Cast<Titles>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

                vm.List = plans;
                return View(vm);
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);
                 
            }
            return View(vm);
        }
         

        [HttpPost]
        public async Task<IActionResult> Index(string patientNameAdd, string usernameAdd, string completedAdd, string reasonAdd, string actionAdd, string outcomeAdd, DateTime setEndDate, DateTime setStartDate, DateTime setTodaysDate, string titleAdd)
        {
            try
            {
                bool complete = false;
                switch (completedAdd)
                {
                    case "1":
                        complete = true;
                        break;

                    case "0":
                        complete = false;
                        break;

                }
                var data = new CarePlanWebDto
                {
                    PatientName = patientNameAdd,
                    UserName = usernameAdd,
                    Action = actionAdd,
                    ActualEndDate = setEndDate,
                    ActualStartDate = setStartDate,
                    TargetStartDate = setTodaysDate,
                    Completed = complete,
                    Outcome = outcomeAdd,
                    Title = titleAdd,
                    Reason = reasonAdd,
                    IpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString()
                };
                
                var baseUrl = _config.GetValue<string>("API:BaseUrl");
                var url = _config.GetValue<string>("API:Post");
                var response = await _clientHelper.PostAsync(baseUrl, url, data);

                switch (response.Success)
                {
                    case false:
                        _accessor.HttpContext.Session.SetString("ErrorMsg", response.Message.Replace("'", ""));
                        break;
                    case true:
                        _accessor.HttpContext.Session.SetString("SuccessMsg", response.Message.Replace("'", ""));
                        break;
                } 

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);

            }
            return RedirectToAction("Index", "Home");
        }
        

        [HttpPost]
        public async Task<IActionResult> Edit(long id, string patientNameEdit, string usernameEdit, string completedEdit, string reasonEdit, string actionEdit, string outcomeEdit, DateTime actualStartDateEdit, DateTime targetStartDateEdit, DateTime setEndDate, string titleEdit)
        {
            try
            {
                bool complete = false;
                switch (completedEdit)
                {
                    case "1":
                        complete = true;
                        break;

                    case "0":
                        complete = false;
                        break;

                }

                var data = new CarePlanWebDto
                {
                    Id = id,
                    PatientName = patientNameEdit,
                    UserName = usernameEdit,
                    Action = actionEdit,
                    ActualEndDate = setEndDate,
                    ActualStartDate = targetStartDateEdit,
                    TargetStartDate = actualStartDateEdit,
                    Completed = complete,
                    Outcome = outcomeEdit,
                    Title = titleEdit,
                    Reason = reasonEdit,
                    IpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString()
                };

                var baseUrl = _config.GetValue<string>("API:BaseUrl");
                var url = _config.GetValue<string>("API:UpdateById");
                var response = await _clientHelper.PutAsync(baseUrl, url, id.ToString(), data);

                switch (response.Success)
                {
                    case false:
                        _accessor.HttpContext.Session.SetString("ErrorMsg", response.Message.Replace("'", ""));
                        break;
                    case true:
                        _accessor.HttpContext.Session.SetString("SuccessMsg", response.Message.Replace("'", ""));
                        break;
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);

            }
            return RedirectToAction("Index", "Home");
        }
        

        [HttpPost]
        public async Task<IActionResult> Delete(long deleteId)
        {
            try
            {
                var IpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var baseUrl = _config.GetValue<string>("API:BaseUrl");
                var url = _config.GetValue<string>("API:DeleteById");
                var concat = $"?carePlanId={deleteId}&&ipaddress={IpAddress}";
                var response = await _clientHelper.DeleteAsync(baseUrl, url, concat);

                switch (response.Success)
                {
                    case false:
                        _accessor.HttpContext.Session.SetString("ErrorMsg", response.Message.Replace("'", ""));
                        break;
                    case true:
                        _accessor.HttpContext.Session.SetString("SuccessMsg", response.Message.Replace("'", ""));
                        break;
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError(ex.Message);

            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}