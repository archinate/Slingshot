<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class form_DataGrid
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.Label_Slingshot = New System.Windows.Forms.Label()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.Button1 = New System.Windows.Forms.Button()
    Me.DataGridView_Grasshopper = New System.Windows.Forms.DataGridView()
    CType(Me.DataGridView_Grasshopper, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'Label_Slingshot
    '
    Me.Label_Slingshot.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.Label_Slingshot.AutoSize = True
    Me.Label_Slingshot.BackColor = System.Drawing.Color.Transparent
    Me.Label_Slingshot.Font = New System.Drawing.Font("Arial", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label_Slingshot.Location = New System.Drawing.Point(12, 316)
    Me.Label_Slingshot.Margin = New System.Windows.Forms.Padding(0)
    Me.Label_Slingshot.Name = "Label_Slingshot"
    Me.Label_Slingshot.Size = New System.Drawing.Size(113, 37)
    Me.Label_Slingshot.TabIndex = 0
    Me.Label_Slingshot.Text = "SLING"
    '
    'Label1
    '
    Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.Label1.AutoSize = True
    Me.Label1.BackColor = System.Drawing.Color.Transparent
    Me.Label1.Font = New System.Drawing.Font("Arial", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label1.ForeColor = System.Drawing.Color.DarkOrange
    Me.Label1.Location = New System.Drawing.Point(113, 316)
    Me.Label1.Margin = New System.Windows.Forms.Padding(0)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(114, 37)
    Me.Label1.TabIndex = 1
    Me.Label1.Text = "SHOT!"
    '
    'Button1
    '
    Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.Button1.Location = New System.Drawing.Point(417, 316)
    Me.Button1.Name = "Button1"
    Me.Button1.Size = New System.Drawing.Size(105, 37)
    Me.Button1.TabIndex = 2
    Me.Button1.Text = "Close"
    Me.Button1.UseVisualStyleBackColor = True
    '
    'DataGridView_Grasshopper
    '
    Me.DataGridView_Grasshopper.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.DataGridView_Grasshopper.BackgroundColor = System.Drawing.SystemColors.Control
    Me.DataGridView_Grasshopper.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
    Me.DataGridView_Grasshopper.Location = New System.Drawing.Point(12, 12)
    Me.DataGridView_Grasshopper.Name = "DataGridView_Grasshopper"
    Me.DataGridView_Grasshopper.Size = New System.Drawing.Size(510, 298)
    Me.DataGridView_Grasshopper.TabIndex = 3
    '
    'form_DataGrid
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.BackColor = System.Drawing.SystemColors.Window
    Me.ClientSize = New System.Drawing.Size(534, 362)
    Me.Controls.Add(Me.DataGridView_Grasshopper)
    Me.Controls.Add(Me.Button1)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.Label_Slingshot)
    Me.Name = "form_DataGrid"
    Me.Text = "Slingshot! Query Result"
    CType(Me.DataGridView_Grasshopper, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents Label_Slingshot As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Button1 As System.Windows.Forms.Button
  Friend WithEvents DataGridView_Grasshopper As System.Windows.Forms.DataGridView
End Class
