using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1_XML_WPF_Password
{
    class TextFieldsViewModel : INotifyPropertyChanged
    {
        private string _userID;
        private string _desc;
        private string _pass;
        private DateTime? _date;


        public TextFieldsViewModel()
        {
        }

        public string UserID
        {
            get { return _userID; }
            set
            {
                this.MutateVerbose(ref _userID, value, RaisePropertyChanged());
            }
        }

        public string Description
        {
            get { return _desc; }
            set
            {
                this.MutateVerbose(ref _desc, value, RaisePropertyChanged());
            }
        }

        public string Password
        {
            get { return _pass; }
            set
            {
                this.MutateVerbose(ref _pass, value, RaisePropertyChanged());
            }
        }

        public DateTime? Date
        {
            get { return _date; }
            set
            {
                _date = value;
                this.MutateVerbose(ref _date, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
