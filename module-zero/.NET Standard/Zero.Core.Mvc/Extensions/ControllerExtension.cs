using Microsoft.AspNetCore.Mvc;

namespace Zero.Core.Mvc.Extensions
{
    public static class ControllerExtension
    {
        public static void SetTempSuccessStatus(this Controller controller)
        {
            SetTempSuccessStatus(controller, "IsSuccess");
        }

        public static void SetTempSuccessStatus(this Controller controller, string key)
        {
            controller.TempData[key] = true;
        }

        public static void GetTempSuccessStatus(this Controller controller)
        {
            GetTempSuccessStatus(controller, "IsSuccess");
        }

        public static void GetTempSuccessStatus(this Controller controller, string key)
        {
            if ((controller.TempData[key] as bool?) == true)
            {
                controller.ViewData[key] = true;
            }
        }
    }
}