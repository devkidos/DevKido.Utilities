//for any query or any solution in IT, please contact to anrorathod@gmail.com - +919725437729
using System.Web.Mvc;

namespace DevKido.Utilities.AlertMessage
{
    /// <summary>
    /// Adds the alerts to an existing ActionResult
    /// </summary>
    public class AlertDecoratorResult : ActionResult
    {
        public ActionResult InnerResult { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }

        public AlertDecoratorResult(ActionResult innerResult, string command, string message)
        {
            InnerResult = innerResult;
            Command = command;
            Message = message;
        }

        /// <summary>
        /// Uses the extension method to get the list of alerts from temp data
        /// add a new alert to this list and then hand the execution off to
        /// the innerResult
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            var alerts = context.Controller.TempData.GetAlerts();
            alerts.Add(new Alert(Command, Message));
            InnerResult.ExecuteResult(context);
        }
    }
}