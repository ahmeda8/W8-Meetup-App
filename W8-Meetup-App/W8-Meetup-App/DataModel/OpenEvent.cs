using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W8_Meetup_App.Common;

namespace W8_Meetup_App.Data
{
    class OpenEvent :BindableBase
    {
        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _status = string.Empty;
        public string Subtitle
        {
            get { return this._status; }
            set { this.SetProperty(ref this._status, value); }
        }

        public string Status;
        public string ID;
        public string Distance;
        public string Time;
        public string RsvpYes;
        public string Description;
        public string GroupID;
        public string GroupName;
        public string GroupWho;
    }
}
