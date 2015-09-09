using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// Captures and x,y,z value from a dialog
/// </summary>
public class XYZ:UserControl
{
    private Label label1;
    private Label label2;
    private VisualScenarioGenerator.Decimal decX;
    private VisualScenarioGenerator.Decimal decY;
    private VisualScenarioGenerator.Decimal decZ;
    private GroupBox groupBox1;
    private Label label3;


    public VisualScenarioGenerator.Decimal DecX
    {
        get { return decX; }
        set { decX = value; }
    }
    public VisualScenarioGenerator.Decimal DecY
    {
        get { return decY; }
        set { decY = value; }
    }
    public VisualScenarioGenerator.Decimal DecZ
    {
        get { return decZ; }
        set { decZ = value; }
    }
	public XYZ()
	{
        InitializeComponent();
	}

    private void InitializeComponent()
    {
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.decX = new VisualScenarioGenerator.Decimal();
        this.decY = new VisualScenarioGenerator.Decimal();
        this.decZ = new VisualScenarioGenerator.Decimal();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.SuspendLayout();
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(25, 9);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(14, 13);
        this.label1.TabIndex = 3;
        this.label1.Text = "X";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(100, 9);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(14, 13);
        this.label2.TabIndex = 4;
        this.label2.Text = "Y";
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(175, 9);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(14, 13);
        this.label3.TabIndex = 5;
        this.label3.Text = "Z";
        // 
        // decX
        // 
        this.decX.Location = new System.Drawing.Point(7, 30);
        this.decX.Name = "decX";
        this.decX.Size = new System.Drawing.Size(53, 21);
        this.decX.TabIndex = 6;
        this.decX.Value = 0;
        // 
        // decY
        // 
        this.decY.Location = new System.Drawing.Point(81, 29);
        this.decY.Name = "decY";
        this.decY.Size = new System.Drawing.Size(53, 21);
        this.decY.TabIndex = 7;
        this.decY.Value = 0;
        // 
        // decZ
        // 
        this.decZ.Location = new System.Drawing.Point(156, 29);
        this.decZ.Name = "decZ";
        this.decZ.Size = new System.Drawing.Size(53, 21);
        this.decZ.TabIndex = 8;
        this.decZ.Value = 0;
        // 
        // groupBox1
        // 
        this.groupBox1.Location = new System.Drawing.Point(0, 0);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(215, 61);
        this.groupBox1.TabIndex = 9;
        this.groupBox1.TabStop = false;
        // 
        // XYZ
        // 
        this.Controls.Add(this.decX);
        this.Controls.Add(this.decY);
        this.Controls.Add(this.decZ);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.groupBox1);
        this.Name = "XYZ";
        this.Size = new System.Drawing.Size(218, 64);
        this.ResumeLayout(false);
        this.PerformLayout();

    }
}
