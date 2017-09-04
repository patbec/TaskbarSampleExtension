using System;
using System.Runtime.InteropServices;

namespace TaskbarSampleExt
{
    /// <summary>
    /// The display name of the extension and the description for the HelpText(displayed in status bar when menu command selected).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DeskBandInfoAttribute : System.Attribute
    {
        private string _displayName;
        private string _helpText;

        public string DisplayName
        {
            get { return _displayName; }
        }

        public string HelpText
        {
            get { return _helpText; }
        }

        public DeskBandInfoAttribute() { }

        public DeskBandInfoAttribute(string displayName, string helpText)
        {
            _displayName = displayName;
            _helpText = helpText;
        }
    }
}
