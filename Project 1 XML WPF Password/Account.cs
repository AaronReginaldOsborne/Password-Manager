using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1_XML_WPF_Password
{

    public class passwordmanager
    {

        private passwordmanagerAccount[] accountsField;

        public passwordmanagerAccount[] accounts
        {
            get
            {
                return this.accountsField;
            }
            set
            {
                this.accountsField = value;
            }
        }
    }

    public class passwordmanagerAccount
    {

        private string descriptionField;

        private passwordmanagerAccountPassword passwordField;

        private string loginurlField;

        private string accountnumberField;

        private string useridField;

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public passwordmanagerAccountPassword password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }

        public string loginurl
        {
            get
            {
                return this.loginurlField;
            }
            set
            {
                this.loginurlField = value;
            }
        }

        public string accountnumber
        {
            get
            {
                return this.accountnumberField;
            }
            set
            {
                this.accountnumberField = value;
            }
        }

        public string userid
        {
            get
            {
                return this.useridField;
            }
            set
            {
                this.useridField = value;
            }
        }
    }

    public class passwordmanagerAccountPassword
    {

        private string strengthField;

        private string percentageField;

        private string dateField;

        private string valueField;

        public string strength
        {
            get
            {
                return this.strengthField;
            }
            set
            {
                this.strengthField = value;
            }
        }

        public string percentage
        {
            get
            {
                return this.percentageField;
            }
            set
            {
                this.percentageField = value;
            }
        }

        public string date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
