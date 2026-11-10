using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ContactsSystemWinForms
{
    public partial class frmListContacts : Form
    {
        private readonly ContactsApiClient _contacts;
        private readonly IFormNavigator _navigator;

        public frmListContacts(
      ContactsApiClient contacts,
      IFormNavigator navigator)
        {
            InitializeComponent();

            _contacts = contacts;
            _navigator = navigator;
        }

        private async Task RefreshContactsListAsync()
        {
            dgvAllContacts.DataSource = await _contacts.GetAllAsync();
        }
       
        public async Task LoadContactsAsync()
        {
            var contacts = await _contacts.GetAllAsync();

            dgvAllContacts.DataSource = contacts;
        }
        private void frmListContacts_Load(object sender, EventArgs e)
        {
            //await LoadContactsAsync();
        }

        private async void btnAddNewContact_Click(object sender, EventArgs e)
        {
            _navigator.ShowDialog<frmAddEditContact>(-1);

            await RefreshContactsListAsync();
        }

        private async void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvAllContacts.CurrentRow == null)
                return;

            int contactId =
                (int)dgvAllContacts.CurrentRow.Cells[0].Value;

            _navigator.ShowDialog<frmAddEditContact>(contactId);

            await RefreshContactsListAsync();
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvAllContacts.CurrentRow == null)
                return;

            int contactId =
                (int)dgvAllContacts.CurrentRow.Cells[0].Value;

            if (MessageBox.Show(
                $"Are you sure you want to delete contact [{contactId}]?",
                "Confirm Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) != DialogResult.OK)
            {
                return;
            }

            bool deleted = await _contacts.DeleteAsync(contactId);

            if (deleted)
            {
                MessageBox.Show("Contact deleted successfully.");

                await RefreshContactsListAsync();
            }
            else
            {
                MessageBox.Show("Contact was not deleted.");
            }
        }

        private void dgvAllContacts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAllContacts.CurrentRow == null)
                return;

            textBox1.Text =
                dgvAllContacts.CurrentRow.Cells[0].Value?.ToString();

            textBox2.Text =
                dgvAllContacts.CurrentRow.Cells[1].Value?.ToString();

            textBox3.Text =
                dgvAllContacts.CurrentRow.Cells[2].Value?.ToString();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await RefreshContactsListAsync();
        }

        private async void frmListContacts_Shown(object sender, EventArgs e)
        {
            Hide();

            try
            {
                await LoadContactsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load contacts:\n{ex.Message}",
                    "Loading Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            // إظهار النافذة بعد انتهاء التحميل
            Show();

        }
    }
}
