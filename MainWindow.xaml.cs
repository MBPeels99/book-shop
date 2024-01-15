using System;
using System.Collections;
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

namespace BookShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseControllers databaseControllers = new DatabaseControllers();

        private Boolean updateToggle    = false;
        private Boolean newBookToggle   = false;
        public MainWindow()
        {
            InitializeComponent();
            initializeBooks();
        }

        public void initializeBooks()
        {
            ArrayList allBooks = databaseControllers.getAllBooks();
            StackPanel stackPanel = new StackPanel();

            txtAuthor.Clear();
            txtTitle.Clear();
            txtISBN.Clear();
            txtDescription.Clear();

            for (int i = 0; i < allBooks.Count; i++)
            {
                Label label     = new Label();
                label.Content   = allBooks[i].ToString();
                label.FontSize  = 10;
                label.MouseEnter += new MouseEventHandler(label_MouseEnter);
                label.MouseLeave += new MouseEventHandler(label_MouseLeave);
                label.MouseLeftButtonDown += new MouseButtonEventHandler(label_MouseLeftButtonDown);
                stackPanel.Children.Add(label);
            }
            gbBookList.FontSize = 16;
            gbBookList.Content = stackPanel;
        }

        private void label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
            {
                label.FontSize = 16;
                label.Foreground = Brushes.Red;
            }
        }

        private void label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
            {
                label.FontSize = 10;
                label.Foreground = Brushes.Black;
            }
        }

        private void label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;

            if (label != null)
            {
                //TODO: Get info from Database
                string title = (string)label.Content;
                ArrayList bookInfo = databaseControllers.getBookInfo(title);

                txtISBN.Text         = bookInfo[0].ToString();
                txtTitle.Text        = bookInfo[1].ToString();
                txtAuthor.Text       = bookInfo[2].ToString();
                if (bookInfo[3] == null)
                {
                    txtDescription.Text = "Add Description here";
                } else
                {
                    txtDescription.Text = bookInfo[3].ToString();
                }
                
                // Set textboxes to read-only
                txtISBN.IsReadOnly          = true;
                txtTitle.IsReadOnly         = true;
                txtAuthor.IsReadOnly        = true;
                txtDescription.IsReadOnly   = true;
            }
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Set textboxes to editable
            txtTitle.IsReadOnly        = false;
            txtAuthor.IsReadOnly       = false;
            txtDescription.IsReadOnly  = false;

            //Set updateToggle to true, so that it can be checked in the save button
            updateToggle = true;

            //Turn off other toggle buttons
            newBookToggle = false;
        }

        private void buttonNewBook_Click(object sender, RoutedEventArgs e)
        {
            txtAuthor.Clear();
            txtTitle.Clear();
            txtISBN.Clear();
            txtDescription.Clear();

            txtISBN.IsReadOnly        = false;
            txtTitle.IsReadOnly       = false;
            txtAuthor.IsReadOnly      = false;
            txtDescription.IsReadOnly = false;

            newBookToggle   = true;

            updateToggle    = false;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            string isbn = txtISBN.Text;
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string description = txtDescription.Text;

            if (newBookToggle)
            {
                Boolean rowsAffected = databaseControllers.addNewBook(isbn, title, author, description);
                if (rowsAffected)
                {
                    MessageBox.Show("Book as been succesfully added","Succes",MessageBoxButton.OK);
                    initializeBooks();
                }
            }

            if (updateToggle)
            {
                Boolean rowsAffected = databaseControllers.updateBook(isbn, title, author, description);
                if (rowsAffected)
                {
                    MessageBox.Show("Book as been succesfully updated", "Succes", MessageBoxButton.OK);
                    initializeBooks();
                }
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure that you want to delete this book?", "WARNING", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Boolean rowsAffected = databaseControllers.deleteBookFromDB(txtISBN.Text);
                if (rowsAffected)
                {
                    MessageBox.Show("Book as been succesfully added", "Succes", MessageBoxButton.OK);
                    initializeBooks();
                }
            }
        }
    }
}
