using NLog.Config;
using NLog.LayoutRenderers;
using NLog;
using System.Text;

namespace Domain.Exceptions
{


    [LayoutRenderer("UniqueName")]
    public class NLogLayoutRenderer : LayoutRenderer
    {
        #region Fields (1)

        private string _constantName;

        #endregion Fields

        #region Enums (1)

        public enum PatternType
        {
            /// <summary>
            /// Long date + current process ID
            /// </summary>
            LongDateAndPID,
            /// <summary>
            /// Long date (including ms)
            /// </summary>
            LongDate,
        }

        #endregion Enums

        #region Properties (2)

        public string ConstantName
        {
            get
            {
                var now = DateTime.Now.Hour; 

                if (_constantName == null)
                {
                    _constantName = DateTime.Now.ToString("dd-MM-yyyy") + ", " + DateTime.Now.ToString("hh tt");

                }

                return _constantName;
            }
        }

        [DefaultParameter]
        public PatternType Format { get; set; }

        #endregion Properties

        #region Methods (2)

        // Protected Methods (1) 

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(ConstantName);
        }
        // Private Methods (1) 

        #endregion Methods
    }
}
