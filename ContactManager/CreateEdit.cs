using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactManager
{
    public partial class CreateEdit : Form
    {
        private ContactModel _contact;
        public CreateEdit()
        {
            InitializeComponent();
        }

        public CreateEdit(ContactModel contact)
        {
            InitializeComponent();
            Text = "Rediģēt";
            _contact = contact;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ContactModel c = new ContactModel();
            c.FirstName = textFirstName.Text;
            c.LastName = textLastName.Text;
            c.Email = textEmail.Text;
            c.Address = textAddress.Text;

            if (_contact == null)
            {
                Sqlite.SaveContact(c);
            }
            else
            {
                c.Id = _contact.Id;
                Sqlite.UpdateContact(c);                
            }

            Close();
        }

        private void CreateEdit_Load(object sender, EventArgs e)
        {
            if(_contact != null)
            {
                textLastName.Text = _contact.LastName;
                textEmail.Text = _contact.Email;
                textFirstName.Text = _contact.FirstName;
                textAddress.Text = _contact.Address;
            }
        }
    }
}
