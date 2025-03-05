Imports System.Data
Imports System.Data.SqlClient

Partial Class settings
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then _
        LoadCombo()
    End Sub

    Private Sub LoadCombo()
        Try
            Dim str_Connection As String = FMovers.Ticketing.DAL.Crypto.Decrypt(ConfigurationManager.ConnectionStrings("FMoversLocal").ConnectionString, "")

            Dim Cmd As New SqlCommand
            Dim DA As New SqlDataAdapter
            Dim DS_City As New DataSet
            Dim DS_Ter As New DataSet
            Dim DS_Settings As New DataSet
            Dim Con As New SqlConnection
            Dim Sql As String = ""

            Con.ConnectionString = str_Connection

            If Con.State = ConnectionState.Closed Then
                Con.Open()
            End If


            Cmd.Connection = Con
            Cmd.CommandTimeout = 0
            Cmd.CommandType = CommandType.Text

            Sql = " Select * from system_info "

            Cmd.CommandText = Sql

            DA.SelectCommand = Cmd

            DA.Fill(DS_Settings)


            Cmd.Connection = Con
            Cmd.CommandTimeout = 0
            Cmd.CommandType = CommandType.Text

            Sql = " Select * from City "

            Cmd.CommandText = Sql

            DA.SelectCommand = Cmd

            DA.Fill(DS_City, "Rep")

            'cmbCity.DataSource = DS_City.Tables(0)
            'cmbCity.DataTextField = "City_Name"
            'cmbCity.DataValueField = "City_Id"
            'cmbCity.DataBind()

            'If DS_Settings.Tables.Count > 0 Then
            '    If DS_Settings.Tables(0).Rows.Count > 0 Then
            '        For i As Integer = 0 To cmbCity.Items.Count - 1
            '            If DS_Settings.Tables(0).Rows(0)((0)) = cmbCity.Items(i).Value Then
            '                cmbCity.SelectedIndex = i
            '            End If
            '        Next
            '    End If
            'End If


            'Sql = " Select * from terminal "

            'Cmd.CommandText = Sql

            'DA.SelectCommand = Cmd

            'DA.Fill(DS_Ter, "Rep")

            'cmbTerminal.DataSource = DS_Ter.Tables(0)
            'cmbTerminal.DataTextField = "Terminal_Name"
            'cmbTerminal.DataValueField = "Terminal_Id"
            'cmbTerminal.DataBind()

            'If DS_Settings.Tables.Count > 0 Then
            '    If DS_Settings.Tables(0).Rows.Count > 0 Then
            '        For i As Integer = 0 To cmbTerminal.Items.Count - 1
            '            If DS_Settings.Tables(0).Rows(0)((1)) = cmbTerminal.Items(i).Value Then
            '                cmbTerminal.SelectedIndex = i
            '            End If
            '        Next
            '    End If
            'End If




        Catch ex As Exception
            Response.Write(ex.Message)

        End Try

    End Sub

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

    '    'Dim str_Connection As String = ConfigurationManager.ConnectionStrings("FMoversLocal").ConnectionString
    '    'Dim Cmd As New SqlCommand
    '    'Dim Con As New SqlConnection
    '    'Dim Sql As String = ""
    '    'Sql = "update system_info set Source_City_Id = " & cmbCity.SelectedValue & " , Default_Terminal = " & cmbTerminal.SelectedValue & " "
    '    'Con.ConnectionString = str_Connection

    '    'If Con.State = ConnectionState.Closed Then
    '    '    Con.Open()
    '    'End If

    '    'Cmd.Connection = Con
    '    'Cmd.CommandTimeout = 0
    '    'Cmd.CommandType = CommandType.Text
    '    'Cmd.CommandText = Sql
    '    'Cmd.ExecuteNonQuery()



    'End Sub

 
End Class
