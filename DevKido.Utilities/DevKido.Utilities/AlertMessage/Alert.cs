//for any query or any solution in IT, please contact to anrorathod@gmail.com - +919725437729

namespace DevKido.Utilities.AlertMessage
{
    /// <summary>
    /// Display a message to the user via Toastr.
    /// Command is the toastr action, success, info, etc. and 
    /// Message is the text to display in the alert.
    /// </summary>

    public class Alert
    {
        public string Command { get; set; }
        public string Message { get; set; }

        public Alert(string command, string message)
        {
            Command = command;
            Message = message;
        }
    }
}