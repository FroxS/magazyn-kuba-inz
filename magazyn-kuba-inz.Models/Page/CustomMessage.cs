using Warehouse.Models.Enums;

namespace Warehouse.Models.Page
{
    public class CustomMessage
    {
        #region Public properties

        public EMessageType Type { get; set; }

        public string Text { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomMessage(EMessageType type, string text)
        {
            Type = type;
            Text = text;
        }

        #endregion
    }
}