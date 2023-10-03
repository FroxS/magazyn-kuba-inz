using magazyn_kuba_inz.Models.Enums;

namespace magazyn_kuba_inz.Models.Page
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