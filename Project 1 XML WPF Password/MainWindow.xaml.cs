using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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


// Namespaces added manually
using System.Xml;      // XmlReader, XmlDocument and XmlReaderSetting classes
using System.Xml.Schema;  // XmlSchemaValidationFlags class
using System.IO;      // File class


namespace Project_1_XML_WPF_Password
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Member variables
        private static string xmlFilePath = "";
        private static XmlDocument doc = null;
        public static XmlReader reader;
        private static XmlReaderSettings settings;
        public MainWindow()
        {
            InitializeComponent();

            // Set the validation settings
            settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            ValidationEventHandler handler
                = new ValidationEventHandler(ValidationCallback);
            settings.ValidationEventHandler += handler;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
        }
        private void ButtonQuit_Click(object sender, RoutedEventArgs e)
        {
            //close parser before the file save
            reader.Close();
            //Final save
            doc.Save(xmlFilePath);
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                //home
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new UserControlHome());
                    break;
                // open file
                case 1:
                    OpenXmlFile();
                    break;
                // Add page
                case 2:
                    GridPrincipal.Children.Clear();
                    if (doc != null)
                        GridPrincipal.Children.Add(new UserControlAddPassword(doc));
                    break;
                // Show data
                case 3:
                    GridPrincipal.Children.Clear();
                    if (doc != null)
                        GridPrincipal.Children.Add(new UserControlShow(doc));
                    break;
                default:
                    break;
            }
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }

        private void OpenXmlFile()
        {
            try
            {
                // Allow the user to select a class library
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "XML|*.xml";
                ofd.ValidateNames = true;
                ofd.Multiselect = false;

                // Show the dialog
                bool? result = ofd.ShowDialog();


                if (result == true)
                {
                    try
                    {
                        xmlFilePath = ofd.FileName;
                        // Create the XmlReader object and read/validate the XML file
                        reader = XmlReader.Create(xmlFilePath, settings);

                        // Load the xml into the DOM
                        doc = new XmlDocument();
                        doc.Load(reader);
                        /* d. If the XML file is valid, load the XML into the DOM, display the name of the
                              user as recorded in the XML file, and allow the current user to choose from
                              any of the following activities: */
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
            }
        }

        // Callback method to display validation errors and warnings if the xml file is invalid
        // according to its schema
        private static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\n{0,-20}{1}", "WARNING:", args.Message);
            else
            {
                Console.WriteLine("\n{0,-20}{1}", "SCHEMA ERROR:", args.Message);
            }
        } // end ValidationCallback()

    }
}
