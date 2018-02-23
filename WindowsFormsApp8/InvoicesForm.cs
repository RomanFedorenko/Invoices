using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestProject
{
    public partial class InvoicesForm : Form
    {
        public InvoicesForm()
        {
            InitializeComponent();
        }

        private void clientsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.clientsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.crmguruDataSet);
            MessageBox.Show("Изменения внесены в базу!");

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //filling clients table
            this.clientsTableAdapter.Fill(this.crmguruDataSet.Clients);

            //select first client to load some invoices for the startup
            if (clientsDataGridView.Rows.Count != 0)
            {
                clientsDataGridView.Rows[0].Selected = true;
            }
            else
            {
                MessageBox.Show("В базе нет клиентов");
            }

            //filling invoices table
            this.invoiceTableAdapter.Fill(this.crmguruDataSet.Invoice);

            //filter invoices by selected client and calculating total amount
            FilterInvoices();
            CalculateTotalAmount();




        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.invoiceBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.crmguruDataSet);
            MessageBox.Show("Изменения внесены в базу!");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            string filter = String.Empty;
            if (nameBox.Text != String.Empty)
            {
                filter += "[name] LIKE '%" + nameBox.Text + "%'";
            }
            if (surnameBox.Text != String.Empty)
            {
                if (filter != String.Empty)
                {
                    filter += " AND ";
                }
                filter += "[surname] LIKE '%" + surnameBox.Text + "%'";
            }
            if (patronymicBox.Text != String.Empty)
            {
                if (filter != String.Empty)
                {
                    filter += " AND ";
                }
                filter += "[patronymic] LIKE '%" + patronymicBox.Text + "%'";
            }

            if (filter != String.Empty)
            {
                clientsBindingSource.Filter = filter;
                if (clientsDataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("По вашому запросу ничего не найдено");
                    labelTotal.Text = "0";
                  

                }

            }
            else
            {
                MessageBox.Show("Введите данные для поиска!");
            }


        }

        private void clientsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (clientsDataGridView.SelectedRows.Count != 0)
            {
                FilterInvoices();
                CalculateTotalAmount();
            }

        }

        private void CalculateTotalAmount()
        {

            double total = 0;
            var ins = crmguruDataSet.Invoice.Where(x => x.client_id == Convert.ToInt32(clientsDataGridView.SelectedRows[0].Cells[0].Value));
            foreach (var invoice in ins)
            {
                total += invoice.amount;
            }
            labelTotal.Text = total.ToString();
        }

        private void FilterInvoices()
        {
            invoiceBindingSource.Filter = "client_id =" + clientsDataGridView.SelectedRows[0].Cells[0].Value.ToString();

        }
        private void buttonCount_Click(object sender, EventArgs e)
        {

        }
    }

    
}


