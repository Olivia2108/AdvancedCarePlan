using NLog.Config;
using NLog.LayoutRenderers;
using NLog;
using System.Text;

namespace CarePlanWeb
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
                if (_constantName != null)
                {

                    //string[] dateString = _constantName.Split(',');
                    //string[] date = dateString[0].Split('-');
                    //string[] time = dateString[1].Split(' ');
                    //DateTime enter_date = Convert.ToDateTime(date[1] + "/" + date[0] + "/" + date[2] + " " + time[1] + " " + time[2]);

                    //var previousHour = enter_date.Hour;
                    //var previousDay = enter_date.ToString("dd-MM-yyyy");
                    //var nowDay = DateTime.Now.ToString("dd-MM-yyyy");
                    //if (now == 0)
                    //{
                    //    _constantName = DateTime.Now.ToString("dd-MM-yyyy") + ", " + DateTime.Now.ToString("hh tt");
                    //}
                    //if (previousHour < now && previousDay == nowDay)
                    //{
                    //    _constantName = DateTime.Now.ToString("dd-MM-yyyy") + ", " + DateTime.Now.ToString("hh tt");
                    //}
                }

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
