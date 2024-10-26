﻿using ClassLibrary.Common;
using ClassLibrary.Mvc.Html;
using ClassLibrary.Mvc.Models.Home;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;

namespace ClassLibrary.Mvc.Controllers
{
    [Controller]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public abstract partial class WebBaseController<T>(
        ILogger<T> logger
        ) : Controller where T : WebBaseController<T>
    {
        protected readonly ILogger _logger = logger;

        protected string _invalidModelState = string.Empty;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            base.OnActionExecuting(context);
        }

        protected BadRequestObjectResult InvalidModelState()
        {
            string message = _invalidModelState + $"{AsciiCodes.CRLF}";
            List<KeyValuePair<string, ModelStateEntry?>> errors = ModelState
                .Where(x => x.Value?.ValidationState == ModelValidationState.Invalid)
                .ToList();

            foreach (KeyValuePair<string, ModelStateEntry?> keyValue in errors)
            {
                message += $"{AsciiCodes.CRLF}Key: {keyValue.Key}{AsciiCodes.CRLF}";
                if (keyValue.Value != null)
                    foreach (ModelError error in keyValue.Value.Errors)
                        message += $"{error.ErrorMessage} ";

                message += $"{AsciiCodes.CRLF}";
            }

            return new BadRequestObjectResult(message);
        }

        protected ObjectResult ExceptionResult(Exception exception)
        {
            string controller = RouteData.Values.GetValueOrDefault("Controller")?.ToString() ?? string.Empty;
            string action = RouteData.Values.GetValueOrDefault("Action")?.ToString() ?? string.Empty;
            string errorMessage = $"Exception thrown in Controller: {controller} Action: {action} {AsciiCodes.CRLF}Exception: {exception.Message}";
            if (exception.InnerException != null)
                errorMessage += $"{AsciiCodes.CRLF}Inner Exception: {exception.InnerException.Message}";

            _logger.LogError(exception, "{errorMessage}", errorMessage);
            return new ObjectResult(errorMessage) { StatusCode = 403 };
        }

        protected KeyValuePair<int, string> ValidateModel<M>(M model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            int key = 0;
            string value = string.Empty;

            bool isValid = TryValidateModel(model);
            if (!isValid)
            {
                foreach (var modelValue in ModelState.Values)
                {
                    var errors = modelValue.Errors;
                    if (errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            key++;
                            value += $"{Tag.Brackets(error.ErrorMessage)}{Tag.LineBreak()}";
                        }
                    }
                }
            }

            return new KeyValuePair<int, string>(key, value);
        }

        protected string ExceptionMessage(Exception exception)
        {
            string controller = RouteData.Values.GetValueOrDefault("Controller")?.ToString() ?? string.Empty;
            string action = RouteData.Values.GetValueOrDefault("Action")?.ToString() ?? string.Empty;
            string errorMessage = $"Exception thrown in Controller: {controller} Action: {action} {AsciiCodes.CRLF}Exception: {exception.Message}";
            if (exception.InnerException != null)
                errorMessage += $"{AsciiCodes.CRLF}Inner Exception: {exception.InnerException.Message}";

            _logger.LogError(exception, "{errorMessage}", errorMessage);

            return errorMessage;
        }

        [HttpGet, HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "Agree with ScottyMac52 - https://github.com/dotnet/roslyn-analyzers/issues/5626#issuecomment-1079411215")]
        public virtual IActionResult Error(int id)
        {
            if (id == 0)
                if (Request.Method.Equals("post", StringComparison.CurrentCultureIgnoreCase))
                    _ = int.TryParse((RouteData?.Values["id"]?.ToString()) ?? "0", out id);

            var vm = new ErrorViewModel(id);

            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            vm.RequestId = requestId;

            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                if (exceptionFeature.Error.GetType().IsAssignableFrom(typeof(HttpRequestException)))
                {
                    try
                    {
                        HttpStatusCode? httpStatusCode = ((HttpRequestException)exceptionFeature.Error).StatusCode;
                        if (httpStatusCode != null)
                        {
                            vm.StatusCode = (int)httpStatusCode;
                            vm.StatusMessage = exceptionFeature.Error.Message;
                            _logger.LogInformation($"{exceptionFeature.Error.Message} RequestId = {requestId}");
                        }
                        else
                        {
                            vm.Exception = exceptionFeature.Error;
                            _logger.LogError(exceptionFeature.Error, $"Exception RequestId = {requestId}");
                        }
                    }
                    catch (Exception exception)
                    {
                        vm.Exception = exceptionFeature.Error;
                        _logger.LogError(exceptionFeature.Error, $"Exception RequestId = {requestId}");
                        _logger.LogError(exception, $"Exception RequestId = {requestId}");
                    }
                }
                else
                {
                    vm.Exception = exceptionFeature.Error;
                    _logger.LogError(exceptionFeature.Error, $"Exception RequestId = {requestId}");
                }
            }

            if (Request.IsAjaxRequest())
                return StatusCode(vm.StatusCode, vm.StatusMessage);

            return View("Error", vm);

        }
    }
}
