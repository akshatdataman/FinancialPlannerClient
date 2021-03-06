﻿using FinancialPlanner.Common;
using FinancialPlanner.Common.Model;
using FinancialPlannerClient.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialPlannerClient
{
    public partial class Main : Form
    {
        private const string AUDITLOGCONTROLLER ="Activities/Add";
        public Main()
        {
            InitializeComponent();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {            
            try
            {
                FinancialPlanner.Common.JSONSerialization jsonSerialization = new FinancialPlanner.Common.JSONSerialization();
                string apiurl = Program.WebServiceUrl +"/"+ AUDITLOGCONTROLLER;

                Activities activity = new Activities();
                activity.ActivityTypeValue = ActivityType.Logout;
                activity.EntryType = EntryStatus.Success;
                activity.SourceType = Source.Server;
                activity.HostName = Environment.MachineName;
                activity.UserName = Program.CurrentUser.UserName;

                string DATA =  jsonSerialization.SerializeToString<Activities>(activity);

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(apiurl, DATA);

                if (json != null)
                {
                    var resultObject = jsonSerialization.DeserializeFromString<Result>(json);
                }
            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex.ToString());
            }
        }

        private void lstViewProsCust_Click(object sender, EventArgs e)
        {
            ProspectCustomer.ProspectCustomerList frmViewAllProsCust= new ProspectCustomer.ProspectCustomerList();
            frmViewAllProsCust.Dock = DockStyle.Fill;
            frmViewAllProsCust.ShowDialog();
        }

        private void prospectedCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProspectCustomer.ProspectCustomerList frmViewAllProsCust= new ProspectCustomer.ProspectCustomerList();
            frmViewAllProsCust.TopLevel = false;
            this.pnlMain.Controls.Add(frmViewAllProsCust);
            frmViewAllProsCust.Dock = DockStyle.Fill;
            frmViewAllProsCust.Show();
        }

        private void pnlMain_ControlAdded(object sender, ControlEventArgs e)
        {
            if (pnlMain.Controls.Count > 1)
                pnlMain.Controls[0].Visible = false;
        }

        private void pnlMain_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (pnlMain.Controls.Count == 1)
                pnlMain.Controls[0].Visible = true;
        }

        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {          
        }

        private void dataGaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clients.ClientList frmClientList = new Clients.ClientList();
            frmClientList.TopLevel = false;
            this.pnlMain.Controls.Add(frmClientList);
            frmClientList.Dock = DockStyle.Fill;
            frmClientList.Show();
        }

        private void createNewRsikProfiledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RiskProfile.frmRiskProfiledReturnList frmriskProfileReturnList =
                new RiskProfile.frmRiskProfiledReturnList();
            frmriskProfileReturnList.TopLevel = false;
            this.pnlMain.Controls.Add(frmriskProfileReturnList);
            frmriskProfileReturnList.Dock = DockStyle.Fill;
            frmriskProfileReturnList.Show();
        }

        private void estimatedPlanningOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlanOptions.ClientPlanOptionList frmPlanOption = new PlanOptions.ClientPlanOptionList();
            frmPlanOption.TopLevel = false;
            this.pnlMain.Controls.Add(frmPlanOption);
            frmPlanOption.Dock = DockStyle.Fill;
            frmPlanOption.Show();
        }

        private void currentInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentStatus.ClientCurrentStatusList frmClientCurrrentStatusLst =
                new CurrentStatus.ClientCurrentStatusList();
            frmClientCurrrentStatusLst.TopLevel = false;
            this.pnlMain.Controls.Add(frmClientCurrrentStatusLst);
            frmClientCurrrentStatusLst.Dock = DockStyle.Fill;
            frmClientCurrrentStatusLst.Show();
        }

        private void investmentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clients.AllClientsList frmClientList = new Clients.AllClientsList();
            frmClientList.TopLevel = false;
            this.pnlMain.Controls.Add(frmClientList);
            frmClientList.Dock = DockStyle.Fill;
            frmClientList.Show();
        }

        private void testingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Testing testing = new Testing();
            testing.ShowDialog();
        }

        private void festivalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Others other = new Others("Festivals");
            other.ShowDialog();
        }

        private void cRMGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Others other = new Others("CRM Groups");
            other.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void auditTrailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuditTrail.AuditTrail auditTrail = new AuditTrail.AuditTrail();
            auditTrail.TopLevel = false;
            this.pnlMain.Controls.Add(auditTrail);
            auditTrail.Dock = DockStyle.Fill;
            auditTrail.Show();
        }

        private void areasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Others other = new Others("Areas");
            other.ShowDialog();
        }
    }
}
