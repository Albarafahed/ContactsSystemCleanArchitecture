namespace ContactsSystemWinForms
{
    public partial class frmAddEditContact : Form
    {
        private readonly ContactsApiClient _contacts;
        private readonly CountriesApiClient _countries;

        public enum enMode
        {
            AddNew = 0,
            Update = 1
        }


        private enMode _mode;

        private int _contactId;

        private ContactDto? _contact;

        public frmAddEditContact(
      ContactsApiClient contacts,
      CountriesApiClient countries,
      int contactId = -1)
        {
            InitializeComponent();

            _contacts = contacts;
            _countries = countries;

            _contactId = contactId;

            _mode = contactId == -1
                ? enMode.AddNew
                : enMode.Update;
        }

      
        private async Task LoadDataAsync()
        {
            cbCountry.DataSource = await _countries.GetAllAsync();

            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "CountryId";

            if (_mode == enMode.AddNew)
            {
                lblMode.Text = "Add New Contact";

                _contact = new ContactDto();

                return;
            }

            _contact =
                await _contacts.GetAsync(_contactId);

            if (_contact == null)
            {
                MessageBox.Show(
                    $"Contact with ID {_contactId} was not found.");

                Close();

                return;
            }

            lblMode.Text =
                $"Edit Contact ID = {_contactId}";

            lblContactID.Text =
                _contact.ContactId.ToString();

            txtFirstName.Text =
                _contact.FirstName;

            txtLastName.Text =
                _contact.LastName;

            txtEmail.Text =
                _contact.Email;

            txtPhone.Text =
                _contact.Phone;

            txtAddress.Text =
                _contact.Address;

            dtpDateOfBirth.Value =
                _contact.DateOfBirth;

            cbCountry.SelectedValue =
                _contact.CountryId;

            if (!string.IsNullOrEmpty(_contact.ImagePath))
            {
                pictureBox1.ImageLocation =
                    _contact.ImagePath;
            }

            llRemoveImage.Visible =
                !string.IsNullOrEmpty(_contact.ImagePath);
        }

        private async void frmAddEditContact_Load(
            object sender,
            EventArgs e)
        {
            await LoadDataAsync();
        }

        private async void btnSave_Click(
     object sender,
     EventArgs e)
        {
            if (_contact == null)
            {
                MessageBox.Show("Contact is not loaded.");
                return;
            }

            if (cbCountry.SelectedValue == null)
            {
                MessageBox.Show("Please select country.");
                return;
            }

            _contact.FirstName =
                txtFirstName.Text;

            _contact.LastName =
                txtLastName.Text;

            _contact.Email =
                txtEmail.Text;

            _contact.Phone =
                txtPhone.Text;

            _contact.Address =
                txtAddress.Text;

            _contact.DateOfBirth =
                dtpDateOfBirth.Value;

            _contact.CountryId =
                (int)cbCountry.SelectedValue;

            _contact.ImagePath =
                pictureBox1.ImageLocation ?? "";

            var saved = await _contacts.SaveAsync(_contact);
            if (saved is not null)
            {
                _contact = saved;
                MessageBox.Show(
                    "Data Saved Successfully.");

                _mode = enMode.Update;

                _contactId =
                    _contact.ContactId;

                lblMode.Text =
                    $"Edit Contact ID = {_contact.ContactId}";

                lblContactID.Text =
                    _contact.ContactId.ToString();
            }
            else
            {
                MessageBox.Show(
                    "Data was not saved.");
            }
        }

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

        private void llOpenFileDialog_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e)
        {

            openFileDialog1.Filter =
                "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                pictureBox1.ImageLocation =
                    openFileDialog1.FileName;


                llRemoveImage.Visible = true;

            }

        }


        private void llRemoveImage_LinkClicked(object sender,
            LinkLabelLinkClickedEventArgs e)
        {

            pictureBox1.ImageLocation = null;

            llRemoveImage.Visible = false;

        }

    }
}
