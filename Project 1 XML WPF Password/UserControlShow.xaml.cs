using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for UserControlShow.xaml
    /// </summary>
    public partial class UserControlShow : UserControl
    {
        private XmlDocument _doc;
        private Password password;
        public UserControlShow()
        {
            InitializeComponent();
        }

        public UserControlShow(XmlDocument doc)
        {
            InitializeComponent();
            password = new Password(doc);
            LoadFromFile(doc);
        }

        //Helper Methods
        private void LoadFromFile(XmlDocument doc)
        {
            //grab all accounts from node
            XmlNodeList account = doc.GetElementsByTagName("account");

            var numCol = 3;

            for (var i = 0; i < numCol; ++i)
            {
                ColumnDefinition tempCol = new ColumnDefinition();
                tempCol.Width = new GridLength(1, GridUnitType.Star);
                Accounts.ColumnDefinitions.Add(tempCol);
            }

            for (var i = 0; i < account.Count; ++i)
            {
                if (i % numCol == 0)
                {
                    RowDefinition tempRow = new RowDefinition();
                    tempRow.Height = new GridLength(1, GridUnitType.Star);
                    Accounts.RowDefinitions.Add(tempRow);
                }
            }

            int newRowCounter = 0;

            for (int i = 0; i < account.Count; i++)
            {
                passwordmanagerAccount currentAccount = password.GrabAccount(i);


                var flipper = new Flipper();
                flipper.Margin = new Thickness(20);

                if (i >= numCol)
                    if (i % numCol == 0)
                        newRowCounter++;

                flipper.SetValue(Grid.RowProperty, newRowCounter);
                flipper.SetValue(Grid.ColumnProperty, (i % numCol));



                Grid innerGrid = new Grid();
                innerGrid.Height = 300;
                innerGrid.Width = 200;

                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(160);
                RowDefinition gridRow2 = new RowDefinition();
                gridRow2.Height = new GridLength(1, GridUnitType.Star);

                innerGrid.RowDefinitions.Add(gridRow1);
                innerGrid.RowDefinitions.Add(gridRow2);

                var colorZone = new ColorZone();
                colorZone.Mode = ColorZoneMode.PrimaryLight;
                colorZone.VerticalAlignment = VerticalAlignment.Stretch;
                innerGrid.Children.Add(colorZone);

                PackIcon AccountCircle = new PackIcon();
                AccountCircle.Kind = PackIconKind.AccountCircle;
                AccountCircle.Height = 128;
                AccountCircle.Width = 128;
                AccountCircle.VerticalAlignment = VerticalAlignment.Center;
                AccountCircle.HorizontalAlignment = HorizontalAlignment.Center;
                innerGrid.Children.Add(AccountCircle);


                StackPanel sp = new StackPanel();
                sp.SetValue(Grid.RowProperty, 1);
                sp.VerticalAlignment = VerticalAlignment.Center;

                TextBlock userID = new TextBlock();
                userID.Text = currentAccount.description;
                userID.HorizontalAlignment = HorizontalAlignment.Center;

                sp.Children.Add(userID);

                Button flipperEditButton = new Button();
                flipperEditButton.Style = (Style)Application.Current.TryFindResource("MaterialDesignFlatButton");
                flipperEditButton.Content = "EDIT";
                flipperEditButton.Margin = new Thickness(0, 4, 0, 0);
                flipperEditButton.Command = Flipper.FlipCommand;
                sp.Children.Add(flipperEditButton);

                Button flipperDeleteButton = new Button();
                flipperDeleteButton.Style = (Style)Application.Current.TryFindResource("MaterialDesignFlatButton");
                flipperDeleteButton.Content = "DELETE";
                flipperDeleteButton.Margin = new Thickness(0, 4, 0, 0);
                flipperDeleteButton.Command = new Delete(i);
                sp.Children.Add(flipperDeleteButton);

                innerGrid.Children.Add(sp);

                flipper.FrontContent = innerGrid;

                //Back side of flipper

                Grid backGrid = new Grid();
                backGrid.Height = 300;
                backGrid.Width = 200;

                RowDefinition backGridRow1 = new RowDefinition();
                backGridRow1.Height = new GridLength(1, GridUnitType.Auto);
                RowDefinition backGridRow2 = new RowDefinition();
                backGridRow2.Height = new GridLength(1, GridUnitType.Star);

                backGrid.RowDefinitions.Add(backGridRow1);
                backGrid.RowDefinitions.Add(backGridRow2);

                ColorZone backColorZone = new ColorZone();
                backColorZone.Mode = ColorZoneMode.Accent;
                backColorZone.Padding = new Thickness(6);

                StackPanel backStack = new StackPanel();
                backStack.Orientation = Orientation.Horizontal;

                Button backButton = new Button();
                backButton.Style = (Style)Application.Current.TryFindResource("MaterialDesignToolForegroundButton");
                backButton.Command = Flipper.FlipCommand;
                backButton.HorizontalAlignment = HorizontalAlignment.Left;

                PackIcon backArrowLeft = new PackIcon();
                backArrowLeft.Kind = PackIconKind.ArrowLeft;
                backArrowLeft.HorizontalAlignment = HorizontalAlignment.Right;

                backButton.Content = backArrowLeft;
                backStack.Children.Add(backButton);

                TextBlock backEditUser = new TextBlock();
                backEditUser.Margin = new Thickness(8, 0, 0, 0);
                backEditUser.VerticalAlignment = VerticalAlignment.Center;
                backEditUser.Text = "EDIT USER";
                backStack.Children.Add(backEditUser);

                backColorZone.Content = backStack;

                backGrid.Children.Add(backColorZone);

                Grid backInformationGrid = new Grid();
                backInformationGrid.SetValue(Grid.RowProperty, 1);
                backInformationGrid.Margin = new Thickness(0, 6, 0, 0);
                backInformationGrid.HorizontalAlignment = HorizontalAlignment.Center;
                backInformationGrid.VerticalAlignment = VerticalAlignment.Top;
                backInformationGrid.Width = 172;

                for (var j = 0; j < 5; ++j)
                {
                    RowDefinition tempRow = new RowDefinition();
                    tempRow.Height = new GridLength(1, GridUnitType.Star);
                    backInformationGrid.RowDefinitions.Add(tempRow);
                }

                //                    < TextBox materialDesign: HintAssist.Hint = "User ID:" materialDesign: HintAssist.IsFloating = "True"
                //                 Margin = "0 12 0 0" > trandall </ TextBox >

                TextBox backUserID = new TextBox();
                backUserID.Name = "tbUserID"+i.ToString();
                backUserID.Margin = new Thickness(0, 12, 0, 0);
                backUserID.Text = currentAccount.userid;
                HintAssist.SetHint(backUserID, "User ID:");
                HintAssist.SetIsFloating(backUserID, true);
                backUserID.KeyDown += OnKeyDownHandler;
                backInformationGrid.Children.Add(backUserID);

                //                    < TextBox Grid.Row = "1" materialDesign: HintAssist.Hint = "Desciption" materialDesign: HintAssist.IsFloating = "True"
                //                 Margin = "0 12 0 0" > CIBC Online Banking</ TextBox >

                TextBox backDescrip = new TextBox();
                backDescrip.SetValue(Grid.RowProperty, 1);
                backDescrip.Margin = new Thickness(0, 12, 0, 0);
                backDescrip.Text = currentAccount.description;
                HintAssist.SetHint(backDescrip, "Desciption:");
                HintAssist.SetIsFloating(backDescrip, true);
                backInformationGrid.Children.Add(backDescrip);

                //                        < TextBox Grid.Row = "2" materialDesign: HintAssist.Hint = "Password" materialDesign: HintAssist.IsFloating = "True"
                //                 Margin = "0 12 0 0" > pug12345 </ TextBox >

                TextBox backPassword = new TextBox();
                backPassword.SetValue(Grid.RowProperty, 2);
                backPassword.Margin = new Thickness(0, 12, 0, 0);
                backPassword.Text = currentAccount.password.Value;
                HintAssist.SetHint(backPassword, "Password:");
                HintAssist.SetIsFloating(backPassword, true);
                backInformationGrid.Children.Add(backPassword);

                //                    < TextBox Grid.Row = "3" materialDesign: HintAssist.Hint = "Log-in URL" materialDesign: HintAssist.IsFloating = "True"
                //                 Margin = "0 12 0 0" > very weak </ TextBox >

                TextBox backLogURL = new TextBox();
                backLogURL.SetValue(Grid.RowProperty, 3);
                backLogURL.Margin = new Thickness(0, 12, 0, 0);
                backLogURL.Text = currentAccount.loginurl;
                HintAssist.SetHint(backLogURL, "Log-in URL:");
                HintAssist.SetIsFloating(backLogURL, true);
                backInformationGrid.Children.Add(backLogURL);

                //                        < TextBox Grid.Row = "4" materialDesign: HintAssist.Hint = "Account Number" materialDesign: HintAssist.IsFloating = "True"
                //                 Margin = "0 12 0 0" > very weak </ TextBox >

                TextBox backAccountNumber = new TextBox();
                backAccountNumber.SetValue(Grid.RowProperty, 4);
                backAccountNumber.Margin = new Thickness(0, 12, 0, 0);
                backAccountNumber.Text = currentAccount.accountnumber;
                HintAssist.SetHint(backAccountNumber, "Account Number:");
                HintAssist.SetIsFloating(backAccountNumber, true);
                backInformationGrid.Children.Add(backAccountNumber);

                backGrid.Children.Add(backInformationGrid);
                flipper.BackContent = backGrid;

                Accounts.Children.Add(flipper);
            }
        }

        public void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            var value = sender as TextBox;
            if (e.Key == Key.Return)
            {
                MessageBox.Show("You Entered: " + value.Name.ToString());
                int removeIndexFromWord = Int32.Parse(value.Name.Replace("tbUserID", ""));
                var account = password.GrabAccount(removeIndexFromWord);
                password.SaveAccount(account, removeIndexFromWord);
            }
        }

        class Delete : ICommand
        {
            private int deleteIndex = 0;
            public Delete(int index)
            {
                deleteIndex = index;
            }
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                MessageBox.Show("Deleting " + deleteIndex);
            }
        }
    }
}
