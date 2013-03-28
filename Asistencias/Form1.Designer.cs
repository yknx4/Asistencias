namespace Asistencias
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAsistencia = new System.Windows.Forms.TabPage();
            this.tabRegistro = new System.Windows.Forms.TabPage();
            this.txtCuenta = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblEstado = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabAsistencia.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabAsistencia);
            this.tabControl1.Controls.Add(this.tabRegistro);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(471, 191);
            this.tabControl1.TabIndex = 0;
            // 
            // tabAsistencia
            // 
            this.tabAsistencia.Controls.Add(this.lblEstado);
            this.tabAsistencia.Controls.Add(this.button1);
            this.tabAsistencia.Controls.Add(this.label1);
            this.tabAsistencia.Controls.Add(this.txtCuenta);
            this.tabAsistencia.Location = new System.Drawing.Point(4, 25);
            this.tabAsistencia.Name = "tabAsistencia";
            this.tabAsistencia.Padding = new System.Windows.Forms.Padding(3);
            this.tabAsistencia.Size = new System.Drawing.Size(463, 162);
            this.tabAsistencia.TabIndex = 0;
            this.tabAsistencia.Text = "Asistencia";
            this.tabAsistencia.UseVisualStyleBackColor = true;
            // 
            // tabRegistro
            // 
            this.tabRegistro.Location = new System.Drawing.Point(4, 25);
            this.tabRegistro.Name = "tabRegistro";
            this.tabRegistro.Padding = new System.Windows.Forms.Padding(3);
            this.tabRegistro.Size = new System.Drawing.Size(463, 162);
            this.tabRegistro.TabIndex = 1;
            this.tabRegistro.Text = "Registro";
            this.tabRegistro.UseVisualStyleBackColor = true;
            // 
            // txtCuenta
            // 
            this.txtCuenta.Location = new System.Drawing.Point(108, 6);
            this.txtCuenta.Name = "txtCuenta";
            this.txtCuenta.Size = new System.Drawing.Size(166, 22);
            this.txtCuenta.TabIndex = 0;
            this.txtCuenta.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 1;
            this.label1.Tag = "No. de Cuenta";
            this.label1.Text = "No. de Cuenta";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(279, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Asistencia";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(7, 39);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(0, 17);
            this.lblEstado.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 215);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabAsistencia.ResumeLayout(false);
            this.tabAsistencia.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAsistencia;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCuenta;
        private System.Windows.Forms.TabPage tabRegistro;
    }
}

