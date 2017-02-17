Imports MySql.Data.MySqlClient

Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        conn_str()

        DataGridView1.ColumnCount = 2

        DataGridView1.Columns(0).Name = "ID"
        DataGridView1.Columns(1).Name = "NAME"

        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Retrieve_Subject()
    End Sub



    

    Private Sub Insert_Subject()
        Dim sql As String = "insert into subject(subj_name) values (@subj_name)"
        Dim cmd = New MySqlCommand(sql, conn)

        cmd.Parameters.AddWithValue("@subj_name", txtName.Text)

        Try
            conn.Open()
            If cmd.ExecuteNonQuery() > 0 Then
                MsgBox("Record has been successfully Added")
                ClearText()
            End If
            conn.Close()
            Retrieve_Subject()
        Catch ex As Exception
            MsgBox(ex.Message)
            conn.Close()
        End Try
    End Sub

    Private Sub Retrieve_Subject()
        DataGridView1.Rows.Clear()

        Dim sql As String = "select * from subject"
        Dim cmd = New MySqlCommand(sql, conn)

        Try
            conn.Open()
            adapter = New MySqlDataAdapter(cmd)
            adapter.Fill(dt)
            For Each row In dt.Rows
                Populate(row(0), row(1))
            Next
            conn.Close()
            dt.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
            conn.Close()
        End Try
    End Sub

    Private Sub Populate(ByVal id As String, ByVal name As String)
        Dim row As String() = New String() {id, name}
        DataGridView1.Rows.Add(row)
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Insert_Subject()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim id As String = DataGridView1.SelectedRows(0).Cells(0).Value()
        Update_Subject(id)
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim id As String = DataGridView1.SelectedRows(0).Cells(0).Value()
        Delete_Subject(id)
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        ClearText()
    End Sub


    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim id As String = DataGridView1.SelectedRows(0).Cells(0).Value()
        Console.WriteLine(id)
        Dim sql As String = "Select * from subject where subj_id='" + id + "'"
        Dim cmd = New MySqlCommand(sql, conn)

        Try
            conn.Open()
            reader = cmd.ExecuteReader
            Do While reader.Read()
                txtName.Text = reader.GetString(1)
            Loop
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            conn.Close()
        End Try

    End Sub

    Private Sub Delete_Subject(ByVal id As String)
        Dim sql As String = "delete from subject where subj_id='" + id + "'"
        Dim cmd = New MySqlCommand(sql, conn)

        Try
            conn.Open()
            adapter.DeleteCommand = conn.CreateCommand
            adapter.DeleteCommand.CommandText = sql
            If MessageBox.Show("Sure?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
                If cmd.ExecuteNonQuery() > 0 Then
                    MsgBox("Record has been successfully Deleted")
                    ClearText()
                End If
            End If
            conn.Close()
            Retrieve_Subject()
        Catch ex As Exception
            MsgBox(ex.Message)
            conn.Close()
        End Try
    End Sub


    Private Sub Update_Subject(ByVal id As String)
        Dim sql As String = "update subject set subj_name='" + txtName.Text + "' where subj_id ='" + id + "'"
        Try
            conn.Open()
            adapter.UpdateCommand = conn.CreateCommand
            adapter.UpdateCommand.CommandText = sql
            If adapter.UpdateCommand.ExecuteNonQuery() > 0 Then
                MsgBox("Record has been Successfully Updated")
                ClearText()
            End If
            conn.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            conn.Close()
        End Try
    End Sub


    Private Sub ClearText()
        txtName.Text = ""
    End Sub

End Class
