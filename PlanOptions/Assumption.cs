﻿using FinancialPlanner.Common;
using FinancialPlanner.Common.Model;
using FinancialPlannerClient.Master;
using FinancialPlannerClient.PlannerInfo;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace FinancialPlannerClient.PlanOptions
{
    public partial class Assumption : DevExpress.XtraEditors.XtraForm
    {
        Client client;
        Planner planner;
        string spouseName;
        int spouseAge;
        public Assumption()
        {
            InitializeComponent();
        }
        public Assumption(Client client, Planner planner, string spouseName, int spouseAge)
        {
            InitializeComponent();
            this.client = client;
            this.planner = planner;
            this.spouseName = spouseName;
            this.spouseAge = spouseAge;
            fillupAssumptionInfo();
        }

        private void fillupAssumptionInfo()
        {
            lblClientTiitle.Text = client.Name;
            lblSpouseTitle.Text = spouseName;
            PlannerAssumptionInfo plannerassumptionInfo = new PlannerAssumptionInfo();
            PlannerAssumption plannerAssumption = plannerassumptionInfo.GetAll(planner.ID);
            displayPlannerAssumptionData(plannerAssumption);
        }

        private void displayPlannerAssumptionData(PlannerAssumption plannerAssumption)
        {
            if (plannerAssumption.Id != 0)
            {
                txtClientRetAge.Tag = plannerAssumption.Id;
                txtClientRetAge.Text = plannerAssumption.ClientRetirementAge.ToString();
                txtSpouseRetAge.Text = plannerAssumption.SpouseRetirementAge.ToString();
                txtClientLifeExp.Text = plannerAssumption.ClientLifeExpectancy.ToString();
                txtSpouseLifeExp.Text = plannerAssumption.SpouseLifeExpectancy.ToString();
                txtPreRetInflationRate.Text = plannerAssumption.PreRetirementInflactionRate.ToString();
                txtPostRetInflationRate.Text = plannerAssumption.PostRetirementInflactionRate.ToString();
                txtEquityReturn.Text = plannerAssumption.EquityReturnRate.ToString();
                txtDebtReturn.Text = plannerAssumption.DebtReturnRate.ToString();
                txtOtherReturn.Text = plannerAssumption.OtherReturnRate.ToString();
                txtPlannerAssumptionDescription.Text = plannerAssumption.Decription;
                rdoPrimaryRetirement.Enabled = this.client.IsMarried;
                rdoPrimaryRetirement.SelectedIndex = (plannerAssumption.Id == 0) ? 0 :
                    (plannerAssumption.IsClientRetirmentAgeIsPrimary) ? 0 : 1;
                txtIncomeRiseForClient.Text = plannerAssumption.ClientIncomeRise.ToString();
                txtIncomeRiseForSpouse.Text = plannerAssumption.SpouseIncomeRise.ToString();
                txtOngoingExpRise.Text = plannerAssumption.OngoingExpRise.ToString();
            }
            else
                btnAdd.Visible = true;
        }

        private void Assumption_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Parent != null)
                this.Parent.Controls.Clear();
        }

        private void btnSaveAssumption_Click(object sender, EventArgs e)
        {
            try
            {
                PlannerAssumptionInfo PlannerAssumptionInfo = new PlannerAssumptionInfo();
                PlannerAssumption PlannerAssumption = getPlannerAssumptionData();
                bool isSaved = false;

                isSaved = PlannerAssumptionInfo.Update(PlannerAssumption);

                if (isSaved)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Record save successfully.", "Record Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fillupAssumptionInfo();
                }
                else
                    DevExpress.XtraEditors.XtraMessageBox.Show("Unable to save record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace();
                StackFrame sf = st.GetFrame(0);
                MethodBase currentMethodName = sf.GetMethod();
                LogDebug(currentMethodName.Name, ex);
            }
        }
        private void LogDebug(string methodName, Exception ex)
        {
            DebuggerLogInfo debuggerInfo = new DebuggerLogInfo();
            debuggerInfo.ClassName = this.GetType().Name;
            debuggerInfo.Method = methodName;
            debuggerInfo.ExceptionInfo = ex;
            Logger.LogDebug(debuggerInfo);
        }
        private PlannerAssumption getPlannerAssumptionData()
        {
            PlannerAssumption plannerAssumption = new PlannerAssumption();
            plannerAssumption.Id = int.Parse(txtClientRetAge.Tag.ToString());
            plannerAssumption.Pid = this.planner.ID;
            plannerAssumption.ClientRetirementAge = int.Parse(txtClientRetAge.Text);
            plannerAssumption.SpouseRetirementAge = int.Parse(txtSpouseRetAge.Text);
            plannerAssumption.ClientLifeExpectancy = int.Parse(txtClientLifeExp.Text);
            plannerAssumption.SpouseLifeExpectancy = int.Parse(txtSpouseLifeExp.Text);
            plannerAssumption.PreRetirementInflactionRate = decimal.Parse(txtPreRetInflationRate.Text);
            plannerAssumption.PostRetirementInflactionRate = decimal.Parse(txtPostRetInflationRate.Text);
            plannerAssumption.EquityReturnRate = decimal.Parse(txtEquityReturn.Text);
            plannerAssumption.DebtReturnRate = decimal.Parse(txtDebtReturn.Text);
            plannerAssumption.OtherReturnRate = decimal.Parse(txtOtherReturn.Text);
            plannerAssumption.Decription = txtPlannerAssumptionDescription.Text;
            plannerAssumption.UpdatedOn = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            plannerAssumption.UpdatedBy = Program.CurrentUser.Id;
            plannerAssumption.UpdatedByUserName = Program.CurrentUser.UserName;
            plannerAssumption.MachineName = System.Environment.MachineName;
            plannerAssumption.IsClientRetirmentAgeIsPrimary = (rdoPrimaryRetirement.SelectedIndex == 0 ? true : false);
            plannerAssumption.ClientIncomeRise = decimal.Parse(txtIncomeRiseForClient.Text);
            plannerAssumption.SpouseIncomeRise = decimal.Parse(txtIncomeRiseForSpouse.Text);
            plannerAssumption.OngoingExpRise = decimal.Parse(txtOngoingExpRise.Text);

            return plannerAssumption;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AssumptionMasterInfo assumptionMasterInfo = new AssumptionMasterInfo();
            AssumptionMaster assumptionMaster = assumptionMasterInfo.GetAll();
            fillupDefaultAssumption(assumptionMaster);
        }

        private void fillupDefaultAssumption(AssumptionMaster assumptionMaster)
        {
            txtClientRetAge.Tag = "0";
            txtClientRetAge.Text = assumptionMaster.RetirementAge.ToString();
            txtSpouseRetAge.Text = assumptionMaster.RetirementAge.ToString();
            txtClientLifeExp.Text = assumptionMaster.LifeExpectancy.ToString();
            txtSpouseLifeExp.Text = assumptionMaster.LifeExpectancy.ToString();
            txtPreRetInflationRate.Text = assumptionMaster.PreRetirementInflactionRate.ToString();
            txtPostRetInflationRate.Text = assumptionMaster.PostRetirementInflactionRate.ToString();
            txtEquityReturn.Text = assumptionMaster.EquityReturnRate.ToString();
            txtDebtReturn.Text = assumptionMaster.DebtReturnRate.ToString();
            txtOtherReturn.Text = assumptionMaster.OtherReturnRate.ToString();
            txtPlannerAssumptionDescription.Text = "";

            txtIncomeRiseForClient.Text = assumptionMaster.IncomeRaiseRatio.ToString();
            txtIncomeRiseForSpouse.Text = assumptionMaster.IncomeRaiseRatio.ToString();
            txtOngoingExpRise.Text = assumptionMaster.OngoingExpRise.ToString();
        }

        private void txtSpouseLifeExp_Validating(object sender, CancelEventArgs e)
        {
            if (txtSpouseLifeExp.Text == "0" || string.IsNullOrEmpty(txtSpouseLifeExp.Text))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Invalid value for spouse life Expectancy", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(txtSpouseLifeExp.Text) && int.Parse(txtSpouseLifeExp.Text) < spouseAge)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Spouse expected life age should not be less than spouse current age.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }
            }
        }

        private void txtSpouseLifeExp_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows 0-9, backspace, and decimal
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }
    }
}
