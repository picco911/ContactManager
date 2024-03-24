using System;
using System.Windows.Forms;

namespace ContactManager
{
    public partial class ContactList : Form
    {
        private bool _deleteForm = false;
        public ContactList()
        {
            InitializeComponent();
        }

        public ContactList(bool deleteForm)
        {
            InitializeComponent();
            this._deleteForm = deleteForm;
            this.Text = "Dzēst";
            btnOk.Text = "Dzēst";
        }

        private void LoadContact()
        {
            listContact.DataSource = null;
            var contacts = Sqlite.LoadContact();
            listContact.DataSource = contacts;
            listContact.DisplayMember = "FullName";
        }

        private void ContactList_Load(object sender, EventArgs e)
        {
            LoadContact();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditContact()
        {
            if (_deleteForm)
            {
                DialogResult dialogResult = MessageBox.Show("Dzēst?", "Dzēst?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var id = ((ContactManager.ContactModel)listContact.SelectedItem).Id;
                    Sqlite.DeleteContact(id);
                    LoadContact();
                }
            }
            else
            {
                var id = ((ContactManager.ContactModel)listContact.SelectedItem).Id;
                using (var form = new CreateEdit(Sqlite.LoadContact(id)))
                {
                    form.ShowDialog(this);
                    LoadContact();
                }
            }
        }

        private void listContact_DoubleClick(object sender, EventArgs e)
        {
            EditContact();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            EditContact();
        }
    }
}
