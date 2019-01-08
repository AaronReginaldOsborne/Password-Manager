using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Project_1_XML_WPF_Password
{
    /// <summary>
    /// Interaction logic for UserControlAddPassword.xaml
    /// </summary>
    public partial class UserControlAddPassword : UserControl
    {
        XmlDocument localDoc;
        public UserControlAddPassword()
        {
            InitializeComponent();
        }

        public UserControlAddPassword(XmlDocument doc)
        {
            InitializeComponent();
            DataContext = new TextFieldsViewModel();
            localDoc = doc;
        }

        private void btnSavePass_Click(object sender, RoutedEventArgs e)
        {
            Password password = new Password(localDoc);
            password.AddItem(
                tbDesc.Text,
                tbUserID.Text,
                tbPass.Text,
                dpDate.Text,
                tbLog.Text,
                tbAcc.Text
                );
            ClearFields();
        }

        private void ClearFields()
        {
            tbDesc.Text = "";
            tbUserID.Text = "";
            tbPass.Text = "";
            dpDate.Text = "";
            tbLog.Text = "";
            tbAcc.Text = "";
        }
    }
}
