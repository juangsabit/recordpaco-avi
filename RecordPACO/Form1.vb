Imports Mysql.Data.MySqlClient
Public Class Form1

    'Dim str As String = "server=10.14.81.14; uid=general; pwd=Avi123!; database=recordsn"
    Dim str As String = "server=localhost; uid=root; pwd=; database=recordsn"
    Dim con As New MySqlConnection(str)
    Dim SNList As New List(Of String)

    Sub load()
        TextBox1.Clear()
        TextBox1.Select()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load()
    End Sub
    Private Sub TextBoxSN_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress, TextBox2.KeyPress, TextBox3.KeyPress
        If (e.KeyChar = Chr(13)) Then
            Dim cmd As MySqlCommand
            con.Open()
            cmd = con.CreateCommand
            cmd.CommandText = "select serial_number from fct where serial_number = '" + sender.text + "' and paco_id is null"
            Dim lrd As MySqlDataReader = cmd.ExecuteReader()
            If SNList.Contains(sender.Text) Then
                sender.backcolor = Color.Red
                sender.selectall()
                con.Close()
            Else
                If lrd.HasRows Then
                    sender.backcolor = Color.LimeGreen
                    SelectNextControl(sender, True, False, False, False)
                    SNList.Add(sender.Text)
                    con.Close()
                Else
                    sender.backcolor = Color.Red
                    sender.selectall()
                    con.Close()
                End If
            End If
        End If
    End Sub

    Private Sub TextBox4_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox4.KeyPress
        If (e.KeyChar = Chr(13)) Then
            If SNList.Count = 3 Then
                Dim cmd As MySqlCommand
                Dim id As String
                con.Open()
                cmd = con.CreateCommand
                cmd.CommandText = "SELECT AUTO_INCREMENT FROM information_schema.TABLES WHERE TABLE_SCHEMA = 'recordsn' AND TABLE_NAME = 'paco'"
                id = Convert.ToString(cmd.ExecuteScalar())
                'MsgBox(id)
                cmd.CommandText = "INSERT into paco(label, created_at)values(@label, @created);"
                cmd.Parameters.AddWithValue("@label", TextBox4.Text)
                cmd.Parameters.AddWithValue("@created", DateTime.Now)
                cmd.ExecuteNonQuery()
                For a As Integer = 0 To SNList.Count - 1
                    cmd.CommandText = "UPDATE fct SET paco_id = '" + id + "' WHERE serial_number = '" + SNList(a) + "';"
                    cmd.ExecuteNonQuery()
                Next
                con.Close()
                MsgBox("success")
            End If
        End If
    End Sub
End Class
