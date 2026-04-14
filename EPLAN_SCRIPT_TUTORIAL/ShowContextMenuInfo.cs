#region should be included to avoid namespace conflict
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
# endregion

using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.Gui;
using Eplan.EplApi.Scripting;

namespace EPLAN_SCRIPT_TUTORIAL
{
    public class ShowContextMenuInfo
    {
        // set refreshAfterChanges true to avoid flickering problem.
        RibbonBar myRibbonBar = new RibbonBar(true);

        public const string ActionName = "ShowContextMenuInfo";

        [DeclareAction(ActionName)]
        public void ShowDialogFunc()
        {
            FrmSelect frm = new FrmSelect();
            frm.ShowDialog();
        }

        #region extend UI
        [DeclareRegister]
        public void RegisterRibbonItems()
        {
            var newTab = myRibbonBar.Tabs.FirstOrDefault(item => item.Name == m_newTabName);
            if (newTab == null) newTab = myRibbonBar.AddTab(m_newTabName);

            var cmdGroup = newTab.CommandGroups.FirstOrDefault(item => item.Name == m_commandGroupName);
            if (cmdGroup == null) cmdGroup = newTab.AddCommandGroup(m_commandGroupName, 0);

            var command = cmdGroup.AddCommand(m_commandName, ActionName);
        }

        [DeclareUnregister]
        public void UnRegisterRibbonItems()
        {
            CleanCommand(ActionName);
        }

        void CleanCommand(string actionCommandLine)
        {
            var newTab = myRibbonBar.Tabs
                .FirstOrDefault(item => item.Name == m_newTabName);
            if (newTab == null) return;

            var cmdGroup = newTab.CommandGroups.FirstOrDefault(item => item.Name == m_commandGroupName);
            if (cmdGroup == null) return;

            var command = cmdGroup.Commands.FirstOrDefault(item => item.Value.ActionCommandLine == ActionName).Value;
            if (command == null) return;

            command.Remove();
        }

        public string m_newTabName = "EPLAN_SCRIPT_TUTORIAL";
        public string m_commandGroupName = "Common";
        public string m_commandName = ActionName;
        #endregion
    }

    public partial class FrmSelect : Form
    {
        public FrmSelect()
        {
            InitializeComponent();
            Eplan.EplApi.Base.Settings oSettings = new Eplan.EplApi.Base.Settings();
            checkBox1.Checked = oSettings.GetBoolSetting("USER.EnfMVC.ContextMenuSetting.ShowIdentifier", 0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Eplan.EplApi.Base.Settings oSettings = new Eplan.EplApi.Base.Settings();
            oSettings.SetBoolSetting("USER.EnfMVC.ContextMenuSetting.ShowIdentifier", checkBox1.Checked, 0);
        }
    }

    partial class FrmSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(39, 11);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(149, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "是否显示ContextMenuIfo";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // FrmSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 40);
            this.Controls.Add(this.checkBox1);
            this.Name = "FrmSelect";
            this.Text = "设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
    }
}
